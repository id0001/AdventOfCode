namespace AdventOfCode.Lib;

public readonly struct Point3 : IPoint, IEquatable<Point3>
{
    public Point3(int x, int y, int z) => (X, Y, Z) = (x, y, z);

    public int X { get; init; }

    public int Y { get; init; }

    public int Z { get; init; }

    public int this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new NotSupportedException()
    };

    public int Dimensions => 3;

    public static Point3 Zero { get; } = new();

    IEnumerable<IPoint> IPoint.GetNeighbors(bool includeDiagonal) => GetNeighbors(includeDiagonal).Cast<IPoint>();

    public IEnumerable<Point3> GetNeighbors(bool includeDiagonal = false)
    {
        for (var z = -1; z <= 1; z++)
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0) ^ (y == 0) ^ (z == 0)))
                continue;

            if (x == 0 && y == 0 && z == 0)
                continue;

            yield return new Point3(X + x, Y + y, Z + z);
        }
    }

    public Point2 ToPoint2() => new(X, Y);
    
    public static Point3 Subtract(Point3 left, Point3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static Point3 Add(Point3 left, Point3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

    public bool Equals(Point3 other) => X == other.X && Y == other.Y && Z == other.Z;

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

    public override bool Equals(object? obj) => obj is Point3 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static bool operator ==(Point3 left, Point3 right) => left.Equals(right);

    public static bool operator !=(Point3 left, Point3 right) => !(left == right);
    
    public static Point3 operator +(Point3 left, Point3 right) => Add(left, right);
    
    public static Point3 operator -(Point3 left, Point3 right) => Subtract(left, right);
}