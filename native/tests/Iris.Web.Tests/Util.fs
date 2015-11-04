namespace Iris.Web.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery

[<JavaScript>]
module Util = 
  
  let elById (id : string) : Dom.Element = JS.Document.GetElementById id
  
  let mkContent () : Dom.Element =
    let el = JS.Document.CreateElement "div"
    el.id <- "content"
    JQuery.Of("body").Append el |> ignore
    el
    
  let cleanup (el : Dom.Element) : unit =
    failwith "cleanup needs implementing"
  
  let withContent (wrapper : HTMLElement -> unit) : unit =
    let content = mkContent () 
    wrapper content