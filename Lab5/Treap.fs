module Treap

open System.Text

type Node =
    | Empty
    | Node of x: int * y: int * left: Node * right: Node

type Treap private (root: Node) =
    member val public Root = root with get

    override this.ToString() =
         Treap.ToStringInternal root 0

    static member public Build(coords : (int * int)[]) : Treap =
        let sortedCoords = Array.sortBy (fun (x, y) -> x) coords
        let root = Treap.BuildTreeInternal (sortedCoords)

        new Treap(root)

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
