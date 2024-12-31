namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static IEnumerable<T> EnumerateDownRightDiagonal<T>(this T[,] source, int diagonal)
    {
        var width = source.GetLength(1);
        var height = source.GetLength(0);

        var (x, y) = diagonal < height
            ? (0, height - diagonal - 1)
            : (diagonal - (height - 1), 0);

        for (; x < width && y < height; x++, y++)
            yield return source[y, x];
    }

    public static IEnumerable<IEnumerable<T>> EnumerateDownRightDiagonals<T>(this T[,] source)
    {
        for (var diagonal = 0; diagonal < source.GetDiagonalLength(); diagonal++)
            yield return source.EnumerateDownRightDiagonal(diagonal);
    }
}