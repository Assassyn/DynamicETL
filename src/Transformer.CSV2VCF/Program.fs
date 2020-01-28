// Learn more about F# at http://fsharp.org

open System
open Transformer
open Transformer.Engine
open Transformer.Plugin.Excel.Reader
open Transformer.Actions
open Transformer.Actions.vCard.Reader

[<EntryPoint>]
let main argv =
  let path = "C:\Users\szymo\OneDrive\contacts - Copy.csv"
  
  use reader = new CSVReader(path)
  let actions = seq {
    yield (RenameAction("First Name","GivenName"):> Engine.IAction)
    yield (RenameAction("Birthday","BirthDate"):> Engine.IAction)
    yield (VCardWriter ("C:\\Users\\szymo\\OneDrive\\contacts.vcf"):> Engine.IAction)
  }
  let transform = Transform(reader, actions)
  transform.Execute()

  printfn "ETL proces has finished."
  0 // return an integer exit code
