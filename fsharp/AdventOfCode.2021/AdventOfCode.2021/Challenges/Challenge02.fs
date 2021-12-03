module Challenges.Challenge02

open Utils.IO

// Old code:

//type Movement = { Direction:string; Amount:int }

//let mapMovement (line:string )=
//    line.Split(" ", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
//        |> fun x -> { Movement.Direction = x.[0]; Movement.Amount = int(x.[1]) }

//let readInput =
//    readLines 2
//        |> Seq.map mapMovement

//let move (m:Movement) =
//    match m.Direction with
//    | "forward" -> (m.Amount,0)
//    | "down"-> (0,m.Amount)
//    | "up" -> (0,-m.Amount)
//    | _ -> raise (NotImplementedException ())

//let fold1 (x,y) (m:Movement) =
//    let (dx,dy) = move m
//    (x+dx,y+dy)

//let fold2 (x,y,aim) (m:Movement) =
//    let (dx,daim) = move m
//    (x+dx,y + dx*aim,aim + daim)

//let part1 =
//    readInput
//        |> Seq.fold fold1 (0,0)
//        |> fun (x,y) -> x*y
//        |> string

//let part2 =
//    readInput
//    |> Seq.fold fold2 (0,0,0)
//    |> fun (x,y,_) -> x*y
//    |> string

// New improved code:

type Movement =
    | Horizontal of int
    | Vertical of int

let parseInput (line:string) =
    match line.Split " " with
    | [| "forward"; v |] -> Horizontal (int v)
    | [| "up"; v |] -> Vertical (-int v)
    | [| "down"; v |] -> Vertical (int v)
    | _ -> failwithf "Unsupported input command: %s" line

let part1 =
    readLines 2
        |> Seq.map parseInput
        |> Seq.fold
            (fun (x,y) cmd ->
                match cmd with
                | Horizontal v -> (x+v,y)
                | Vertical v -> (x,y+v)
            )
            (0,0)
        |> fun (x,y) -> x*y
        |> string

let part2 =
    readLines 2
        |> Seq.map parseInput
        |> Seq.fold
            (fun (x,y,aim) cmd ->
                match cmd with
                | Horizontal v -> (x+v,y+(v*aim),aim)
                | Vertical v -> (x,y,aim + v)
            )
            (0,0,0)
        |> fun (x,y,_) -> x*y
        |> string