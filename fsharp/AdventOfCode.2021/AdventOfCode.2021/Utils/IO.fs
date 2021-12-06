module Utils.IO

open System
open System.IO

let InputFolder = "Inputs"

let private challenge2Filename challenge =
    Path.Combine [| InputFolder; sprintf "%02d.txt" challenge |]

let readLines<'T> (challenge:int) =
    let filename = challenge2Filename challenge
    File.ReadAllLines(filename)
        |> Seq.map (fun x -> Convert.ChangeType(x, typeof<'T>) :?> 'T)

let readLine<'T> (challenge:int) (separator:char) =
    let filename = challenge2Filename challenge
    File.ReadAllText (filename)
    |> (fun line -> line.Split(separator))
    |> Seq.map (fun s -> Convert.ChangeType(s, typeof<'T>) :?> 'T)