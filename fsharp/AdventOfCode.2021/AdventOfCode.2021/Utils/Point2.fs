namespace Utils

type Point2 =
    { X:int; Y:int; }

    /// Returns the 4 axis-aligned neighbors of this point.
    member this.Neighbors4() =
        [for x in -1..1 do for y in -1..1 -> {X=x;Y=y}]
        |> Seq.filter (fun p -> (p.X = 0 || p.Y = 0) && not(p.X = 0 && p.Y = 0))
        |> Seq.map (fun p -> {X = (this.X + p.X); Y = (this.Y + p.Y)})

