module Challenges.Challenge05

open Utils.IO
open System.Text.RegularExpressions
open System

type Point = {X:int; Y:int}
type Segment  = {Start:Point; End:Point}

let createPoint x y = {Point.X = x; Point.Y = y}
let createSegment p0 p1 = {Segment.Start = p0; Segment.End = p1}

let getSegments =
    readLines<string> 5
    |> Seq.map (
        fun line -> Regex.Match(line, @"^(\d+),(\d+) -> (\d+),(\d+)$")
                    |> fun m -> createSegment (createPoint (m.Groups[1].Value |> int) (m.Groups[2].Value |> int)) (createPoint (m.Groups[3].Value |> int) (m.Groups[4].Value |> int))
                )

let line p0 p1 =
    let dx = Math.Sign(p1.X - p0.X)
    let dy = Math.Sign(p1.Y - p0.Y)
    let len = Math.Max(Math.Abs(p1.X-p0.X), Math.Abs(p1.Y-p0.Y))

    Seq.zip
        (match dx with
        | 0 -> seq { for _ in 0..len -> p0.X }
        | _ -> seq { for x in p0.X..dx..p1.X -> x })
        (match dy with
        | 0 -> seq { for _ in 0..len -> p0.Y }
        | _ -> seq { for y in p0.Y..dy..p1.Y -> y })
    |> Seq.map (fun (x,y) -> createPoint x y)

let part1 =
    getSegments
    |> Seq.filter (fun s -> s.Start.X = s.End.X || s.Start.Y = s.End.Y)
    |> Seq.collect (fun s -> line s.Start s.End)
    |> Seq.groupBy (fun s -> s)
    |> Seq.filter (fun x -> (Seq.length (snd x)) >= 2)
    |> Seq.length
    |> string

let part2 =
    getSegments
    |> Seq.collect (fun s -> line s.Start s.End)
    |> Seq.groupBy (fun s -> s)
    |> Seq.filter (fun x -> (Seq.length (snd x)) >= 2)
    |> Seq.length
    |> string
