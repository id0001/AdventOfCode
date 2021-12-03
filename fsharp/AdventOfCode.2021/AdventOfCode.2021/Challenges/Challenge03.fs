module Challenges.Challenge03

open Utils.IO
open System

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
    let lines =readLines<string> 3
    let len = Seq.length (Seq.head lines)
    let counts = seq {
        for i in 0..len-1 do
            lines
            |> Seq.groupBy (fun x -> Seq.item i x)
            |> Seq.sortBy fst
            |> Seq.map (fun (k,v) -> Seq.length v)
            |> Seq.toArray
        }

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

let part2 =
    let data = readLines<string> 3
    let oxygen = findCode data (fun a b -> a = b)
    let co2 = findCode data (fun a b -> a <> b)
    Convert.ToInt32(string oxygen, 2) * Convert.ToInt32(string co2, 2) |> string