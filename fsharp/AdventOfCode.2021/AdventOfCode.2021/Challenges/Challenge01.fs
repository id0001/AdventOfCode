module Challenges.Challenge01

open Utils.IO

let cmp pair = fst pair < snd pair

let setup = fun () -> readLines<int> 1

let part1 input =
    input
        |> Array.pairwise
        |> Array.filter cmp
        |> Array.length
        |> string

let part2 (input:int array) =
    input
        |> Array.windowed 3
        |> Array.map Array.sum
        |> Array.pairwise
        |> Array.filter cmp
        |> Array.length
        |> string