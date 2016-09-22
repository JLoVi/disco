namespace Iris.Core

#if JAVASCRIPT

open Fable.Core
open Fable.Import
open Fable.Import.JS
open Iris.Core.FlatBuffers

#else

open FlatBuffers
open Iris.Serialization.Raft

#endif

#if JAVASCRIPT

//   ____           _____ ____
//  / ___|   _  ___|  ___| __ )
// | |  | | | |/ _ \ |_  |  _ \
// | |__| |_| |  __/  _| | |_) |
//  \____\__,_|\___|_|   |____/

[<Import("Iris", from="buffers")>]
module CueFBSerialization =

  open Iris.Core.FlatBuffers

  type CueFB =
    [<Emit("$0.Id()")>]
    abstract Id: string

    [<Emit("$0.Name()")>]
    abstract Name: string

  type CueFBConstructor =
    abstract prototype: CueFB with get, set

    [<Emit("Iris.Serialization.Raft.CueFB.startCueFB($1)")>]
    abstract StartCueFB: builder: FlatBufferBuilder -> unit

    [<Emit("Iris.Serialization.Raft.CueFB.addId($1, $2)")>]
    abstract AddId: builder: FlatBufferBuilder * id: Offset<string> -> unit

    [<Emit("Iris.Serialization.Raft.CueFB.addName($1, $2)")>]
    abstract AddName: builder: FlatBufferBuilder * name: Offset<string> -> unit

    [<Emit("Iris.Serialization.Raft.CueFB.endCueFB($1)")>]
    abstract EndCueFB: builder: FlatBufferBuilder -> Offset<'a>

    [<Emit("Iris.Serialization.Raft.CueFB.getRootAsCueFB($1)")>]
    abstract GetRootAsCueFB: buffer: ByteBuffer -> CueFB

    // [<Emit("new .$0($1)")>]
    // abstract Create: unit -> CueFB

  let CueFB : CueFBConstructor = failwith "JS only"


open CueFBSerialization

#endif

type Cue =
  { Id:      Id
  ; Name:    string
  ; IOBoxes: IOBox array }

  //  ____  _
  // | __ )(_)_ __   __ _ _ __ _   _
  // |  _ \| | '_ \ / _` | '__| | | |
  // | |_) | | | | | (_| | |  | |_| |
  // |____/|_|_| |_|\__,_|_|   \__, |
  //                           |___/

  static member FromFB(fb: CueFB) : Cue option =
    let ioboxes = Array.zeroCreate fb.IOBoxesLength

    for i in 0 .. (fb.IOBoxesLength - 1) do
      let iobox = fb.IOBoxes(i)
      if iobox.HasValue then
        iobox.Value
        |> IOBox.FromFB
        |> Option.map (fun iobox -> ioboxes.[i] <- iobox)
        |> ignore

    try
      { Id = Id fb.Id
      ; Name = fb.Name
      ; IOBoxes = ioboxes
      } |> Some
    with
      | exn -> None

  member self.ToOffset(builder: FlatBufferBuilder) : Offset<CueFB> =
    let id = string self.Id |> builder.CreateString
    let name = self.Name |> builder.CreateString
    let ioboxoffsets = Array.map (fun (iobox: IOBox) -> iobox.ToOffset(builder)) self.IOBoxes
    let ioboxes = CueFB.CreateIOBoxesVector(builder, ioboxoffsets)
    CueFB.StartCueFB(builder)
    CueFB.AddId(builder, id)
    CueFB.AddName(builder, name)
    CueFB.AddIOBoxes(builder, ioboxes)
    CueFB.EndCueFB(builder)

  static member FromBytes(bytes: Binary.Buffer) : Cue option =
    CueFB.GetRootAsCueFB(Binary.createBuffer bytes)
    |> Cue.FromFB

  member self.ToBytes() = Binary.buildBuffer self
