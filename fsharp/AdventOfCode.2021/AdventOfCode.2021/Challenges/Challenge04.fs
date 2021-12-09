module Challenges.Challenge04

open Utils.IO
open System

let readNumbersFromInput (input:string seq) =
    (Seq.head input).Split(',')
    |> Seq.map int
    |> Seq.toArray

let readBoardsFromInput (input:string seq) =
    Seq.skip 2 input
    |> Seq.filter (fun line -> String.length line > 0)
    |> Seq.chunkBySize 5
    |> Seq.map
        (fun board ->
            board
            |> Array.collect (
                fun s ->
                    s.Split(' ', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
                    |> Array.map int))
    |> Seq.toArray

let pivot list =
    list
    |> Array.collect (fun l -> Array.mapi (fun i x -> (i,x)) l) // wrap with indices
    |> Array.groupBy fst // group by index
    |> Array.map (fun (_, s) -> Array.map snd s) // unwrap

let checkRows board numbers =
    board
    |> Array.chunkBySize 5
    |> Array.exists (fun item -> item |> Array.forall (fun x -> Array.contains x numbers))

let checkColumns board numbers =
    board
    |> Array.chunkBySize 5
    |> pivot
    |> Array.exists (fun item -> item |> Array.forall (fun x -> Array.contains x numbers))

let hasWon board numbers =
    (checkRows board numbers) || (checkColumns board numbers)

let findWinningBoard boards numbers =
    let numberlist = [|for i in 0..(Seq.length numbers) -> Array.take i numbers|]
    let rec findWinningBoardRec i =
        match (Array.tryFind (fun board -> hasWon board numberlist.[i]) boards) with
        | Some b -> (b, numberlist.[i])
        | None -> findWinningBoardRec (i+1)
    findWinningBoardRec 0

let findLastWinningBoard boards numbers =
    let numberlist = [|for i in 0..(Seq.length numbers) -> Array.take i numbers|]
    let rec findWinningBoardRec i =
        if Array.forall (fun board -> hasWon board numberlist.[i]) boards then
            (Array.find (fun board -> not(hasWon board numberlist.[i-1])) boards, numberlist.[i])
        else
            findWinningBoardRec (i+1)
    findWinningBoardRec 0

let calculateScore board numbers =
    let sum = Seq.except numbers board |> Seq.sum
    sum * (Seq.last numbers)

let setup =
    let lines = readLines<string> 4
    (readNumbersFromInput lines, readBoardsFromInput lines)

let part1 (numbers, boards) =
    let (board, numberHistory) = findWinningBoard boards numbers
    calculateScore board numberHistory |> string

let part2 (numbers, boards) =
    let (board, numberHistory) = findLastWinningBoard boards numbers
    calculateScore board numberHistory |> string