namespace Iris.Service.Groups

open System.Collections.Generic
open Nessos.FsPickler
open Iris.Core.Types
open Vsync

[<AutoOpen>]
module CueGroup =

  type Id = string

  type CueDict = Dictionary<Id,Cue>

  (* ---------- CueAction ---------- *)

  type CueAction =
    | Add 
    | Update 
    | Delete 
    interface IEnum with
      member self.ToInt() : int =
        match self with
          | Add    -> 1
          | Update -> 2 
          | Delete -> 3 

  (* ---------- CueGroup ---------- *)

  type CueGroup(grpname) as self = 
    [<DefaultValue>] val mutable group : IrisGroup<CueAction,Cue>

    let mutable cues : CueDict = new CueDict()

    let AddHandler(action, cb) =
      self.group.AddHandler(action, cb)

    let AllHandlers =
      [ (CueAction.Add,    self.CueAdded)
      ; (CueAction.Update, self.CueUpdated)
      ; (CueAction.Delete, self.CueDeleted)
      ]

    (* constructor *)
    do
      self.group <- new IrisGroup<CueAction,Cue>(grpname)
      self.group.AddInitializer(self.Initialize)
      self.group.AddViewHandler(self.ViewChanged)
      self.group.CheckpointMaker(self.MakeCheckpoint)
      self.group.CheckpointLoader(self.LoadCheckpoint)
      List.iter AddHandler AllHandlers

    member self.Dump() =
      for cue in cues do
        printfn "cue id: %s" cue.Key

    member self.Add(c : Cue) =
      cues.Add(c.Id, c)

    (* Become member of group *)
    member self.Join() = self.group.Join()

    (* CueAction on the group *)
    member self.Send(action : CueAction, cue : Cue) =
      self.group.Send(action, cue)

    (* State initialization and transfer *)
    member self.Initialize() =
      printfn "should load state from disk/vvvv now"

    member self.MakeCheckpoint(view : View) =
      printfn "makeing a snapshot. %d cues in it" cues.Count
      for pair in cues do
        self.group.SendCheckpoint(pair.Value)
      self.group.DoneCheckpoint()

    member self.LoadCheckpoint(cue : Cue) =
      if cues.ContainsKey(cue.Id)
      then cues.[cue.Id] <- cue
      else cues.Add(cue.Id, cue)
      printfn "loaded a snapshot. %d cues in it" cues.Count

    (* View changes *)
    member self.ViewChanged(view : View) : unit =
      printfn "viewid: %d" <| view.GetViewid() 

    (* Event Handlers for CueAction *)
    member self.CueAdded(cue : Cue) : unit =
      if not <| cues.ContainsKey(cue.Id)
      then
        self.Add(cue)
        printfn "cue added cb: "
        self.Dump()

    member self.CueUpdated(cue : Cue) : unit =
      printfn "%s updated" cue.Name

    member self.CueDeleted(cue : Cue) : unit =
      printfn "%s removed" cue.Name