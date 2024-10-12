// Task 5

// For more information see https://aka.ms/fsharp-console-apps
open ITreap
open Treap

open System
open System.IO
open System.Text.RegularExpressions

let pattern = "[A-Za-z0-9 .,!?:;+-]+"
let coords = 
    ((String.Empty, File.ReadAllLines(@"text.txt")) |> String.Join, $"\(({pattern}),[ ]*({pattern})\)")
    |> Regex.Matches
    |> Seq.map (fun a ->
        let x = a.Groups[1].Value
        let y = a.Groups[2].Value
        (x, y))
    |> Seq.toArray

printfn "Coords:"
Seq.iter (printfn "%A") coords

let treap = new Treap<string> (coords)

printfn "_______"
printfn "Treap: "
printfn "%A" (treap)

let intermediateNodes = (treap :> ITreap<string>).GetIntermediateNodes()
let leafs = (treap :> ITreap<string>).GetLeafs()

printfn "Root:"
printfn "%O" ((treap :> ITreap<string>).Root)

printfn "Intermediate:"
Seq.iter (printfn "%O") (intermediateNodes)

printfn "Leafs:"
Seq.iter (printfn "%O") (leafs)

let invertString (str: string) = 
    new string(
        Seq.toArray (Seq.map (fun c ->
            if Char.IsLetter(c) then
                if Char.IsUpper(c) then Char.ToLower(c)
                else Char.ToUpper(c)
            else c) 
            str))

let transformedTreap = (treap :> ITreap<string>).Map(fun (x, y) ->
    (invertString x, invertString y))

printfn "Transformed treap:"
printf "%O" transformedTreap

let containsWhitespaceOrPunctuation a =
    (Regex.Matches (a, "[ .,!?:;]+")).Count > 0

let filteredNodes = (treap :> ITreap<string>).Filter(fun node ->
    match node with 
    | Empty -> false
    | Node (x, y, _, _) -> containsWhitespaceOrPunctuation x || containsWhitespaceOrPunctuation y)

printfn "Filtered nodes:"
Seq.iter (printfn "%O") (filteredNodes)