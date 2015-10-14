module VerifyBuffering.Parser

open ICSharpCode.NRefactory.CSharp
open System.IO;

let ParseProgramCode(programCode: string) = 
    let parser = new CSharpParser()
    parser.Parse(programCode)

let ParseFile(fileName: string) =
    let content = File.ReadAllText(fileName)
    let parser = new CSharpParser()
    parser.Parse(content, fileName)

