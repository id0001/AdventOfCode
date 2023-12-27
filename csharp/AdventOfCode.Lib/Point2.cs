namespace AdventOfCode.Lib;

public readonly record struct Point2(int X, int Y) : IPoint<int>, INeighbors<int>
{
    public static readonly Point2 Zero = new();

    int IPoint<int>.this[int index] => index switch
    {
        0 => X,
        1 => Y,
        _ => throw new NotSupportedException()
    };

    int IPoint<int>.Dimensions => 2;

    public Point2 Up => new(X, Y - 1);

    public Point2 Right => new(X + 1, Y);

    public Point2 Down => new(X, Y + 1);

    public Point2 Left => new(X - 1, Y);

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

    public override string ToString() => $"({X},{Y})";

    public static Point2 Subtract(Point2 left, Point2 right) => new(left.X - right.X, left.Y - right.Y);

    public static Point2 Add(Point2 left, Point2 right) => new(left.X + right.X, left.Y + right.Y);

    public static Point2 Multiply(Point2 point, int multiplier) => new(point.X * multiplier, point.Y * multiplier);

    public static int DistanceSquared(Point2 left, Point2 right)
    {
        var dy = right.Y - left.Y;
        var dx = right.X - left.X;
        return dx * dx + dy * dy;
    }

    public static double Distance(Point2 left, Point2 right) => System.Math.Sqrt(DistanceSquared(left, right));

    /// <summary>
    ///     Rotates a point around a pivot on a circle by the amount defined by angle
    /// </summary>
    /// <param name="point">The point to move</param>
    /// <param name="pivot">The pivot to rotate around</param>
    /// <param name="angle">The angle in radians to rotate by</param>
    /// <returns>The new rotated point</returns>
    public static Point2 Turn(Point2 point, Point2 pivot, double angle)
    {
        var sin = System.Math.Sin(angle);
        var cos = System.Math.Cos(angle);
        var (dx, dy) = point - pivot;

        var x = pivot.X + (int) System.Math.Round(cos * dx - sin * dy);
        var y = pivot.Y + (int) System.Math.Round(sin * dx + cos * dy);
        return new Point2(x, y);
    }

    public static IEnumerable<Point2> BresenhamLine(Point2 from, Point2 to)
    {
        if (System.Math.Abs(to.Y - from.Y) < System.Math.Abs(to.X - from.X))
        {
            if (from.X > to.X)
                return BresenhamLineLow(to.X, to.Y, from.X, from.Y).Reverse();
            return BresenhamLineLow(from.X, from.Y, to.X, to.Y);
        }

        if (from.Y > to.Y)
            return BresenhamLineHigh(to.X, to.Y, from.X, from.Y).Reverse();
        return BresenhamLineHigh(from.X, from.Y, to.X, to.Y);
    }

    public static int ManhattanDistance(Point2 p0, Point2 p1) =>
        System.Math.Abs(p1.X - p0.X) + System.Math.Abs(p1.Y - p0.Y);

    /// <summary>
    ///     Enumerate the points that cover the border at the manhattan distance from the given center point.
    /// </summary>
    /// <param name="center">The center</param>
    /// <param name="distance">The manhattan distance</param>
    /// <returns>An enumerable of Point2 that cover the border</returns>
    public static IEnumerable<Point2> ManhattanBorder(Point2 center, int distance)
    {
        for (var x = center.X - distance; x <= center.X + distance; x++)
        {
            var dy = distance - System.Math.Abs(center.X - x);

            yield return new Point2(x, center.Y - dy);
            if (dy > 0)
                yield return new Point2(x, center.Y + dy);
        }
    }

    public static Point2 operator +(Point2 left, Point2 right) => Add(left, right);

    public static Point2 operator -(Point2 left, Point2 right) => Subtract(left, right);

    public static Point2 operator *(Point2 left, int right) => Multiply(left, right);

    public static Point2 operator *(int left, Point2 right) => Multiply(right, left);

    public static implicit operator Vector2(Point2 value) => new(value.X, value.Y);

    private static IEnumerable<Point2> BresenhamLineHigh(int x0, int y0, int x1, int y1)
    {
        var dx = x1 - x0;
        var dy = y1 - y0;
        var xi = 1;
        if (dx < 0)
        {
            xi = -1;
            dx = -dx;
        }

        var d = 2 * dx - dy;
        var x = x0;


        for (var y = y0; y <= y1; y++)
        {
            yield return new Point2(x, y);
            if (d > 0)
            {
                x = x + xi;
                d = d + 2 * (dx - dy);
            }
            else
            {
                d = d + 2 * dx;
            }
        }
    }

    private static IEnumerable<Point2> BresenhamLineLow(int x0, int y0, int x1, int y1)
    {
        var dx = x1 - x0;
        var dy = y1 - y0;
        var yi = 1;
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }

        var d = 2 * dy - dx;
        var y = y0;

        for (var x = x0; x <= x1; x++)
        {
            yield return new Point2(x, y);
            if (d > 0)
            {
                y = y + yi;
                d = d + 2 * (dy - dx);
            }
            else
            {
                d = d + 2 * dy;
            }
        }
    }
}