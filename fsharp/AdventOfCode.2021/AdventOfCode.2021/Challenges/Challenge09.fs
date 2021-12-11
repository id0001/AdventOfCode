module Challenges.Challenge09

open Utils
open Utils.IO

let isOutOfBounds (grid:int[][]) x y =
    x < 0 || y < 0 || x >= (Array.length grid.[0]) || y >= (Array.length grid)

let isLocalMinima (grid:int[][]) x y =
    Point2.neighbors4 x y
    |> Array.forall
        (fun n ->
            match n.X, n.Y with
            | nx, ny when isOutOfBounds grid nx ny -> true
            | nx, ny when grid.[ny].[nx] > grid.[y].[x] -> true
            | _, _ -> false)

let rec floodFill (p:Point2) (filled:Point2 Set) (grid:int[][]) =
    match p.X, p.Y with
    | x,y when isOutOfBounds grid x y -> filled
    | x,y when grid.[y].[x] = 9 -> filled
    | _,_ when Set.contains p filled -> filled
    | _,_ ->
        p.Neighbors4()
        |> Array.fold
            (fun filledAcc n ->
                floodFill n filledAcc grid) (Set.add p filled)

let setup = fun () -> readGrid<int> 9

let part1 input =
    let rows = Array.length input
    let cols = Array.length (input.[0])
    [| for y in 0..rows-1 do
        for x in 0..cols-1 do
            if isLocalMinima input x y then
                input.[y].[x] + 1 |]
    |> Array.sum
    |> string

let part2 input =
    let rows = Array.length input
    let cols = Array.length (input.[0])
    [| for y in 0..rows-1 do
        for x in 0..cols-1 do
            if isLocalMinima input x y then
                (floodFill {X = x; Y = y} Set.empty input) |> Set.count |]
    |> Array.sortByDescending (fun x -> x)
    |> Array.take 3
    |> Array.reduce (*)
    |> string