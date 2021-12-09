module Challenges.Challenge09

open Utils
open Utils.IO

let gridPoints rangeY rangeX =
    seq {for y in rangeY do for x in rangeX -> {X=x;Y=y}}

let isOutOfBounds x y (grid:int array array) =
    x < 0 || y < 0 || x >= (Array.length grid.[0]) || y >= Array.length grid

let isLowestPoint (p:Point2) (grid:int array array) =
    p.Neighbors4()
    |> Seq.forall
        (fun n ->
            match n.X, n.Y with
            | x, y when isOutOfBounds x y grid -> true
            | x, y when grid.[p.Y].[p.X] < grid.[y].[x] -> true
            | _ -> false)

let lowestPoints grid =
    gridPoints {0..(Array.length grid)-1} {0..(Array.length grid.[0])-1}
    |> Seq.filter
        (fun p -> isLowestPoint p grid)

let calculateRiskFactor grid =
    grid
    |> lowestPoints
    |> Seq.map (fun p -> grid.[p.Y].[p.X] + 1)
    |> Seq.sum

let rec floodFill (p:Point2) filled (grid:int array array) =
    match p.X, p.Y with
    | x, y when isOutOfBounds x y grid -> filled
    | x, y when grid.[y].[x] = 9 -> filled
    | _ when Seq.contains p filled -> filled
    | _ ->
        p.Neighbors4()
        |> Seq.fold
            (fun accFilled n ->
                floodFill n accFilled grid) (Seq.append filled (Seq.singleton p))

let part1 =
    readGrid<int> 9
    |> calculateRiskFactor
    |> string

let part2 =
    let grid = readGrid<int> 9
    grid
    |> lowestPoints
    |> Seq.map (fun p -> floodFill p Seq.empty grid)
    |> Seq.sortByDescending (fun basin -> Seq.length basin)
    |> Seq.take 3
    |> Seq.map Seq.length
    |> Seq.reduce (*)
    |> string
