namespace Transformer.Plugin.Excel

open Transformer.Engine
open FSharp.Control

module Reader =
    type ExcelReader(path: string, sheetName: string) =
      interface IExtract with
        member this.Read() = 
           asyncSeq {
             let testEntity = {
               order = 1
               properties = Map.empty<string, string>
             }
             yield testEntity
           }