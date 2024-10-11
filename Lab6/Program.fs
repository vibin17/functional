// Task 5

// For more information see https://aka.ms/fsharp-console-apps
open Treap

open System
open System.IO
open System.Text.Json
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

let treap = new Treap(coords)

printfn "_______"
printfn "Treap: "
printf "%A" treap

let intermediate = treap.GetIntermediateNodes()
let leafs = treap.GetLeafs()

printf "Intermediate:"
printf "%A" intermediate

printf "Leafs:"
printf "%A" leafs