module Treap

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Text.Json

type Node =
    | Empty

    | Node of x: int * y: int * left: Node * right: Node

type Treap (coords : (int * int)[]) =
    member val public Root = Treap.BuildTreeInternal (Array.sortBy (fun (x, y) -> x) coords) with get

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
