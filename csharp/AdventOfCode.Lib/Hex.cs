namespace AdventOfCode.Lib;

public readonly record struct Hex(int X, int Y, int Z) : IPoint<int>, INeighbors<int>
{
    public static readonly Hex Zero = new();

    public Hex North => new(X, Y - 1, Z + 1);

    public Hex NorthEast => new(X + 1, Y - 1, Z);

    public Hex SouthEast => new(X + 1, Y, Z - 1);

    public Hex South => new(X, Y + 1, Z - 1);

    public Hex SouthWest => new(X - 1, Y + 1, Z);

    public Hex NorthWest => new(X - 1, Y, Z + 1);

    public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

    public override string ToString() => $"{X},{Y},{Z}";

    public static int ManhattanDistance(Hex a, Hex b) =>
        (System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y) + System.Math.Abs(a.Z - b.Z)) / 2;

    #region Interface implementations

    int IPoint<int>.this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new NotSupportedException()
    };

    int IPoint<int>.Dimensions => 3;

    IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal)
        => this.GetNeighbors().Cast<IPoint<int>>();

    #endregion
}