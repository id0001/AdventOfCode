namespace AdventOfCode.Lib;

public static class DoubleExtensions
{
    public static bool IsSimilarTo(this double value, double other, int precision = 6) =>
        System.Math.Abs(value - other) < System.Math.Pow(10, -System.Math.Abs(precision));
}