// Task 2

// For more information see https://aka.ms/fsharp-console-apps
open System;
open System.IO;
open System.Text.Json;

printfn "Enter b0"

let b0 = Double.Parse(Console.ReadLine())

printfn "Enter q"

let q = Double.Parse(Console.ReadLine())

printfn "Enter n"

let n = Int32.Parse(Console.ReadLine())
let mutable prev = b0

let geometricProgression = [|
    b0
    for i in 1..n do
        let next = prev * q

        prev <- next

        next
|]

let sum = Seq.sum geometricProgression
let median = 
    if (n % 2) = 0 then (geometricProgression[n / 2 - 1] + geometricProgression[n / 2]) / 2.0
    else geometricProgression[n / 2]

printfn "Min: %A" (Seq.min geometricProgression)
printfn "Max: %A" (Seq.max geometricProgression)
printfn "Sum: %A" sum
printfn "Arithmetic mean: %A" (sum / float n)
printfn "Geometric mean: %A" ((Seq.reduce (fun prev current -> prev * current) geometricProgression) ** n)
printfn "Median: %A" median

let fileName = $"gp-{JsonSerializer.Serialize(DateTime.Now)}.txt"
let file = File.Create fileName

File.WriteAllText fileName (JsonSerializer.Serialize(geometricProgression))

printfn $"Geometric progression saved to {fileName}"
