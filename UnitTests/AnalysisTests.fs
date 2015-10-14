namespace UnitTests

open System
open VerifyBuffering.Parser
open NUnit.Framework
open FsUnit
open TestingUtils
open VerifyBuffering.Analysis

[<TestFixture>]
type AnalysisTests() =
    [<Test>]
    member test.``Find double array constructor``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.DoubleArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal 
            [
                "line 24: Numeric array construction";
                "line 34: Numeric array construction" // This is a known limitation of our siimplfied approach to evaluate the source code
            ]

    [<Test>]
    member test.``Find float array constructor``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.FloatArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal ["line 13: Numeric array construction"]

    [<Test>]
    member test.``Find int array constructor``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.IntArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should be Empty

    [<Test>]
    member test.``Find vector class constructor``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.DataVectorCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal ["line 13: Vector construction"]

    [<Test>]
    member test.``Ignore string array constructor``() = 
        let syntaxTree = ParseProgramCode(LoadSnippet(typeof<CSharpCodeSnippets.StringArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should be Empty