namespace Challenges

module Challenge01 =

    open Utils.IO

    let cmp pair = fst pair < snd pair

    let part1 =
        readLines<int> 1
            |> Seq.pairwise
            |> Seq.filter cmp
            |> Seq.length
            |> string

    let part2 =
        readLines<int> 1
            |> Seq.windowed 3
            |> Seq.map Array.sum
            |> Seq.pairwise
            |> Seq.filter cmp
            |> Seq.length
            |> string