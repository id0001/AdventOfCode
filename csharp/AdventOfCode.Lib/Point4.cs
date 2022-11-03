namespace AdventOfCode.Lib;

public readonly struct Point4 : IPoint, IEquatable<Point4>
{
    public Point4(int x, int y, int z, int w) => (X, Y, Z, W) = (x, y, z, w);

    public int X { get; init; }

    public int Y { get; init; }

    public int Z { get; init; }
    
    public int W { get; init; }

    public int this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        3 => W,
        _ => throw new NotSupportedException()
    };

    public int Dimensions => 4;

    public static Point3 Zero { get; } = new();

    IEnumerable<IPoint> IPoint.GetNeighbors(bool includeDiagonal) => GetNeighbors(includeDiagonal).Cast<IPoint>();

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

    public static Point4 Subtract(Point4 left, Point4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

    public static Point4 Add(Point4 left, Point4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W - right.W);

    public void Deconstruct(out int x, out int y, out int z, out int w) => (x, y, z, w) = (X, Y, Z, W);

    public bool Equals(Point4 other) => X == other.X && Y == other.Y && Z == other.Z;

    public bool Equals(IPoint? other)
    {
        if (other is null)
            return false;

        if (other.Dimensions != Dimensions)
            return false;

        for (var d = 0; d < Dimensions; d++)
            if (this[d] != other[d])
                return false;

        return true;
    }

    public override bool Equals(object? obj) => obj is Point4 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public static bool operator ==(Point4 left, Point4 right) => left.Equals(right);

    public static bool operator !=(Point4 left, Point4 right) => !(left == right);
    
    public static Point4 operator +(Point4 left, Point4 right) => Add(left, right);
    
    public static Point4 operator -(Point4 left, Point4 right) => Subtract(left, right);
}