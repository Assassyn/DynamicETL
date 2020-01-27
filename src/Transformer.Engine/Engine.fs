namespace Transformer

open System.Collections
open FSharp.Control

module Engine =
  type Entity = 
    {
      order: int
      properties: Map<string, string>
    }

  type IExtract =
    abstract Read: unit -> Entity seq

  type IAction =
    abstract Execute: Entity -> Entity

  type Transform(initialExtracxt: IExtract, actions: IAction seq) = 
    member this.Execute () =
      let data = initialExtracxt.Read()
      let executreAction (actions: IAction seq) (item: Entity) = actions |> Seq.fold (fun acc elem -> elem.Execute(acc)) item |> ignore
      data 
      |> Seq.iter (executreAction actions)

module Actions =
  open Engine

  type RenameAction(oldName: string, newName:string) =
    interface IAction with
      member this.Execute entity =
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

module Empty = 
  type Transform() = 
    interface Engine.IAction with 
      member this.Execute(entity: Engine.Entity) =
        entity

  let transform = new Transform()