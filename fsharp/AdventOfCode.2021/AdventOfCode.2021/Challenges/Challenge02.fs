module Challenges.Challenge02

open Utils.IO

type Movement =
    | Horizontal of int
    | Vertical of int

let parseInput (line:string) =
    match line.Split " " with
    | [| "forward"; v |] -> Horizontal (int v)
    | [| "up"; v |] -> Vertical (-int v)
    | [| "down"; v |] -> Vertical (int v)
    | _ -> failwithf "Unsupported input command: %s" line

let setup =
    let lines = readLines 2
    fun () ->
        lines
        |> Array.map
            (fun (line:string) ->
                match line.Split (' ') with
                | [| "forward"; v |] -> Horizontal (int v)
                | [| "up"; v |] -> Vertical (-int v)
                | [| "down"; v |] -> Vertical (int v)
                | _ -> failwithf "Unsupported input command: %s" line)

let part1 input =
    input
    |> Array.fold
        (fun (x,y) cmd ->
            match cmd with
            | Horizontal v -> (x+v,y)
            | Vertical v -> (x,y+v)
        )
        (0,0)
    |> fun (x,y) -> x*y
    |> string

let part2 input =
    input
    |> Array.fold
        (fun (x,y,aim) cmd ->
            match cmd with
            | Horizontal v -> (x+v,y+(v*aim),aim)
            | Vertical v -> (x,y,aim + v)
        )
        (0,0,0)
    |> fun (x,y,_) -> x*y
    |> string