module Challenges.Challenge06

open Utils.IO

type FishGroup = {
    SexyTime:int;
    Amount:int64
    }

let groupFish data =
    data
    |> List.groupBy (fun x -> x)
    |> List.map (fun (k,v) -> {SexyTime = int k; Amount = int64 (List.length v) })

let breed groups =
    groups
    |> List.collect (
        fun group ->
            match group.SexyTime with
            | 0 -> [{group with SexyTime = 6}; {group with SexyTime = 8}]
            | x -> [{group with SexyTime = x-1;}]
                )
    |> List.groupBy (fun x -> x.SexyTime)
    |> List.map (fun (k,v) -> {SexyTime = k; Amount = v |> List.sumBy (fun x -> x.Amount)})

let rec cycle day cycleAmount groups =
    if day = cycleAmount then
        groups
    else
        cycle (day+1) cycleAmount (breed groups)

let part1 =
    readLine<int64> 6 ','
    |> Seq.toList
    |> groupFish
    |> cycle 0 80
    |> List.sumBy (fun x -> x.Amount)
    |> string

let part2 =
    readLine<int64> 6 ','
    |> Seq.toList
    |> groupFish
    |> cycle 0 256
    |> List.sumBy (fun x -> x.Amount)
    |> string