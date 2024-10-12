module Treap

open ITreap

open System
open System.IO
open System.Text
open System.Text.Json

type Treap<'T when 'T : comparison and 'T :> IComparable<'T>> (coords: ('T * 'T)[]) =
    interface ITreap<'T> with
        member val Root : Node<'T> = Treap.BuildTreeInternal (Array.sortBy (fun (x, y) -> x) coords) with get

        member this.GetLeafs() =
            Treap.FilterInternal (this :> ITreap<'T>).Root (fun node -> 
                match node with 
                | Node (_, _, Empty, Empty) -> true
                | _ -> false)

        member this.GetIntermediateNodes() =
            let filtered = Treap.FilterInternal (this :> ITreap<'T>).Root (fun node -> 
                match node with
                | Empty -> false
                | Node (_, _, Empty, Empty) -> false
                | _ -> true
            )

            if filtered.Length > 0 then
                filtered[1..]

            else filtered

        member this.Filter (predicate): Node<'T>[] = 
            Treap.FilterInternal ((this :> ITreap<'T>).Root) predicate

        member this.Map (selector): ITreap<'T> =
            let mapped = Treap.MapInternal (this :> ITreap<'T>).Root selector

            new Treap<'T>(mapped)

        member this.Dump(pattern) =
            let filename = $"""{pattern}-{DateTime.Now.ToString("MM-dd__HH-mm-ss")}.txt""";

            using (File.Open(filename, FileMode.CreateNew)) (
                fun fileStream ->
                    fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(this.ToString()))
            )

            printfn $"Treap saved to {filename}"

    override this.ToString() =
         Treap.ToStringInternal (this :> ITreap<'T>).Root 0

    static member private BuildTreeInternal (coords: ('T * 'T)[]) : Node<'T> =
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

    static member private MapInternal<'TResult> (node: Node<'T>) (selector: 'T * 'T -> 'TResult * 'TResult) : ('TResult * 'TResult)[] =
       match node with
       | Empty -> Array.empty
       | Node (x, y, left, right) ->
            let leftMapped = Treap.MapInternal left selector
            let rightMapped = Treap.MapInternal right selector
            let mappedXY = selector (x, y)

            [| mappedXY; yield! leftMapped; yield! rightMapped |]

    static member private FilterInternal (node: Node<'T>) (predicate: Node<'T> -> bool): Node<'T>[] =
        match node with
        | Empty -> Array.empty
        | Node (x, y, left, right) ->
            let left = Treap.FilterInternal left predicate
            let right = Treap.FilterInternal right predicate

            if (predicate node) then
                [| node; yield! left ; yield! right |]
            else
                [| yield! left ; yield! right |]

    static member private ToStringInternal (node : Node<'T>) (currentGeneration : int) : string =
        let builder = new StringBuilder()

        Treap.ToStringWalk builder node 0 |> ignore

        builder.ToString()

    static member private ToStringWalk (sb : StringBuilder) (node : Node<'T>) (currentGen : int)  =
        match node with
        | Empty -> ()
        | Node(x, y, left, right) ->
            let nodeString = String.replicate currentGen "\t" + $" - Node({x}, {y})\n";

            sb.Append(nodeString) |> ignore
            Treap.ToStringWalk sb left (currentGen + 1)
            Treap.ToStringWalk sb right (currentGen + 1)
