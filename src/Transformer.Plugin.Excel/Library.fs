namespace Transformer.Plugin.Excel

open System
open System.IO
open System.Text
open ExcelDataReader
open FSharp.Control
open Transformer.Engine

module Reader =
    type CSVReader(path: string) =
      let fileStream = File.Open(path, FileMode.Open) 
      let excelReader = 
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        ExcelReaderFactory.CreateCsvReader(fileStream)
      let getHeaders (reader: IExcelDataReader) =
        reader.Read() |> ignore
        let headers = 
          seq {
            for i = 0 to (reader.FieldCount - 1) do
              reader.GetString(i)
          }
        headers |> Array.ofSeq
      let readRowWithHeaders headers (reader: IExcelDataReader) =
        let extractValue index header = 
          (header, reader.GetString(index))
          
        headers
        |> Array.mapi extractValue
        |> Map.ofArray

      interface IDisposable with 
        member this.Dispose() = 
          fileStream.Dispose()
          excelReader.Dispose()

      interface IExtract with
        member this.Read() = 
          let mutable count = 0
          let headers = getHeaders excelReader
          seq {
            while excelReader.Read() do 
              let ordinal = excelReader.GetString(0)
              
              let entity = {
                order = count
                properties = readRowWithHeaders headers excelReader
              }
              count <- count + 1
              entity
          }