open System;
open System.IO;
open System.Text.RegularExpressions;

let mostFrequentGroup =
    ((String.Empty, File.ReadAllLines @"text.txt") |> String.Join, "\w+")
    |> Regex.Matches
    |> Seq.map(fun a -> a.Value)
    |> Seq.countBy(fun word -> word.ToLower())
    |> Seq.groupBy(fun (_, frequency) -> frequency)
    |> Seq.maxBy(fun group -> fst group)

for wordFrequencyPair in snd mostFrequentGroup do
    printfn "%A" wordFrequencyPair
