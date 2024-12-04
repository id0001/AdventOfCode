namespace AdventOfCode.Lib;

public static class Array2dExtensions
{
    public static Rectangle Bounds<T>(this T[,] source) => new(0, 0, source.GetLength(1), source.GetLength(0));
}