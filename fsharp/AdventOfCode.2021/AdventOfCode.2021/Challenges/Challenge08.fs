module Challenges.Challenge08

open System
open Utils.IO

type IOPair = {
    Inputs: string array;
    Outputs: string array
    }

let toIOPair (line:string) =
    line.Split ('|', StringSplitOptions.TrimEntries)
    |> (fun s -> { IOPair.Inputs = s.[0].Split(' '); IOPair.Outputs = s.[1].Split(' ')})

let numberByLength len seq =
    Seq.find(fun x -> (String.length x) = len) seq

let countCommonSegments a b =
    Set.intersect (Set.ofSeq a) (Set.ofSeq b) |> Set.count

let getNumber s one four =
    match (String.length s, countCommonSegments s one, countCommonSegments s four) with
    | (2, _, _) -> 1
    | (3, _, _) -> 7
    | (4, _, _) -> 4
    | (7, _, _) -> 8
    | (5, 2, 3) -> 3
    | (5, 1, 2) -> 2
    | (5, 1, 3) -> 5
    | (6, 2, 3) -> 0
    | (6, 1, 3) -> 6
    | (6, 2, 4) -> 9
    | _ -> -1

let intPow a exp = int ((float a) ** (float exp))

let numberFromIOPair pair =
    let one = numberByLength 2 pair.Inputs
    let four = numberByLength 4 pair.Inputs
    let mapped = Map.ofSeq (Seq.map (fun s -> (Set.ofSeq s, getNumber s one four)) pair.Inputs)
    let len = Seq.length pair.Outputs
    pair.Outputs
        |> Seq.mapi (fun i s -> mapped.Item (Set.ofSeq s) * (intPow 10 (len-1-i)))
        |> Seq.sum

let setup = 
    let lines = readLines<string> 8
    fun () -> 
        lines
        |> Seq.map toIOPair

let part1 input =
    let simpleNums = [|2;4;3;7|]
    input
    |> Seq.map
        (fun io ->
            io.Outputs
            |> Seq.filter (fun s -> Array.contains (Seq.length s) simpleNums)
            |> Seq.length)
    |> Seq.sum
    |> string

let part2 input =
    input
        |> Seq.map numberFromIOPair
        |> Seq.sum
        |> string
