namespace Utils

open System.Diagnostics

type TimedOperation<'T> = { Elapsed:float; Value:'T }

module Day =
    let timeOperation<'T> (func: unit -> 'T) : TimedOperation<'T> =
        let timer = Stopwatch.StartNew()
        let returnValue = func()
        timer.Stop()
        { Elapsed = timer.Elapsed.TotalMilliseconds; Value = returnValue }

    let executePart<'T> (input:'T) (part:'T -> string) =
        timeOperation (fun () -> part input)

    let execute<'T> (setup: unit -> 'T) (part1:'T -> string) (part2:'T -> string) =
        let result1 = executePart (setup()) part1
        let result2 = executePart (setup()) part2
        (result1, result2)

    let executeAndPrint<'T> (setup: unit -> 'T) (part1:'T -> string) (part2:'T -> string) =
        let (result1, result2) = execute setup part1 part2
        if (String.length result1.Value) > 0 then
            printfn "- Part 1 (%.3fms): %s" result1.Elapsed result1.Value

        if (String.length result2.Value) > 0 then
            printfn "- Part 2 (%.3Fms): %s" result2.Elapsed result2.Value