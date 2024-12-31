namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static T At<T>(this T[,] source, Point2 pos) => source[pos.Y, pos.X];
}