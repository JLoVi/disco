namespace Iris.Client

// * Imports

open System
open System.Threading
open System.Collections.Concurrent
open Iris.Core
open Iris.Client
open Iris.Zmq
open Iris.Serialization.Api

// * ApiClient module

//   ____ _ _            _
//  / ___| (_) ___ _ __ | |_
// | |   | | |/ _ \ '_ \| __|
// | |___| | |  __/ | | | |_
//  \____|_|_|\___|_| |_|\__|

[<AutoOpen>]
module ApiClient =

  //  ____       _            _
  // |  _ \ _ __(_)_   ____ _| |_ ___
  // | |_) | '__| \ \ / / _` | __/ _ \
  // |  __/| |  | |\ V / (_| | ||  __/
  // |_|   |_|  |_| \_/ \__,_|\__\___|

  // ** tag

  let private tag (str: string) = sprintf "ApiClient.%s" str

  // ** Subscriptions

  type private Subscriptions = ConcurrentDictionary<int, IObserver<ClientEvent>>

  // ** ClientStateData

  [<NoComparison;NoEquality>]
  type private ClientStateData =
    { Server: Rep
      Socket: Req
      Store:  Store }

    interface IDisposable with
      member self.Dispose() =
        dispose self.Server
        dispose self.Socket

  // ** ClientState

  [<NoComparison;NoEquality>]
  type private ClientState =
    | Loaded of ClientStateData
    | Idle

    interface IDisposable with
      member self.Dispose() =
        match self with
        | Loaded data -> dispose data
        | Idle -> ()

  // ** Reply

  [<RequireQualifiedAccess>]
  type private Reply =
    | State of State
    | Ok

  // ** ReplyChan

  type private ReplyChan = AsyncReplyChannel<Either<IrisError,Reply>>

  // ** Msg

  [<RequireQualifiedAccess;NoComparison;NoEquality>]
  type private Msg =
    | Start    of chan:ReplyChan
    | Dispose  of chan:ReplyChan
    | GetState of chan:ReplyChan

  // ** ApiAgent

  type private ApiAgent = MailboxProcessor<Msg>

  // ** Listener

  type private Listener = IObservable<ClientEvent>

  // ** createListener

  let private createListener (subscriptions: Subscriptions) =
    { new Listener with
        member self.Subscribe(obs) =
          while not (subscriptions.TryAdd(obs.GetHashCode(), obs)) do
            Thread.Sleep(1)

          { new IDisposable with
              member self.Dispose() =
                match subscriptions.TryRemove(obs.GetHashCode()) with
                | true, _  -> ()
                | _ -> subscriptions.TryRemove(obs.GetHashCode())
                      |> ignore } }

  // ** requestHandler

  let private requestHandler (agent: ApiAgent) (raw: byte array) =
    match Binary.decode raw with
    | Right ClientApiRequest.Ping ->
      ApiResponse.Pong
      |> Binary.encode
    | Left error ->
      string error
      |> ApiError.MalformedRequest
      |> ApiResponse.NOK
      |> Binary.encode

  // ** requestRegister

  let private requestRegister (data: ClientStateData) (client: IrisClient) =
    let response =
      client
      |> ServerApiRequest.Register
      |> Binary.encode
      |> data.Socket.Request
      |> Either.bind Binary.decode

    match response with
    | Right OK -> Either.succeed ()
    | Right (NOK error) ->
      string error
      |> Error.asClientError (tag "start")
      |> Either.fail
    | Right Pong ->
      "Unexpected Resposne from server: Pong"
      |> Error.asClientError (tag "start")
      |> Either.fail
    | Left error ->
      error
      |> Either.fail

  // ** start

  let private start (chan: ReplyChan)
                    (server: IrisServer)
                    (client: IrisClient)
                    (agent: ApiAgent) =
    let clientAddr = formatUri client.IpAddress (int client.Port)
    let srvAddr = formatUri server.IpAddress (int server.Port)
    let server = new Rep(clientAddr, requestHandler agent)
    let socket = new Req(client.Id, srvAddr, 50)
    let store = new Store(State.Empty)
    match server.Start(), socket.Start() with
    | Right (), () ->
      let data =
        { Store = store
          Socket = socket
          Server = server }

      match requestRegister data client with
      | Right () ->
        chan.Reply(Right Reply.Ok)
        Loaded data
      | Left error ->
        dispose data
        chan.Reply(Left error)
        Idle
    | Left error, _ ->
      chan.Reply(Left error)
      dispose server
      dispose socket
      Idle

  // ** handleStart

  let private handleStart (chan: ReplyChan)
                          (state: ClientState)
                          (server: IrisServer)
                          (client: IrisClient)
                          (agent: ApiAgent) =
    match state with
    | Loaded data ->
      dispose data
      start chan server client agent
    | Idle ->
      start chan server client agent

  // ** handleDispose

  let private handleDispose (chan: ReplyChan) (state: ClientState) =
    dispose state
    chan.Reply(Right Reply.Ok)
    Idle

  // ** handleGetState

  let private handleGetState (chan: ReplyChan) (state: ClientState) =
    match state with
    | Loaded data ->
      chan.Reply(Right (Reply.State data.Store.State))
      state
    | Idle ->
      "Not loaded"
      |> Error.asClientError (tag "handleGetState")
      |> Either.fail
      |> chan.Reply
      Idle

  // ** loop

  let private loop (initial: ClientState)
                   (server: IrisServer)
                   (client: IrisClient)
                   (subs: Subscriptions)
                   (inbox: ApiAgent) =
    let rec act (state: ClientState) =
      async {
        let! msg = inbox.Receive()

        let newstate =
          match msg with
          | Msg.Start chan    -> handleStart chan state server client inbox
          | Msg.Dispose chan  -> handleDispose chan state
          | Msg.GetState chan -> handleGetState chan state

        return! act newstate
      }
    act initial

  // ** ApiClient module

  [<RequireQualifiedAccess>]
  module ApiClient =

    // ** create

    let create (server: IrisServer) (client: IrisClient) =
      either {
        let cts = new CancellationTokenSource()
        let subs = new Subscriptions()
        let agent = new ApiAgent(loop Idle server client subs, cts.Token)
        let listener = createListener subs
        agent.Start()

        return
          { new IApiClient with
              member self.Start () =
                match agent.PostAndReply(fun chan -> Msg.Start chan) with
                | Right (Reply.Ok) -> Either.succeed ()
                | Right other ->
                  sprintf "Unexpected Reply from ApiAgent: %A" other
                  |> Error.asClientError (tag "Start")
                  |> Either.fail
                | Left error ->
                  error
                  |> Either.fail

              member self.State
                with get () =
                  match agent.PostAndReply(fun chan -> Msg.GetState(chan)) with
                  | Right (Reply.State state) -> Either.succeed state
                  | Right other ->
                    sprintf "Unexpected Reply from ApiAgent: %A" other
                    |> Error.asClientError (tag "State")
                    |> Either.fail
                  | Left error ->
                    error
                    |> Either.fail

              member self.Subscribe (callback: ClientEvent -> unit) =
                { new IObserver<ClientEvent> with
                    member self.OnCompleted() = ()
                    member self.OnError(error) = ()
                    member self.OnNext(value) = callback value }
                |> listener.Subscribe

              member self.Dispose () =
                agent.PostAndReply(fun chan -> Msg.Dispose chan)
                |> ignore
                dispose cts
            }
      }
