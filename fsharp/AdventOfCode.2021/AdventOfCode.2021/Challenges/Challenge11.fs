module Challenges.Challenge11

open Utils
open Utils.IO

type State = 
    {   Grid: int[][];
        Queue: Point2 list
        Visited: Point2 Set }

let isOutOfBounds (grid:int[][]) x y =
    x < 0 || y < 0 || x >= (Array.length grid.[0]) || y >= (Array.length grid)

let getFlash (grid:int[][]) x y =
    match grid.[y].[x] with
    | v when v > 9 -> List.singleton (Point2.create x y)
    | _ -> List.empty

let increment (grid:int[][]) =
    [   for y in 0..9 do 
        for x in 0..9 do 
        (x,y)]
    |> List.fold
        (fun q (x,y) -> 
            grid.[y].[x] <- grid.[y].[x] + 1
            q @ (getFlash grid x y))
            []

let foldAdjecent (grid:int[][]) (p:Point2) (queue:Point2 list) (visited:Point2 Set) =
    p.Neighbors()
    |> Array.fold 
        (fun (q,v) n -> 
            match n with
            | n when isOutOfBounds grid n.X n.Y -> (q, v)
            | n when Set.contains n v -> (q,v)
            | n -> 
                grid.[n.Y].[n.X] <- grid.[n.Y].[n.X] + 1
                match grid.[n.Y].[n.X] with
                | gval when gval > 9 -> (q @ (List.singleton n), Set.add n v)
                | _ -> (q,v))
        (queue, visited)

let rec flash (grid:int[][]) (queue:Point2 list) (visited:Point2 Set) =
    match queue with
    | [] -> Set.count visited
    | p::tail -> 
        grid.[p.Y].[p.X] <- 0
        foldAdjecent grid p tail visited
        ||> (flash grid)

let setup = fun () -> readGrid<int> 11

let part1 input =
    [0..99]
    |> List.fold
        (fun totalFlashes _ ->
            totalFlashes + (increment input |> (fun queue -> flash input queue (Set.ofList queue)))
            ) 0
    |> string

let part2 input =
    Seq.initInfinite (fun n -> n)
    |> Seq.skipWhile
        (fun i -> 
            let flashes = increment input |> (fun queue -> flash input queue (Set.ofList queue))
            match flashes with
            | 100 -> false
            | _ -> true)
    |> Seq.head
    |> fun x -> string (x + 1)
