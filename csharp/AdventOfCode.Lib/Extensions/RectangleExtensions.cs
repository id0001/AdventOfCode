namespace AdventOfCode.Lib;

public static class RectangleExtensions
{
    public static bool Contains(this Rectangle source, Point2 p)
        => p.X >= source.Left && p.X < source.Right && p.Y >= source.Top && p.Y < source.Bottom;

    public static bool IntersectsWith(this Rectangle source, Rectangle target)
        => Rectangle.Intersects(source, target);

    public static Rectangle Intersect(this Rectangle source, Rectangle target)
        => Rectangle.Intersect(source, target);

    public static Rectangle Expand(this Rectangle source, int amount)
        => new(source.X - amount, source.Y - amount, source.Width + amount, source.Height + amount);

    public static IEnumerable<Point2> AsGridPoints(this Rectangle source)
    {
        for (var y = source.Top; y < source.Bottom; y++)
        for (var x = source.Left; x < source.Right; x++)
            yield return new Point2(x, y);
    }
}