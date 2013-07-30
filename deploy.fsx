let exec fsi script =
    let psi = new System.Diagnostics.ProcessStartInfo(fsi)
    psi.Arguments <- script
    psi.UseShellExecute <- false
    printfn "lets try"
    let p = System.Diagnostics.Process.Start(psi)
    p.WaitForExit()
    p.ExitCode

let publishNuget(filepath:string) = 
    let arguments:string = System.String.Format( @"push ""{0}"" -s http://nuget-ait.azurewebsites.net/ 14122011", filepath)
    printfn "%s" arguments
    exec @"C:\utils\nuget.exe" arguments |> ignore

let dirPath = System.IO.Directory.GetCurrentDirectory() + "\Builded"
System.IO.Directory.EnumerateFiles(dirPath) |> Seq.iter(publishNuget)