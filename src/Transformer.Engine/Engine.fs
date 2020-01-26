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

  type ITransform = 
    abstract Convert: Entity -> Entity

  type ILoad =
    abstract Write: Entity -> unit

  type Transform(extractor: IExtract, transform: ITransform, loader: ILoad) = 
    member this.Execute () =
      extractor.Read()
      |> Seq.map transform.Convert
      |> Seq.iter loader.Write

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
