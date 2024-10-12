module ITreap

open System

type Node<'T when 'T :> IComparable<'T>> =
    | Empty
    | Node of x: 'T * y: 'T * left: Node<'T> * right: Node<'T>

    override this.ToString() =
         match this with
         | Node (x, y, _, _) -> $"({x}, {y})"
         | Empty -> "-"

type ITreap<'T when 'T :> IComparable<'T>> =
    abstract member Root : Node<'T>

    abstract member GetLeafs: unit -> Node<'T>[]
    abstract member GetIntermediateNodes: unit -> Node<'T>[]
    abstract member Filter: (Node<'T> -> bool) -> Node<'T>[]
    abstract member Map: (('T * 'T) -> ('T * 'T)) -> ITreap<'T>
    abstract member Dump: string -> unit
    
