namespace Utils

type Point2 =
    { X:int; Y:int; }

    /// Returns the 4 axis-aligned neighbors of this point.
    member this.Neighbors4() = [|
        {X = this.X; Y = this.Y - 1 };
        {X = this.X + 1; Y = this.Y };
        {X = this.X; Y = this.Y + 1 };
        {X = this.X - 1; Y = this.Y }
        |]

    static member neighbors4 x y = [|
        {X = x; Y = y - 1 };
        {X = x + 1; Y = y };
        {X = x; Y = y + 1 };
        {X = x - 1; Y = y }
        |]

    static member create x y = {X = x; Y = y}

    static member zero = {X = 0; Y = 0}

    static member one = {X = 1; Y = 1}