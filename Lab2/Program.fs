// Task 13

// For more information see https://aka.ms/fsharp-console-app
open System;
open System.IO;
open System.Text.RegularExpressions

let numberPattern = "-?\d+(?:\.\d+)?"
let coords = 
    ((String.Empty, File.ReadAllLines(@"text.txt")) |> String.Join, $"\(({numberPattern}),[ ]*({numberPattern})\)")
    |> Regex.Matches
    |> Seq.map (fun a ->
        let x = Double.Parse a.Groups[1].Value
        let y = Double.Parse a.Groups[2].Value
        (x, y))

Seq.iter (printfn "%A") coords

printfn "Enter k"

let k = 
    match Double.TryParse (Console.ReadLine()) with
    | true, value -> value
    | false, _ -> invalidOp "Input is not a number"

printfn "(x, y) under y = x + k:"

Seq.filter 
    (fun (x, y) -> y < k + x) 
    coords
|> Seq.iter (fun (pair) -> printfn "%A" pair)
