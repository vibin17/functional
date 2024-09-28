// Task 5

// For more information see https://aka.ms/fsharp-console-apps
open System.IO

type Node(x: int, y: int, left: Node option, right: Node option) =

    member val public X = x with get
    member val public Y = y with get
    member val public Left = left with get
    member val public right = right with get

    static member public BuildTree(pairs : (int * int) array) : Node option =
        Node.BuildTreeInternal (Array.sortWith (fun a b -> compare (fst a, snd a) (fst b, snd b)) pairs) 0 pairs.Length

    static member private BuildTreeInternal (pairs : (int * int)[]) (start : int) (endIndex : int) : Node option =
        if endIndex - start = 0 then
            None
        elif endIndex - start <= 1 then
           Some (Node(fst pairs[start], snd pairs[start], None, None))
        else
            let mutable rootPair = pairs[start]
            let mutable rootIndex = start

            for i = start + 1 to endIndex - 1 do
                if snd pairs.[i] > snd rootPair || (snd pairs.[i] = snd rootPair && fst pairs.[i] < fst rootPair) then
                    rootPair <- pairs.[i]
                    rootIndex <- i

            let leftSubtree = Node.BuildTreeInternal pairs start rootIndex
            let rightSubtree =
                if rootIndex + 1 < pairs.Length then
                    Node.BuildTreeInternal pairs (rootIndex + 1) endIndex
                else
                    None

            Some (Node(fst rootPair, snd rootPair, leftSubtree, rightSubtree))

let rec toStringFromRoot (root : Node option) : string =
    match root with
    | None -> ""
    | Some rootNode ->
        let rec toStringInternal (node : Node) (currentGeneration : int) : string =
            let nodeString = sprintf " - Node(%d, %d)\n" node.X node.Y
            let indentation = String.replicate currentGeneration "\t"
            let formattedNodeString = indentation + nodeString
            let leftString =
                match node.Left with
                | Some leftNode -> toStringInternal leftNode (currentGeneration + 1)
                | None -> ""
            let rightString =
                match node.Right with
                | Some rightNode -> toStringInternal rightNode (currentGeneration + 1)
                | None -> ""
            formattedNodeString + leftString + rightString

        toStringInternal rootNode 0

// Функция для чтения пар чисел из файла
let readPairsFromFile (filePath: string) =
    use reader = System.IO.File.OpenText(filePath)
    let mutable pairs = []
    while not reader.EndOfStream do
        let line = reader.ReadLine()
        let nums = line.Split([|';'|], System.StringSplitOptions.RemoveEmptyEntries) |> Array.map int
        if nums.Length = 2 then
            pairs <- (nums.[0], nums.[1]) :: pairs
    Array.ofList (List.rev pairs)

// Функция для записи строки в текстовый файл
let writeStringToFile (filePath : string, content : string) =
    use writer = new StreamWriter(filePath)
    writer.Write(content)

// Путь к файлу с парами чисел
let inputFilePath = "input.txt"
// Путь к файлу для вывода результата
let outputFilePath = "output.txt"

// Чтение пар из файла
let pairsFromFile = readPairsFromFile inputFilePath

// Вывод отсортированного массива в консоль
printfn "Sorted Pairs:"
for pair in Array.sortWith (fun a b -> compare (fst a, snd a) (fst b, snd b)) pairsFromFile do
    printfn "%d, %d" (fst pair) (snd pair)

// Построение дерева
let tree = buildTree pairsFromFile

match tree with
| None -> printfn "Tree not created"
| Some rootNode ->
    // Получение строки для вывода
    let treeString = toStringFromRoot tree

    // Вывод результата в консоль и запись в файл
    printfn "%s" treeString
    writeStringToFile outputFilePath treeString
