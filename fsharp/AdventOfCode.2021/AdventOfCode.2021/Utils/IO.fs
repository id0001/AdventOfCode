namespace Utils

module IO =

    open System
    open System.IO

    let InputFolder = "Inputs"

    let private challenge2Filename challenge =
        Path.Combine [| InputFolder; sprintf "%02d.txt" challenge |]

    let readLines<'T> challenge =
        let filename = challenge2Filename challenge
        File.ReadAllLines(filename)
            |> Seq.map (fun x -> Convert.ChangeType(x, typeof<'T>) :?> 'T)