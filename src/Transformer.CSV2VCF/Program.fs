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

  reader.read()
  ||> rename "First Name" "GivenName"
  ||> rename "Birthday" "BirthDate" 
  ||> writeVCard "C:\\Users\\szymo\\OneDrive\\contacts.vcf"
  |> Engine.execute

  printfn "ETL proces has finished."
  0 // return an integer exit code
