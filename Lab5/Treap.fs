module Treap

open System.Text

type Node =
    | Leaf of x: int * y: int
    | WithLeftChild of x: int * y: int * left: Node
    | WithRightChild of x: int * y: int * left: Node

type Node internal (x: int, y: int, left: Node option, right: Node option) =

    member val public X = x with get
    member val public Y = y with get
    member val public Left = left with get
    member val public Right = right with get

type Treap private (root: Node) =
    member val public Root = root with get

    override this.ToString() =
         Treap.ToStringInternal root 0

    static member public Build(coords : (int * int)[]) : Treap option =
        let sortedCoords = Array.sortBy (fun (x, y) -> x) coords
        let root = Treap.BuildTreeInternal (sortedCoords)

        match root with 
        | None -> None
        | Some treeRoot -> Some(new Treap(treeRoot))

    static member private BuildTreeInternal (coords: (int * int)[]) : Node option =
        if coords.Length < 1 then
            None
        else
            let root = Seq.maxBy(fun (x, y) -> (y, x)) coords
            let rootIndex = Seq.findIndex ((=) root) coords

            let left = coords[..rootIndex - 1]
            let right = coords[rootIndex + 1..]

            let leftSubtree = Treap.BuildTreeInternal left
            let rightSubtree = Treap.BuildTreeInternal right

            Some (Node(fst root, snd root, leftSubtree, rightSubtree))

    static member private ToStringInternal (node : Node) (currentGeneration : int) : string =
        let builder = new StringBuilder()

        Treap.ToStringWalk builder node 0 |> ignore

        builder.ToString()

    static member private ToStringWalk (sb : StringBuilder) (node : Node) (currentGen : int)  =
        let nodeString = String.replicate currentGen "\t" + $" - Node({node.X}, {node.Y})\n";
            
        sb.Append(nodeString) |> ignore
        
        if node.Left.IsSome then 
            Treap.ToStringWalk sb node.Left.Value (currentGen + 1)

        if node.Right.IsSome then 
            Treap.ToStringWalk sb node.Right.Value (currentGen + 1)
