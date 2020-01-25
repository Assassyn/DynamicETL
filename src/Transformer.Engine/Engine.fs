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
    abstract Read: unit -> AsyncSeq<Entity>

  type ITransform = 
    abstract Convert: Entity -> Async<Entity>

  type ILoad =
    abstract Write: Entity -> Async<unit>

  type Transform(extractor: IExtract, transform: ITransform, loader: ILoad) = 
    member this.Execute () =
      extractor.Read()
      |> AsyncSeq.mapAsync transform.Convert
      |> AsyncSeq.iterAsync loader.Write
      |> Async.RunSynchronously

module Empty = 
  type Transform() = 
    interface Engine.ITransform with 
      member this.Convert(entity: Engine.Entity) =
        async {
          return entity
        }
  type Load() =
    interface Engine.ILoad with
      member this.Write(entity) =
        async {
          ()
        }

  let transform = new Transform()
  let load = new Load()
