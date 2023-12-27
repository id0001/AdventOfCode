namespace AdventOfCode.Lib;

public readonly record struct Point3(int X, int Y, int Z) : IPoint<int>, INeighbors<int>
{
    public static readonly Point3 Zero = new();
    public static readonly Point3 One = new(1, 1, 1);

    int IPoint<int>.this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new NotSupportedException()
    };

    int IPoint<int>.Dimensions => 3;

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

    public IEnumerable<Point3> GetNeighbors(bool includeDiagonal = false)
    {
        for (var z = -1; z <= 1; z++)
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0 && y == 0) ^ (x == 0 && z == 0) ^ (y == 0 && z == 0)))
                continue;

            if (x == 0 && y == 0 && z == 0)
                continue;

            yield return new Point3(X + x, Y + y, Z + z);
        }
    }

    public Point2 ToPoint2() => new(X, Y);

    public static Point3 Subtract(Point3 left, Point3 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static Point3 Add(Point3 left, Point3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Point3 Multiply(Point3 left, int multiplier) =>
        new(left.X * multiplier, left.Y * multiplier, left.Z * multiplier);

    public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

    public override string ToString() => $"({X},{Y},{Z})";

    public static Point3 operator +(Point3 left, Point3 right) => Add(left, right);

    public static Point3 operator -(Point3 left, Point3 right) => Subtract(left, right);

    public static Point3 operator *(Point3 left, int right) => Multiply(left, right);
}