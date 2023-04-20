﻿namespace AdventOfCode.Lib;

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

    public static Point2 Zero { get; } = new();

    IEnumerable<IPoint> IPoint.GetNeighbors(bool includeDiagonal) => GetNeighbors(includeDiagonal).Cast<IPoint>();

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

    public static Point2 Add(Point2 left, Point2 right) => new(left.X + right.X, left.Y + right.Y);

    public static Point2 Multiply(Point2 point, int multiplier) => new(point.X * multiplier, point.Y * multiplier);

    public static Point2 Multiply(Point2 left, Point2 right) => new(left.X * right.X, left.Y * right.Y);

    public static int DistanceSquared(Point2 left, Point2 right)
    {
        var dy = right.Y - left.Y;
        var dx = right.X - left.X;
        return dx * dx + dy * dy;
    }

    public static double Distance(Point2 left, Point2 right) => System.Math.Sqrt(DistanceSquared(left, right));

    /// <summary>
    /// Rotates a point around a pivot on a circle by the amount defined by angle
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

        var x = pivot.X + (int)System.Math.Round(cos * dx - sin * dy);
        var y = pivot.Y + (int)System.Math.Round(sin * dx + cos * dy);
        return new Point2(x, y);
    }

    public static IEnumerable<Point2> BresenhamLine(Point2 from, Point2 to)
    {
        if (System.Math.Abs(to.Y - from.Y) < System.Math.Abs(to.X - from.X))
        {
            if (from.X > to.X)
                return BresenhamLineLow(to.X, to.Y, from.X, from.Y).Reverse();
            else
                return BresenhamLineLow(from.X, from.Y, to.X, to.Y);
        }
        else
        {
            if (from.Y > to.Y)
                return BresenhamLineHigh(to.X, to.Y, from.X, from.Y).Reverse();
            else
                return BresenhamLineHigh(from.X, from.Y, to.X, to.Y);
        }
    }

    public static bool operator ==(Point2 left, Point2 right) => left.Equals(right);

    public static bool operator !=(Point2 left, Point2 right) => !(left == right);

    public static Point2 operator +(Point2 left, Point2 right) => Add(left, right);

    public static Point2 operator -(Point2 left, Point2 right) => Subtract(left, right);

    public static Point2 operator *(Point2 left, int multiplier) => Multiply(left, multiplier);

    public static Point2 operator *(Point2 left, Point2 right) => Multiply(left, right);

    public static implicit operator Vector2(Point2 value) => new Vector2(value.X, value.Y);

    private static IEnumerable<Point2> BresenhamLineHigh(int x0, int y0, int x1, int y1)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        int xi = 1;
        if (dx < 0)
        {
            xi = -1;
            dx = -dx;
        }

        int d = (2 * dx) - dy;
        int x = x0;



        for (int y = y0; y <= y1; y++)
        {
            yield return new Point2(x, y);
            if (d > 0)
            {
                x = x + xi;
                d = d + (2 * (dx - dy));
            }
            else
            {
                d = d + 2 * dx;
            }
        }
    }

    private static IEnumerable<Point2> BresenhamLineLow(int x0, int y0, int x1, int y1)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        int yi = 1;
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }

        int d = (2 * dy) - dx;
        int y = y0;

        for (int x = x0; x <= x1; x++)
        {
            yield return new Point2(x, y);
            if (d > 0)
            {
                y = y + yi;
                d = d + (2 * (dy - dx));
            }
            else
            {
                d = d + 2 * dy;
            }
        }
    }
}