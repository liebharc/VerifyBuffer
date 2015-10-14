module TestingUtils

open System.Text.RegularExpressions
open System
open System.Reflection
open System.Resources
open System.IO

let DecorateMethodBody(methodBody : string) =
    "public class MainClass { public static void Main() { " + methodBody + "} }"

let ReadAllLines(reader: StreamReader) = seq {
    while not reader.EndOfStream do
        yield reader.ReadLine ()
}

let LoadSnippet(snippetName: Type) =
    let csName = snippetName.Name + ".cs"
    let assembly = typeof<UnitTests.Marker>.Assembly
    let resourceStream = assembly.GetManifestResourceStream(csName);
    let reader = new StreamReader(resourceStream)
    String.Join("\n", ReadAllLines(reader))

let NormalizeErrors(errors: seq<string>) =
    errors 
    |> Seq.map (fun e -> Regex.Replace(e, String.Concat("^.*?CSharpCodeSnippets\\", Path.DirectorySeparatorChar), String.Empty))
    |> Seq.map (fun e -> e.Trim())

