module Challenges.Challenge12

open Microsoft.FSharp.Core
open Utils.IO

let rec findAllPaths1 (queue:string list list) (edges:Map<string,string[]>) (foundPaths:string list list) =
    match queue with
    | [] -> foundPaths
    | path::tail ->
        match List.head path with
        | "end" -> path :: foundPaths
        | last when not(Map.containsKey last edges) -> foundPaths
        | last -> 
            Array.fold
                (fun pathAcc neighbor -> 
                    match neighbor with
                    | neighbor when not(String.isUpper neighbor) && List.contains neighbor path -> pathAcc
                    | neighbor -> findAllPaths1 (tail @ [neighbor::path]) edges pathAcc)
                foundPaths edges[last]

let rec findAllPaths2 (queue:string list list) (edges:Map<string,string[]>) (foundPaths:string list list) =
    match queue with
    | [] -> foundPaths
    | path::tail ->
        let rawLast = List.head path
        let last = if (String.endsWith "'" rawLast) then String.substring2 0 ((String.length rawLast) - 1) rawLast else rawLast
        match last with
        | "end" -> path :: foundPaths
        | last when not(Map.containsKey last edges) -> foundPaths
        | last -> 
            Array.fold
                (fun pathAcc neighbor -> 
                    match neighbor with
                    | neighbor when String.isUpper neighbor || not(List.contains neighbor path) -> findAllPaths2 (tail @ [neighbor::path]) edges pathAcc
                    | neighbor when not(List.exists (fun p -> String.endsWith "'" p) path) && neighbor <> "start" -> findAllPaths2 (tail @ [(neighbor+"'")::path]) edges pathAcc
                    | _ -> pathAcc)
                foundPaths edges[last]

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
    findAllPaths1 [["start"]] input []
    |> List.length
    |> string

let part2 input = 
    findAllPaths2 [["start"]] input []
    |> List.length
    |> string