open System
open Challenges.Challenge06

let executePart part fn =
    printfn "Part %d: %s" part fn

[<EntryPoint>]
let main argv =
    executePart 1 part1
    executePart 2 part2

    printfn ""
    printfn "Press any key to continue..."
    Console.ReadKey true |> ignore
    0