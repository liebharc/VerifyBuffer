namespace UnitTests

open System
open VerifyBuffering.Parser
open NUnit.Framework
open FsUnit
open TestingUtils

[<TestFixture>]
type ParserTests() =
    [<Test>]
    member test.``Valid C# program code block test``() = 
        let syntaxTree = ParseProgramCode(DecorateMethodBody("double[] x = new double[0];")) 
        syntaxTree.Errors |> should be Empty

    [<Test>]
    member test.``Valid C# snippet test``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.DoubleArrayCreation>)) 
        syntaxTree.Errors |> should be Empty