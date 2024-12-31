namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static Point2 Find<T>(this T[,] source, Func<T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
            if (predicate(source[y, x]))
                return new Point2(x, y);

        throw new InvalidOperationException("No element matching the condition was found");
    }

    public static Point2 Find<T>(this T[,] source, Func<Point2, T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
        {
            var p = new Point2(x, y);
            if (predicate(p, source[y, x]))
                return p;
        }

        throw new InvalidOperationException("No element matching the condition was found");
    }
}