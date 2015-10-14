module VerifyBuffering.Main

open VerifyBuffering.Parser
open VerifyBuffering.Analysis

[<EntryPoint>]
let main argv = 
    let messages = Array.toList(argv) |> List.map ParseFile |> List.collect Analyze
    messages |> List.iter (fun m -> printfn "%A" m)
    0 // return an integer exit code