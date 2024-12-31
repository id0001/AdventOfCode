namespace AdventOfCode.Lib;

public static class Point2Extensions
{
    public static IEnumerable<Point2> GetNeighbors(this Point2 source, bool includeDiagonal = false)
    {
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0) ^ (y == 0)))
                continue;

            if (x == 0 && y == 0)
                continue;

            yield return new Point2(source.X + x, source.Y + y);
        }
    }

    public static IEnumerable<Point2> ManhattanBorder(this Point2 source, int distance)
    {
        for (var x = source.X - distance; x <= source.X + distance; x++)
        {
            var dy = distance - System.Math.Abs(source.X - x);

            yield return new Point2(x, source.Y - dy);
            if (dy > 0)
                yield return new Point2(x, source.Y + dy);
        }
    }

    /// <summary>
    ///     Rotates a point around a pivot on a circle by the amount defined by angle
    /// </summary>
    /// <param name="point">The point to move</param>
    /// <param name="pivot">The pivot to rotate around</param>
    /// <param name="angle">The angle in radians to rotate by</param>
    /// <returns>The new rotated point</returns>
    public static Point2 TurnAround(this Point2 point, Point2 pivot, double angle)
    {
        var sin = System.Math.Sin(angle);
        var cos = System.Math.Cos(angle);
        var (dx, dy) = point - pivot;

        var x = pivot.X + (int) System.Math.Round(cos * dx - sin * dy);
        var y = pivot.Y + (int) System.Math.Round(sin * dx + cos * dy);
        return new Point2(x, y);
    }
}