module Challenges.Challenge12

open Microsoft.FSharp.Core
open Utils.IO

let rec countPaths edges currentNode visited canVisitTwice =
    match currentNode, Set.contains currentNode visited with
    | "end",_ -> 1
    | "start", true -> 0
    | _, true when not canVisitTwice -> 0
    | _, hasVisited ->
        let newVisited =
            match currentNode with
            | currentNode when not (String.isUpper currentNode) -> Set.add currentNode visited
            | _ -> visited

        Map.find currentNode edges
        |> Seq.sumBy (fun neighbor -> countPaths edges neighbor newVisited (canVisitTwice && not hasVisited))

let setup =
    fun () ->
        let lines = readLines<string> 12
        lines
        |> Array.collect
            (fun line -> 
                let split = line.Split('-')
                [| (split[0],split[1]); (split[1],split[0])|])
        |> Array.groupBy fst
        |> Array.map (fun (key, values) -> (key, Array.map snd values))
        |> Map.ofArray

let part1 input =
    countPaths input "start" Set.empty false
    |> string

let part2 input = 
    countPaths input "start" Set.empty true
    |> string