namespace AdventOfCode.Lib;

public readonly struct Point2 : IPoint, IEquatable<Point2>
{
    public Point2(int x, int y) => (X, Y) = (x, y);

    public int this[int index] => index switch
    {
        0 => X,
        1 => Y,
        _ => throw new NotSupportedException()
    };

    public int X { get; init; }

    public int Y { get; init; }

    public int Dimensions => 2;

    public static Point2 Empty { get; } = new();

    public IEnumerable<Point2> GetNeighbors(bool includeDiagonal = false)
    {
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0) ^ (y == 0)))
                continue;

            if (x == 0 && y == 0)
                continue;

            yield return new Point2(X + x, Y + y);
        }
    }

    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

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

    public bool Equals(Point2 other) => other.X == X && other.Y == Y;

    public override string ToString() => $"({X},{Y})";

    public override bool Equals(object? obj) => obj is Point2 point2 && Equals(point2);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static Point2 Subtract(Point2 left, Point2 right) => new(left.X - right.X, left.Y - right.Y);

    public static bool operator ==(Point2 left, Point2 right) => left.Equals(right);

    public static bool operator !=(Point2 left, Point2 right) => !(left == right);

    public static Point2 operator -(Point2 left, Point2 right) => Subtract(left, right);
}