[<ReflectedDefinition>]
module Test.Units.VirtualDom

#nowarn "1182"

open FunScript
open FunScript.TypeScript
open FunScript.Mocha
open FunScript.VirtualDom
open FSharp.Html

open Iris.Web.Dom
open Iris.Web.Test.Util

let main () =
  suite "Test.Units.VirtualDom" 

  withContent <| fun content -> 
    test "should add new element to list on diff/patch" <|
      (fun cb ->
       let litem = li <|> text "an item"
       let comb  = ul <||> [ litem ]

       let tree = htmlToVTree comb
       let root = createElement tree

       content.appendChild root |> ignore

       check (root.children.length = 1.) "ul item count does not match (expected 1)"
       
       let newtree = htmlToVTree <| (comb <|> litem)
       let newroot = patch root <| diff tree newtree
       
       check_cc (newroot.children.length = 2.) "ul item count does not match (expected 2)" cb)


  withContent <| fun content -> 
    test "addChild should add new element to VTree children array" <|
      (fun cb ->
       let litem = li <|> text "an item"
       let comb  = ul <||> [ litem ]

       let tree = htmlToVTree comb
       let root = createElement tree

       content.appendChild root |> ignore

       check (root.children.length = 1.) "ul item count does not match (expected 1)"
       
       let newtree = addChild tree <| htmlToVTree litem
       let newroot = patch root <| diff tree newtree
       
       check_cc (newroot.children.length = 2.) "ul item count does not match (expected 2)" cb)

  withContent <| fun content -> 
    test "addChildren should add a list of new VTree children" <|
      (fun cb ->
       let litem = li <|> text "an item"
       let comb  = ul <||> [ litem ]

       let tree = htmlToVTree comb
       let root = createElement tree

       content.appendChild root |> ignore

       check (root.children.length = 1.) "ul item count does not match (expected 1)"
       
       let newtree = addChildren tree <| Array.map htmlToVTree [| litem; litem |]
       let newroot = patch root <| diff tree newtree
       
       check_cc (newroot.children.length = 3.) "ul item count does not match (expected 2)" cb)

// withContent <| fun content -> 
  //   test "patching should update relevant bits of the dom" <|
  //     (fun cb ->
  //      let firstContent = "first item in the list"
  //      let secondContent = "second item in the list"

  //      let list content =
  //        ul <||>
  //           [ li <@> id' "first"  <|> text content
  //           ; li <@> id' "second" <|> text secondContent
  //           ]

  //      let tree = list firstContent |> htmlToVTree
  //      let root = createElement tree

  //      
  //      )