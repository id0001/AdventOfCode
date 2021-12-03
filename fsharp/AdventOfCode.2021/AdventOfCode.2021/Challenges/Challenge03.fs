module Challenges.Challenge03

open Utils.IO
open System

let zip seq = seq
            |> Seq.collect(fun s -> s |> Seq.mapi(fun i e -> (i,e)))
            |> Seq.groupBy fst
            |> Seq.map (fun (_,s) -> s |> Seq.map snd)

let getMostCommon seq i =
    seq
    |> Seq.map (fun s -> Seq.item i s)
    |> Seq.groupBy (fun s -> s)
    |> Seq.sortWith
        (fun a b ->
            if Seq.length (snd a) = Seq.length (snd b) then
                int (fst b) - int (fst a)
            else
                (Seq.length (snd b) ) - (Seq.length (snd a))
        )
    |> Seq.map fst
    |> Seq.head

let findCode data cmp =
    seq { 0 .. 12 }
    |> Seq.fold
        (fun acc i ->
            if Seq.length acc = 1 then
                acc
            else
                let mc = getMostCommon acc i
                Seq.filter (fun s -> cmp (Seq.item i s) mc) acc
        ) data
    |> Seq.head

let part1 =
    readLines<string> 3
    |> zip
    |> Seq.map (fun s -> Seq.toArray s
                        |> Array.groupBy (fun x -> x)
                        |> Array.sortBy fst
                        |> Array.map (fun (_,x) -> Array.length x))
    |> Seq.mapi (fun i e -> (i,e))
    |> Seq.fold
        (fun acc (i,c) ->
            if c.[1] > c.[0] then acc + (1 <<< (11-i)) else acc
        ) 0
    |> fun x -> x * (~~~x &&& 0xFFF)
    |> string

let part2 =
    let data = readLines<string> 3
    let oxygen = findCode data (fun a b -> a = b)
    let co2 = findCode data (fun a b -> a <> b)
    Convert.ToInt32(string oxygen, 2) * Convert.ToInt32(string co2, 2) |> string