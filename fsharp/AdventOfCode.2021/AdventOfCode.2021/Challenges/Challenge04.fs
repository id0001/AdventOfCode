module Challenges.Challenge04

open Utils.IO
open System

let readNumbersFromInput (input:list<string>) = (List.head input).Split(',') |> Array.map int |> Array.toList

let readBoardsFromInput (input:list<string>) =
    List.skip 2 input
    |> List.filter (fun line -> line.Length > 0)
    |> List.chunkBySize 5
    |> List.map (fun board ->
                        board
                        |> List.collect (
                            fun s -> 
                                s.Split(' ', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
                                |> Array.map int
                                |> Array.toList))

let pivot list =
    list
    |> List.collect (fun l -> List.mapi (fun i x -> (i,x)) l) // wrap with indices
    |> List.groupBy fst // group by index
    |> List.map (fun (_, s) -> List.map snd s) // unwrap

let checkRows board numbers =
    board
    |> List.chunkBySize 5
    |> List.exists (fun item -> item |> List.forall (fun x -> List.contains x numbers))

let checkColumns board numbers =
    board
    |> List.chunkBySize 5
    |> pivot
    |> List.exists (fun item -> item |> List.forall (fun x -> List.contains x numbers))

let hasWon board numbers =
    (checkRows board numbers) || (checkColumns board numbers)

let findWinningBoard boards numbers =
    let numberlist = [for i in 0..(List.length numbers) -> List.take i numbers]
    let rec findWinningBoardRec i =
        match (List.tryFind (fun board -> hasWon board numberlist.[i]) boards) with
        | Some b -> (b, numberlist.[i])
        | None -> findWinningBoardRec (i+1)
    findWinningBoardRec 0

let findLastWinningBoard boards numbers =
    let numberlist = [for i in 0..(List.length numbers) -> List.take i numbers]
    let rec findWinningBoardRec i =
        if List.forall (fun board -> hasWon board numberlist.[i]) boards then
            (List.find (fun board -> not(hasWon board numberlist.[i-1])) boards, numberlist.[i])
        else
            findWinningBoardRec (i+1)
    findWinningBoardRec 0

let calculateScore board numbers =
    let sum = List.except numbers board |> List.sum
    sum * (List.last numbers)

let part1 = 
    let lines = readLines<string>(4) |> Seq.toList
    let numbers = readNumbersFromInput lines
    let boards = readBoardsFromInput lines

    let (board, numberHistory) = findWinningBoard boards numbers
    calculateScore board numberHistory |> string

let part2 =
    let lines = readLines<string>(4) |> Seq.toList
    let numbers = readNumbersFromInput lines
    let boards = readBoardsFromInput lines

    let (board, numberHistory) = findLastWinningBoard boards numbers
    calculateScore board numberHistory |> string