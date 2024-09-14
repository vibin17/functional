// For more information see https://aka.ms/fsharp-console-app
open System;
open System.IO;
open System.Text.RegularExpressions

let text = (String.Empty, File.ReadAllLines(@"text.txt")) |> String.Join

let b = 
    Regex.Matches(text, "\((\d+),[ ,]?(\d+)\)")
    |> Seq.map(fun a ->
        let x = Double.Parse a[1]
        let y = Double.Parse a[2]
        return (x, y))

let a = 
    ((String.Empty, File.ReadAllLines(@"text.txt")) |> String.Join, "\((\d+),[ ,]?(\d+)\)")
    |> Regex.Matches; 

return