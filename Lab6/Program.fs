// Task 5

// For more information see https://aka.ms/fsharp-console-apps
open Treap

open System
open System.IO
open System.Text.RegularExpressions

let numberPattern = "-?\d+"
let coords = 
    ((String.Empty, File.ReadAllLines(@"text.txt")) |> String.Join, $"\(({numberPattern}),[ ]*({numberPattern})\)")
    |> Regex.Matches
    |> Seq.map (fun a ->
        let x = Int32.Parse a.Groups[1].Value
        let y = Int32.Parse a.Groups[2].Value
        (x, y))
    |> Seq.toArray

printfn "Coords:"
Seq.iter (printfn "%A") coords

let treap = new Treap (coords)

printfn "_______"
printfn "Treap: "
printfn "%A" (treap)

let intermediateNodes = treap.GetIntermediateNodes()
let leafs = treap.GetLeafs()

printfn "Root:"
printfn "%O" (treap.Root)

printfn "Intermediate:"
Seq.iter (printfn "%O") (intermediateNodes)

printfn "Leafs:"
Seq.iter (printfn "%O") (leafs)

let doubledTreap = treap.Map(fun (x, y) -> (2 * x, 2 * y))

printfn "Doubled treap:"
printf "%O" doubledTreap

let evenNodes = treap.Filter(fun node ->
    match node with 
    | Empty -> false
    | Node (x, y, _, _) -> (x % 2) = 0 && (y % 2) = 0)

printfn "Even nodes:"
Seq.iter (printfn "%O") (evenNodes)