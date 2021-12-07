module Challenges.Challenge07

open Utils.IO
open System

let triangularNumber n = (n*(n+1))/2

let part1 =
    let input = readLine<int> 7 ','
    let min = Seq.min input
    let max = Seq.max input

    seq{min..max-1}
    |> Seq.map (fun i -> input |> Seq.map (fun n -> Math.Abs (n-i)) |> Seq.sum)
    |> Seq.min
    |> string

let part2 =
    let input = readLine<int> 7 ','
    let min = Seq.min input
    let max = Seq.max input

    seq{min..max-1}
    |> Seq.map (fun i -> input |> Seq.map (fun n -> (n-i) |> (Math.Abs >> triangularNumber)) |> Seq.sum)
    |> Seq.min
    |> string