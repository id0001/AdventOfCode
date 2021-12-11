module Challenges.Challenge07

open Utils.IO
open System

let triangularNumber n = (n*(n+1))/2

let setup = fun () -> readLine<int> 7 ','

let part1 input =
    let min = Array.min input
    let max = Array.max input

    [|min..max-1|]
    |> Seq.map (fun i -> input |> Seq.map (fun n -> Math.Abs (n-i)) |> Seq.sum)
    |> Seq.min
    |> string

let part2 input =
    let min = Seq.min input
    let max = Seq.max input

    [| min..max-1 |]
    |> Seq.map (fun i -> input |> Array.map (fun n -> (n-i) |> (Math.Abs >> triangularNumber)) |> Seq.sum)
    |> Seq.min
    |> string