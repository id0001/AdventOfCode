namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static IEnumerable<T> EnumerateDownLeftDiagonal<T>(this T[,] source, int diagonal)
    {
        var width = source.GetLength(1);
        var height = source.GetLength(0);

        var (x, y) = diagonal < width
            ? (diagonal, 0)
            : (width - 1, diagonal - (width - 1));

        for (; x >= 0 && y < height; x--, y++)
            yield return source[y, x];
    }

    public static IEnumerable<IEnumerable<T>> EnumerateDownLeftDiagonals<T>(this T[,] source)
    {
        for (var diagonal = 0; diagonal < source.GetDiagonalLength(); diagonal++)
            yield return source.EnumerateDownLeftDiagonal(diagonal);
    }
}