namespace Utils

type Point2 =
    { X:int; Y:int; }

    /// Returns the 4 axis-aligned neighbors of this point.
    member this.Neighbors4() = Point2.neighbors4 this.X this.Y

    member this.Neighbors() = Point2.neighbors this.X this.Y

    static member neighbors4 x y = [|
        {X = x; Y = y - 1 };
        {X = x + 1; Y = y };
        {X = x; Y = y + 1 };
        {X = x - 1; Y = y }
        |]

    static member neighbors x y = [|
        {X = x - 1; Y = y - 1 };
        {X = x; Y = y - 1 };
        {X = x + 1; Y = y - 1 };
        {X = x - 1; Y = y };
        {X = x + 1; Y = y };
        {X = x - 1; Y = y + 1 };
        {X = x; Y = y + 1 };
        {X = x + 1; Y = y + 1 }
    |]

    static member create x y = {X = x; Y = y}

    static member zero = {X = 0; Y = 0}

    static member one = {X = 1; Y = 1}