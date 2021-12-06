module Challenges.Challenge06

open Utils.IO

let groupFish (data:int64 list) =
    let grouped = data
                |> List.groupBy (fun x -> x)
                |> List.map (fun (a,b) -> (int a,b))
                |> Map.ofList

    [|for i in 0..8 -> int64 i|]
    |> Array.mapi (
        fun i _ ->
            match Map.tryFind i grouped with
            | Some v -> int64 (List.length v)
            | None -> int64 0
        )

let rec breed dayFrom totalDays groups =
    match dayFrom with
    | d when d = totalDays -> groups
    | d -> breed (d+1) totalDays [| groups.[1]; groups.[2]; groups.[3]; groups.[4]; groups.[5]; groups.[6] ; groups.[7] + groups.[0]; groups.[8]; groups.[0] |]

let calculateFishies totalDays groups =
    breed 0 totalDays groups
    |> Array.sum

let part1 =
    readLine<int64> 6 ','
    |> Seq.toList
    |> groupFish
    |> calculateFishies 80
    |> string

let part2 =
    readLine<int64> 6 ','
    |> Seq.toList
    |> groupFish
    |> calculateFishies 256
    |> string