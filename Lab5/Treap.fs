module Treap

open System.Collections.Generic

type Node internal (x: int, y: int, left: Node option, right: Node option) =

    member val public X = x with get
    member val public Y = y with get
    member val public Left = left with get
    member val public Right = right with get

type Treap private (root: Node) =
    member val public Root = root with get

    override this.ToString() =
         Treap.ToStringInternal root 0

    static member public BuildTree(coords : (int * int)[]) : Treap option =
        let sortedCoords = Array.sortBy (fun (x, y) -> x) coords
        let root = Treap.BuildTreeInternal (sortedCoords) 0 coords.Length

        match root with 
        | None -> None
        | Some treeRoot -> Some(new Treap(treeRoot))

    static member private BuildTreeInternal (coords: (int * int)[]) (start: int) (endIndex: int) : Node option =
        if endIndex - start = 0 then
            None
        elif endIndex - start <= 1 then
           Some (Node(fst coords[start], snd coords[start], None, None))
        else
            let mutable (rootX, rootY) = coords[start]
            let mutable rootIndex = start

            for i = start + 1 to endIndex - 1 do
                let (x, y) = coords[i]

                if y > rootY || (y = rootY && x < rootX) then
                    (rootX, rootY) <- coords[i]
                    rootIndex <- i

            let leftSubtree = Treap.BuildTreeInternal coords start rootIndex
            let rightSubtree =
                if rootIndex + 1 < coords.Length then
                    Treap.BuildTreeInternal coords (rootIndex + 1) endIndex
                else
                    None

            Some (Node(rootX, rootY, leftSubtree, rightSubtree))

    static member private ToStringInternal (node : Node) (currentGeneration : int) : string =
        let stack = new Stack<

        let nodeString = sprintf " - Node(%d, %d)\n" node.X node.Y
        let formattedNodeString = (String.replicate currentGeneration "\t") + nodeString
        let leftString =
            match node.Left with
            | Some leftNode -> Treap.ToStringInternal leftNode (currentGeneration + 1)
            | None -> ""

        let rightString =
            match node.Right with
            | Some rightNode -> Treap.ToStringInternal rightNode (currentGeneration + 1)
            | None -> ""

        formattedNodeString + leftString + rightString
