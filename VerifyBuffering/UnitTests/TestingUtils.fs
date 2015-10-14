module TestingUtils

open System.Text.RegularExpressions
open System
open System.IO

let DecorateMethodBody(methodBody : string) =
    "public class MainClass { public static void Main() { " + methodBody + "} }"

let SnippetPath(snippetName: Type) =
    let currentLocation = Path.GetDirectoryName(typeof<CSharpCodeSnippets.DoubleArrayCreation>.Assembly.Location)
    Path.Combine(currentLocation, "..", "..", "..", "CSharpCodeSnippets", snippetName.Name + ".cs")

let NormalizeErrors(errors: seq<string>) =
    errors |> Seq.map (fun e -> Regex.Replace(e, String.Concat("^.*?CSharpCodeSnippets\\", Path.DirectorySeparatorChar), String.Empty))

