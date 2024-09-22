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
    for i in 1..n - 1 do
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
printfn "Geometric mean: %A" ((Seq.fold (fun prev current -> prev * current) 1.0 geometricProgression) ** (float 1 / float n))
printfn "Median: %A" median

let filename = $"""{DateTime.Now.ToString("MM-dd__HH-mm-ss")}.txt""";

using (File.Open(filename, FileMode.CreateNew)) (
    fun fileStream ->
        fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(geometricProgression))
)

printfn $"Geometric progression saved to {filename}"
