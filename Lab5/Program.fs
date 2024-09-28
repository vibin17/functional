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

let tree = Treap.Build coords

match tree with
| None -> printfn "Tree not created"
| Some treap ->
    let filename = $"""{DateTime.Now.ToString("MM-dd__HH-mm-ss")}.txt""";

    using (File.Open(filename, FileMode.CreateNew)) (
        fun fileStream ->
            fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(treap.ToString()))
    )

    printfn "_______"
    printfn "Treap: "
    printf "%A" treap

    printfn $"Treap saved to {filename}"