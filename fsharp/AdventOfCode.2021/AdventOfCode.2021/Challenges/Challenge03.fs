module Challenges.Challenge03

open Utils.IO
open System

let getMostCommon (arr:string array) i =
    arr
    |> Array.map (fun s -> Seq.item i s)
    |> Array.groupBy (fun s -> s)
    |> Array.sortWith
        (fun a b ->
            if Seq.length (snd a) = Seq.length (snd b) then
                int (fst b) - int (fst a)
            else
                (Seq.length (snd b) ) - (Seq.length (snd a))
        )
    |> Array.head
    |> fst

let findCode (data:string array) cmp =
    [| 0 .. 12  |]
    |> Array.fold
        (fun acc i ->
            match Array.length acc with
            | 1 -> acc
            | _ ->
                let mc = getMostCommon acc i
                Array.filter (fun s -> cmp (Seq.item i s) mc) acc) data
    |> Array.head

let setup =
    readLines<string> 3

let part1 (input:string array) =
    let len = Seq.length input.[0]
    let counts = [|
        for i in 0..len-1 do
            input
            |> Array.groupBy (fun x -> Seq.item i x)
            |> Array.sortBy fst
            |> Array.map (fun (k,v) -> Seq.length v) |]

    let gamma =
        seq {0..len-1}
            |> Seq.fold
                (fun a i ->
                    let count = Seq.item i counts
                    if count.[1] > count.[0] then
                        a + (1 <<< (len-1-i))
                    else
                        a
                ) 0

    let epsilon = ~~~gamma &&& 0xFFF
    gamma*epsilon |> string

let part2 input =
    let oxygen = findCode input (fun a b -> a = b)
    let co2 = findCode input (fun a b -> a <> b)
    Convert.ToInt32(string oxygen, 2) * Convert.ToInt32(string co2, 2) |> string