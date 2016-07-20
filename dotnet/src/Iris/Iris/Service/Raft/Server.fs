module Iris.Service.Raft.Server

open System
open System.Threading
open Iris.Core
open Iris.Core.Utils
open Pallet.Core
open fszmq
open fszmq.Context
open fszmq.Socket
open fszmq.Polling
open FSharpx.Stm
open FSharpx.Functional
open Utilities
open Stm
open Db

//  ____        __ _     ____                             ____  _        _
// |  _ \ __ _ / _| |_  / ___|  ___ _ ____   _____ _ __  / ___|| |_ __ _| |_ ___
// | |_) / _` | |_| __| \___ \ / _ \ '__\ \ / / _ \ '__| \___ \| __/ _` | __/ _ \
// |  _ < (_| |  _| |_   ___) |  __/ |   \ V /  __/ |     ___) | || (_| | ||  __/
// |_| \_\__,_|_|  \__| |____/ \___|_|    \_/ \___|_|    |____/ \__\__,_|\__\___|

type RaftServerState =
  | Starting
  | Running
  | Stopping
  | Stopped
  | Failed

[<AutoOpen>]
module RaftServerStateHelpers =

  let hasFailed = function
    | Failed -> true
    | _      -> false


//  ____        __ _     ____
// |  _ \ __ _ / _| |_  / ___|  ___ _ ____   _____ _ __
// | |_) / _` | |_| __| \___ \ / _ \ '__\ \ / / _ \ '__|
// |  _ < (_| |  _| |_   ___) |  __/ |   \ V /  __/ |
// |_| \_\__,_|_|  \__| |____/ \___|_|    \_/ \___|_|

type RaftServer(options: RaftOptions, context: fszmq.Context) as this =
  let max_retry = 5
  let timeout = 10UL

  let database = openDB (options.DataDir </> DB_NAME)

  let serverState = ref Stopped

  let servertoken   = ref None
  let workertoken   = ref None
  let periodictoken = ref None

  let cbs = this :> IRaftCallbacks<_,_>
  let appState = mkState context options |> newTVar

  let requestWorker =
    let cts = new CancellationTokenSource()
    workertoken := Some cts
    new Actor<(Node<IrisNode> * RaftRequest)> (requestLoop appState cbs, cts.Token)

  //                           _
  //  _ __ ___   ___ _ __ ___ | |__   ___ _ __ ___
  // | '_ ` _ \ / _ \ '_ ` _ \| '_ \ / _ \ '__/ __|
  // | | | | | |  __/ | | | | | |_) |  __/ |  \__ \
  // |_| |_| |_|\___|_| |_| |_|_.__/ \___|_|  |___/

  /// ## Start the Raft engine
  ///
  /// Start the Raft engine and start processing requests.
  ///
  /// ### Signature:
  /// - unit: unit
  ///
  /// Returns: unit
  member self.Start() =
    try
      stm {
        serverState := Starting

        requestWorker.Start()

        do!  initialize appState cbs
        let! srvtkn = startServer appState cbs
        let! prdtkn = startPeriodic timeout appState cbs

        servertoken   := Some srvtkn
        periodictoken := Some prdtkn

        serverState := Running

      } |> atomically
    finally
      serverState := Failed

  /// ## Stop the Raft engine, sockets and all.
  ///
  /// Description
  ///
  /// ### Signature:
  /// - arg: arg
  /// - arg: arg
  /// - arg: arg
  ///
  /// Returns: Type
  member self.Stop() =
    match !serverState with
      | Starting | Stopping | Stopped | Failed _ -> ()
      | Running ->
        stm {
          serverState := Stopping

          // cancel the running async tasks
          cancelToken periodictoken
          cancelToken servertoken
          cancelToken workertoken

          let! state = readTVar appState

          // disconnect all cached sockets
          state.Connections
          |> Map.iter (fun (mid: MemberId) (sock: Socket) ->
                      let nodeinfo = List.tryFind (fun c -> c.MemberId = mid) state.Clients
                      match nodeinfo with
                        | Some info -> formatUri info |> disconnect sock
                        | _         -> ())

          do! writeTVar appState { state with Connections = Map.empty }

          serverState := Stopped

          failwith "STOP SHOULD ALSO PERSIST LAST STATE TO DISK"
        } |> atomically

  member self.Options
    with get () =
      let state = readTVar appState |> atomically
      state.Options
    and set opts =
      stm {
        let! state = readTVar appState
        do! writeTVar appState { state with Options = opts }
      } |> atomically

  member self.Context
    with get () = context

  /// Alas, we may only *look* at the current state.
  member self.State
    with get () = atomically (readTVar appState)

  member self.Append entry =
    appendEntry entry appState cbs
    |> atomically

  member self.EntryCommitted resp =
    stm {
      let! state = readTVar appState

      let committed =
        match responseCommitted resp |> runRaft state.Raft cbs with
        | Right (committed, _) -> committed
        | _                    -> false

      return committed
    } |> atomically

  member self.ForceTimeout() =
    failwith "FIXME: ForceTimeout"

  member self.Log msg =
    let state = self.State
    cbs.LogMsg state.Raft.Node msg

  member self.ServerState with get () = !serverState

  //  ____  _                           _     _
  // |  _ \(_)___ _ __   ___  ___  __ _| |__ | | ___
  // | | | | / __| '_ \ / _ \/ __|/ _` | '_ \| |/ _ \
  // | |_| | \__ \ |_) | (_) \__ \ (_| | |_) | |  __/
  // |____/|_|___/ .__/ \___/|___/\__,_|_.__/|_|\___|
  //             |_|

  interface IDisposable with
    member self.Dispose() =
      self.Stop()

  //  ____        __ _     ___       _             __
  // |  _ \ __ _ / _| |_  |_ _|_ __ | |_ ___ _ __ / _| __ _  ___ ___
  // | |_) / _` | |_| __|  | || '_ \| __/ _ \ '__| |_ / _` |/ __/ _ \
  // |  _ < (_| |  _| |_   | || | | | ||  __/ |  |  _| (_| | (_|  __/
  // |_| \_\__,_|_|  \__| |___|_| |_|\__\___|_|  |_|  \__,_|\___\___|

  interface IRaftCallbacks<StateMachine,IrisNode> with
    member self.SendRequestVote node req  =
      let state = self.State
      (node, RequestVote(state.Raft.Node.Id,req))
      |> requestWorker.Post

    member self.SendAppendEntries node ae =
      let state = self.State
      (node, AppendEntries(state.Raft.Node.Id, ae))
      |> requestWorker.Post

    member self.SendInstallSnapshot node is =
      let state = self.State
      (node, InstallSnapshot(state.Raft.Node.Id, is))
      |> requestWorker.Post

    member self.ApplyLog sm      = failwith "FIXME: ApplyLog"
    member self.NodeAdded node   = failwith "FIXME: Node was added."
    member self.NodeUpdated node = failwith "FIXME: Node was updated."
    member self.NodeRemoved node = failwith "FIXME: Node was removed."
    member self.Configured nodes = failwith "FIXME: Cluster configuration done."
    member self.StateChanged o n = failwithf "FIXME: State changed from %A to %A" o n

    member self.PrepareSnapshot raft = failwith "FIXME: PrepareSnapshot"
    member self.RetrieveSnapshot () = failwith "FIXME: RetrieveSnapshot"
    member self.PersistSnapshot log = failwith "FIXME: PersistSnapshot"
    member self.PersistVote node = failwith "FIXME: PersistVote"
    member self.PersistTerm node = failwith "FIXME: PersistTerm"
    member self.PersistLog log   = failwith "FIXME: LogOffer"
    member self.DeleteLog log    = failwith "FIXME: LogPoll"

    member self.HasSufficientLogs node = failwith "FIXME: HasSufficientLogs"

    member self.LogMsg node str =
      if options.Debug then
        printfn "%s" str
