namespace AdventOfCode.Lib;

public static class Array2DExtensions
{
    public static Point2 FindPosition<T>(this T[,] source, Func<T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
            if (predicate(source[y, x]))
                return new Point2(x, y);

        throw new InvalidOperationException("No element matching the condition was found");
    }

    public static Point2 FindPosition<T>(this T[,] source, Func<Point2, T, bool> predicate)
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

    public static Rectangle Bounds<T>(this T[,] source) => new(0, 0, source.GetLength(1), source.GetLength(0));

    public static void PrintToConsole<T>(this T[,] source, Func<Point2, T, char> selector)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                var p = new Point2(x, y);
                var c = selector(p, source[y, x]);
                Console.Write(c);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }

    public static int Count<T>(this T[,] source, Func<T, bool> predicate)
    {
        var c = 0;
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
            if (predicate(source[y, x]))
                c++;

        return c;
    }
}