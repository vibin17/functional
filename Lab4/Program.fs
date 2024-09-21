// Task 1

// For more information see https://aka.ms/fsharp-console-apps
open System;

printfn "Enter a number"

let n = bigint.Parse(Console.ReadLine())
let rec factorialRecursive (n: bigint) : bigint = 
    if n = bigint.One then n
    else n * factorialRecursive(n - bigint.One)

let factorial = factorialRecursive (n)

printfn "Factorial: %A" factorial