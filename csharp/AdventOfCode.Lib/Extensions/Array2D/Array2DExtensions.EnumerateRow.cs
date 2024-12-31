namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static IEnumerable<T> EnumerateRow<T>(this T[,] source, int row)
    {
        for (var i = 0; i < source.GetLength(1); i++)
            yield return source[row, i];
    }

    public static IEnumerable<IEnumerable<T>> EnumerateRows<T>(this T[,] source)
    {
        for (var row = 0; row < source.GetLength(1); row++)
            yield return source.EnumerateRow(row);
    }
}