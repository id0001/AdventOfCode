module Challenges.Challenge10

open Utils.IO

type ValidationResult =
| Success of char list
| Fail of char

let matchingPair c =
    match c with
    | '(' | ')' -> ('(',')')
    | '[' | ']' -> ('[',']')
    | '{' | '}' -> ('{','}')
    | '<' | '>' -> ('<','>')
    | _ -> failwith "Unsupported character"

let addChar c (stack:char list) =
    match c with
    | '(' | '[' | '{' | '<' -> Success(c::stack)
    | ')' | ']' | '}' | '>' -> matchingPair c |> (fun (l,r) -> if List.head stack = l then Success(List.tail stack) else Fail(r))
    | _ -> failwith "Unsupported character"

let validate list =
    let rec recFun list stack =
        match list with
        | [] -> Success(stack)
        | c::tail ->
            match addChar c stack with
            | Fail(x) -> Fail(x)
            | Success(s) -> recFun tail s

    recFun list List.empty

let score1 c =
    match c with
    | ')' -> 3
    | ']' -> 57
    | '}' -> 1197
    | '>' -> 25137
    | _ -> failwith "Unsupported character"

let score2 list =
    list
    |> List.fold
        (fun (score:int64) c ->
            match c with
            | '(' -> (score * 5L) + 1L
            | '[' -> (score * 5L) + 2L
            | '{' -> (score * 5L) + 3L
            | '<' -> (score * 5L) + 4L
            | _ -> failwith "Unsupported character") 0L

let setup = fun () -> readLines<string> 10

let part1 input =
    input
    |> Array.map
        (fun line ->
            match validate (List.ofSeq line) with
            | Fail(c) -> score1 c
            | Success(_) -> 0)
    |> Array.sum
    |> string

let part2 input =
    input
    |> Array.map
        (fun line ->
            match validate (List.ofSeq line) with
            | Fail(_) -> 0L
            | Success(stack) -> score2 stack)
    |> Array.filter (fun x -> x > 0L)
    |> Array.sort
    |> fun arr -> arr.[(Array.length arr)/2]
    |> string
