// Learn more about F# at http://fsharp.org

open System
open Transformer
open Transformer.Plugin.Excel.Reader

[<EntryPoint>]
let main argv =
  let path = "C:\Users\szymo\OneDrive\contacts.csv"
  let reader = ExcelReader(path, "contacts")
  let transform = Engine.Transform(reader, Empty.transform, Empty.load )
  transform.Execute()
  printfn "Hello World from F#!"
  0 // return an integer exit code
