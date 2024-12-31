namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
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