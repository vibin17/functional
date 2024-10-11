module Treap

open System
open System.IO
open System.Text
open System.Text.Json

type Node =
    | Empty
    | Node of x: int * y: int * left: Node * right: Node

type Treap (coords : (int * int)[]) =
    member val public Root = Treap.BuildTreeInternal (Array.sortBy (fun (x, y) -> x) coords) with get

    member public this.GetLeafs() =
        Treap.FilterInternal this.Root (fun node -> 
            match node with 
            | Empty -> false
            | Node (x, y, left, right) ->
                match left with 
                | Empty -> 
                    match right with 
                    | Empty -> true
                    | Node (_) -> false
                | Node (_) -> false)

    member public this.GetIntermediateNodes() =
        Treap.FilterInternal this.Root (fun node -> 
            let mutable hasNodes = false
            
            match node with 
            | Empty -> ()
            | Node (_, _, left, right) ->
                match left with 
                | Empty -> ()
                | Node (_) -> hasNodes <- true
                match right with 
                | Empty -> ()
                | Node (_) -> hasNodes <- true

            hasNodes
        )

    member public this.Filter (predicate: Node -> bool) : Node seq = 
        Treap.FilterInternal (this.Root) predicate

    member public this.Map (selector: int * int -> int * int) : Treap =
        null

    override this.ToString() =
         Treap.ToStringInternal this.Root 0

    member public this.Dump() =
        let filename = $"""{DateTime.Now.ToString("MM-dd__HH-mm-ss")}.txt""";

        using (File.Open(filename, FileMode.CreateNew)) (
            fun fileStream ->
                fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(this.ToString()))
        )

        printfn $"Treap saved to {filename}"

    static member private BuildTreeInternal (coords: (int * int)[]) : Node =
        if coords.Length < 1 then
            Empty
        else
            let root = Seq.maxBy(fun (x, y) -> (y, x)) coords
            let rootIndex = Seq.findIndex ((=) root) coords

            let left = coords[..rootIndex - 1]
            let right = coords[rootIndex + 1..]

            let leftSubtree = Treap.BuildTreeInternal left
            let rightSubtree = Treap.BuildTreeInternal right

            Node (fst root, snd root, leftSubtree, rightSubtree)

    static member private FilterInternal (node: Node) (predicate: Node -> bool): Node[] =
        match node with
        | Empty -> Array.empty
        | Node (x, y, left, right) ->
            let left = Treap.FilterInternal left predicate
            let right = Treap.FilterInternal right predicate

            if (predicate node) then
                [| node; yield! left ; yield! right |]
            else
                [| yield! left ; yield! right |]

    static member private ToStringInternal (node : Node) (currentGeneration : int) : string =
        let builder = new StringBuilder()

        Treap.ToStringWalk builder node 0 |> ignore

        builder.ToString()

    static member private ToStringWalk (sb : StringBuilder) (node : Node) (currentGen : int)  =
        match node with
        | Empty -> ()
        | Node(x, y, left, right) ->
            let nodeString = String.replicate currentGen "\t" + $" - Node({x}, {y})\n";

            sb.Append(nodeString) |> ignore
            Treap.ToStringWalk sb left (currentGen + 1)
            Treap.ToStringWalk sb right (currentGen + 1)
