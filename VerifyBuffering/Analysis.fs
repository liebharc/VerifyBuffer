module VerifyBuffering.Analysis

open System
open System.Linq
open ICSharpCode.NRefactory.CSharp

let ExpensiveObject = "DataVector"
let IgnoreErrorWithConstantSizeLessThan = 10

let Analyze(tree: SyntaxTree) =
    let IsIntegerPrimitive(t: PrimitiveType) =
        match t.Keyword.ToLowerInvariant() with
        | "int" -> true
        | "long"  -> true
        | _ -> false
    let IsNumericPrimitive(t: PrimitiveType) =
        match t.Keyword.ToLowerInvariant() with
        | "double" -> true
        | "float"  -> true
        | _ -> false
    let IsNumericType(t: AstType) = 
        match t with
        | :? PrimitiveType as prim 
                when IsNumericPrimitive(prim)
                -> true
        | _ -> false
    let IsStackedArray(t: ArrayCreateExpression) = 
        t.AdditionalArraySpecifiers |> Seq.isEmpty |> not
    let IsObjectEqualOrLessThan(value: obj, threshold: int) =
        match value with
        | :? int as intValue -> intValue <= threshold
        | _ -> false
    let IsSmallInteger(t: Expression) =
        match t with
        | :? PrimitiveExpression as prim 
                when IsObjectEqualOrLessThan(prim.Value, IgnoreErrorWithConstantSizeLessThan) -> true
        | _ -> false
    let IsSmallConstant(t: Expression, constants: System.Collections.Generic.List<string>) =
        match t with
        | :? IdentifierExpression as identfier
            -> constants.Contains(identfier.Identifier)
        | _ -> false
    let IsArrayOfSmallSize(t: ArrayCreateExpression, constants: System.Collections.Generic.List<string>) =
        IsSmallInteger(t.Arguments.FirstOrDefault()) ||
        IsSmallConstant(t.Arguments.FirstOrDefault(), constants)
    let IsExpensiveObjectConstruction(t: ObjectCreateExpression) =
        t.ToString().Contains(ExpensiveObject)
    let rec AnalyzeAssignment(node: AstNode) : Option<string> =
        match node with 
        | null -> None
        | :? VariableInitializer as assignment 
            when IsSmallInteger(assignment.Initializer) &&
                 not(obj.ReferenceEquals(assignment.Name, null))
          -> Some(assignment.Name)
        | _ -> AnalyzeAssignment(node.NextSibling)
    let rec AnalyzeIntegerExpression(node: AstNode) : Option<string> =
        match node with 
        | null -> None
        | :? PrimitiveType as prim 
            when IsIntegerPrimitive(prim)
            -> AnalyzeAssignment(node.NextSibling)
        | _ -> AnalyzeIntegerExpression(node.NextSibling)
    let AnalyzeConstant(constant: CSharpModifierToken): Option<string> =
        AnalyzeIntegerExpression(constant)
    let IsBlockType(node: AstNode): bool =
        match node with
        | :? EntityDeclaration -> true 
        | :? BlockStatement -> true
        | _ -> false

    let rec AnalyzeNode(node: AstNode, constants: System.Collections.Generic.List<string>):List<string> = 
        let line = node.Region.BeginLine
        let fileName = tree.FileName
        let errorHeader = 
                    fileName + 
                    " line " + 
                    line.ToString() + 
                    ": "
        let thisResult = 
            match node with
            | :? ArrayCreateExpression as arrayCreate 
                when (IsNumericType(arrayCreate.Type) &&
                        not(IsStackedArray(arrayCreate)) &&
                        not(IsArrayOfSmallSize(arrayCreate, constants))) -> 
                [   errorHeader + "Numeric array construction" ]
            | :? ObjectCreateExpression as objectCreate 
                when IsExpensiveObjectConstruction(objectCreate) -> 
                [   errorHeader + "Vector construction" ]
            | :? CSharpModifierToken as modfierToken
                when modfierToken.Modifier = Modifiers.Const ||
                     modfierToken.Modifier = Modifiers.Readonly
                -> 
                    let constant = AnalyzeConstant(modfierToken)
                    match constant with
                    | Some(c) ->
                        constants.Add(c)
                    | _ -> ()
                    []
            | _ -> []

        let childrenCount = node.Children.Count()
        let children = node.Children |> Seq.toList
        let childResults = children |> List.collect (fun n -> AnalyzeNode(n, constants))
        List.concat [ thisResult; childResults ]
    AnalyzeNode(tree, new System.Collections.Generic.List<string>())