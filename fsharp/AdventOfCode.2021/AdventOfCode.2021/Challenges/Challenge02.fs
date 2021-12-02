namespace Challenges

module Challenge02 =

    open Utils.IO
    open System

    type Movement = { Direction:string; Amount:int }

    let mapMovement (line:string )=
        line.Split(" ", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            |> fun x -> { Movement.Direction = x.[0]; Movement.Amount = int(x.[1]) }

    let readInput =
        readLines 2
            |> Seq.map mapMovement

    let move (m:Movement) =
        match m.Direction with
        | "forward" -> (m.Amount,0)
        | "down"-> (0,m.Amount)
        | "up" -> (0,-m.Amount)
        | _ -> raise (NotImplementedException ())

    let fold1 (x,y) (m:Movement) =
        let (dx,dy) = move m
        (x+dx,y+dy)

    let fold2 (x,y,aim) (m:Movement) =
        let (dx,daim) = move m
        (x+dx,y + dx*aim,aim + daim)

    let part1 =
        readInput
            |> Seq.fold fold1 (0,0)
            |> fun (x,y) -> x*y
            |> string

    let part2 =
        readInput
        |> Seq.fold fold2 (0,0,0)
        |> fun (x,y,_) -> x*y
        |> string
