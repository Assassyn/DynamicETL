// Learn more about F# at http://fsharp.org

open System
open Transformer
open Transformer.Engine
open Transformer.Actions
open Transformer.Actions.vCard.Reader

[<EntryPoint>]
let main argv =
  let path = "C:\Users\szymo\OneDrive\contacts.csv"
  
  (CSV.read path)
  ||> rename "First Name" "GivenName"
  ||> rename "Birthday" "BirthDate" 
  ||> writeVCard "C:\\Users\\szymo\\OneDrive\\contacts.vcf"
  |> Engine.execute

  printfn "ETL proces has finished."
  0 // return an integer exit code
