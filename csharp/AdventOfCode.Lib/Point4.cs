namespace AdventOfCode.Lib;

public readonly record struct Point4(int X, int Y, int Z, int W) : IPoint<int>, INeighbors<int>
{
    public static readonly Point3 Zero = new();

    int IPoint<int>.this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        3 => W,
        _ => throw new NotSupportedException()
    };

    int IPoint<int>.Dimensions => 4;

    IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal) => GetNeighbors(includeDiagonal).Cast<IPoint<int>>();

    public bool Equals(IPoint<int>? other)
    {
        if (other is null)
            return false;

        var instance = (IPoint<int>) this;

        if (other.Dimensions != instance.Dimensions)
            return false;

        for (var d = 0; d < instance.Dimensions; d++)
            if (instance[d] != other[d])
                return false;

        return true;
    }

    public IEnumerable<Point4> GetNeighbors(bool includeDiagonal = false)
    {
        for (var w = -1; w <= 1; w++)
        for (var z = -1; z <= 1; z++)
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0) ^ (y == 0) ^ (z == 0) ^ (w == 0)))
                continue;

            if (x == 0 && y == 0 && z == 0 && w == 0)
                continue;

            yield return new Point4(X + x, Y + y, Z + z, W + w);
        }
    }

    public static Point4 Subtract(Point4 left, Point4 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

    public static Point4 Add(Point4 left, Point4 right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W - right.W);

    public static Point4 Multiply(Point4 left, int multiplier) => new(left.X * multiplier, left.Y * multiplier,
        left.Z * multiplier, left.W * multiplier);

    public void Deconstruct(out int x, out int y, out int z, out int w) => (x, y, z, w) = (X, Y, Z, W);

    public static Point4 operator +(Point4 left, Point4 right) => Add(left, right);

    public static Point4 operator -(Point4 left, Point4 right) => Subtract(left, right);

    public static Point4 operator *(Point4 left, int right) => Multiply(left, right);
}