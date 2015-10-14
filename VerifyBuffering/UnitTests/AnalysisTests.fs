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
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.DoubleArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal 
            [
                "DoubleArrayCreation.cs line 24: Numeric array construction";
                "DoubleArrayCreation.cs line 34: Numeric array construction" // This is a known limitation of our siimplfied approach to evaluate the source code
            ]

    [<Test>]
    member test.``Find float array constructor``() = 
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.FloatArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal ["FloatArrayCreation.cs line 13: Numeric array construction"]

    [<Test>]
    member test.``Find int array constructor``() = 
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.IntArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should be Empty

    [<Test>]
    member test.``Find vector class constructor``() = 
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.DspVectorCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal ["DspVectorCreation.cs line 13: Vector construction"]

    [<Test>]
    member test.``Ignore string array constructor``() = 
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.StringArrayCreation>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should be Empty
    [<Test>]
    member test.``Data buffer usage test``() = 
        let syntaxTree = ParseFile(SnippetPath(typeof<CSharpCodeSnippets.DataBufferUsage>)) 
        let result = Analyze(syntaxTree)
        result |> NormalizeErrors |> should equal 
            [
                "DataBufferUsage.cs line 27: IDataBuffer.NewChild call missing";
                "DataBufferUsage.cs line 32: IDataBuffer may not be used more than once per line";
                "DataBufferUsage.cs line 49: IDataBuffer.NewChildIteration call missing";
                "DataBufferUsage.cs line 51: IDataBuffer.NewChildIteration call missing";
                "DataBufferUsage.cs line 66: IDataBuffer.NewChildIteration call missing"
            ]