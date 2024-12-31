namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static int GetDiagonalLength<T>(this T[,] source) => source.GetLength(0) + source.GetLength(1);
}