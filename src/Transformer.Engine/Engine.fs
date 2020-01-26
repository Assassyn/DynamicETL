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

  type ITransform = 
    abstract Convert: Entity -> Entity

  type ILoad =
    abstract Write: Entity -> unit

  type Transform(initialExtracxt: IExtract, actions: IAction seq) = 
    member this.Execute () =
      let data = initialExtracxt.Read()
      let executreAction (actions: IAction seq) (item: Entity) = actions |> Seq.fold (fun acc elem -> elem.Execute(acc)) item
      data 
      |> Seq.map (executreAction actions)

module Empty = 
  type Transform() = 
    interface Engine.ITransform with 
      member this.Convert(entity: Engine.Entity) =
        entity
  type Load() =
    interface Engine.ILoad with
      member this.Write(entity) =
        ()

  let transform = new Transform()
  let load = new Load()
