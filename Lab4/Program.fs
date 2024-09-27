// Task 1

// For more information see https://aka.ms/fsharp-console-apps
open System;

printfn "Enter a number"

let n = bigint.Parse(Console.ReadLine())

if (n < bigint.Zero) then invalidOp("n < 0")

let rec factorialRecursive (n: bigint, accumulator: bigint) : bigint = 
    if n > bigint.One then factorialRecursive(n - bigint.One, accumulator * n)
    else accumulator

let factorial = factorialRecursive (n, 1)

printfn "Factorial: %A" factorial