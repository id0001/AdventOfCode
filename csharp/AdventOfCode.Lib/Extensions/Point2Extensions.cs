namespace AdventOfCode.Lib;

public static class Point2Extensions
{
    public static int ToIndex(this Point2 source, int width) => source.Y * width + source.X;
}