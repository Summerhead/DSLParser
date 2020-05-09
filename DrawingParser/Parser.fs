module Parser

open System.Text.RegularExpressions


let initsRegex = new Regex("(set ((?<initX>\d+) (?<initY>\d+)))")
let directionsRegex = new Regex("(?=(?<direction>up|down|left|right) (?<steps>\d+))")
(*let repeatition = new Regex("((repeat (?<repeat>\d+) times: |)\(((?:(?>[^()]+)|(?1))*)\))")
let taskRegex = new Regex("(set (?<initX>\d+) (?<initY>\d+)|(?<direction>up|down|left|right) (?<steps>\d+)|(repeat (?<repeat>\d+) times: |)\(((?:(?>[^()]+)|(?1))*)\))")*)

let regexGroupString (groupName: string) (m: Match) = m.Groups.[groupName].Value
let regexGroupInt    (groupName: string) (m: Match) = regexGroupString groupName m |> int

let parseInitialX    (m: Match) = regexGroupInt "initX" m
let parseInitialY    (m: Match) = regexGroupInt "initY" m
let parseInitial     (m: Match) = (parseInitialX m, parseInitialY m)
let parseRepeatition (m: Match) = regexGroupInt "repeatition" m
let parseDirection   (m: Match) = regexGroupString "direction" m
let parseSteps       (m: Match) = regexGroupInt "steps" m
let parseOperation   (m: Match) = (parseDirection m, parseSteps m)

let parseOperations (matches: MatchCollection) =
    matches 
    |> Seq.cast
    |> Seq.map parseOperation

let parseInits (matches: MatchCollection) =
    matches 
    |> Seq.cast
    |> Seq.map parseInitial

let parseInit (task: string) =
    match initsRegex.Matches task with
    | matches when matches.Count = 0 -> None
    | matches -> Some (parseInits matches)

let parseDirections (task: string) =
    match directionsRegex.Matches task with
    | matches when matches.Count = 0 -> None
    | matches -> Some (parseOperations matches)

let parseTask (task: string) =
    let initials = parseInit task
    let directions = parseDirections task

    (initials, directions)

let applyOperation (x, y) (operations:seq<string * int>) =
    let operationsEnum = operations.GetEnumerator()
    printfn "operationsEnum: %A" operationsEnum
    (x, y)
    |> Seq.unfold (fun s -> 
        match operationsEnum.MoveNext() with
        | true ->
            let direction, steps = operationsEnum.Current
            match direction with 
            | "up"    -> Some((fst s, snd s - steps), (fst s, snd s - steps))
            | "down"  -> Some((fst s, snd s + steps), (fst s, snd s + steps))
            | "left"  -> Some((fst s - steps, snd s), (fst s - steps, snd s))
            | "right" -> Some((fst s + steps, snd s), (fst s + steps, snd s))
            | _       -> failwith "Unsupported operator"
        | false -> None
        )

let parseCoordinates (initials:seq<int * int> option, operations:seq<string * int> option) =
    let initial = (Seq.toArray <| (Option.get initials)).[0]
    Option.map (fun ops -> Seq.append [initial] (applyOperation (initial) ops)) operations

let result (task: string) =
    task
    |> parseTask
    |> parseCoordinates
