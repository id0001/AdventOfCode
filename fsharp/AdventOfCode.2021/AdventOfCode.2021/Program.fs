open System
open Utils
open Challenges.Challenge11

[<EntryPoint>]
let main argv =
    Day.executeAndPrint setup part1 part2

    printfn ""
    printfn "Press any key to continue..."
    Console.ReadKey true |> ignore
    0