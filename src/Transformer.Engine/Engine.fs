namespace Transformer

open System.Collections
open FSharp.Control

module Engine =
  type Entity = 
    {
      order: int
      properties: Map<string, string>
    }

  type IExtract = unit -> Entity seq

  type IAction = Entity -> Entity
  
  let (||>) (entities: Entity seq) action =
    entities |> Seq.map action
  
  let execute (entities: Entity seq) = 
    entities |> Seq.iter (fun entity -> ())

module Actions =
  open Engine

  let rename oldName newName entity =
    let renameProperty item =
      let oldValue = item.properties.[oldName]
      let properties = item.properties.Add(newName, oldValue)
      {
        order = item.order
        properties = properties
      }
    match entity.properties.ContainsKey oldName with 
    | true -> renameProperty entity
    | _ -> entity
