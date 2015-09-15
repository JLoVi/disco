namespace Iris.Web

open FunScript
open FunScript.TypeScript
open FunScript.TypeScript.virtualDom

[<FunScript.JS>]
module DOM =
  let hello () =
    let tree = virtualDom.Globals.h("div#hello", Array.empty)
    virtualDom.Globals.create tree