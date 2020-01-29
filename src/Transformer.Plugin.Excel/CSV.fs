namespace Transformer.Plugin.Excel

open System
open System.IO
open System.Text
open ExcelDataReader
open Transformer.Engine

module Reader =
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
      let read path =
        let mutable count = 0
        use fileStream = File.Open(path, FileMode.Open) 
        use excelReader = 
          Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
          ExcelReaderFactory.CreateCsvReader(fileStream)
        let headers = getHeaders excelReader
        seq {
          while excelReader.Read() do 
            let entity = {
              order = count
              properties = readRowWithHeaders headers excelReader
            }
            count <- count + 1
            entity
        }