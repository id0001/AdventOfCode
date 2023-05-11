namespace AdventOfCode.Lib.Math;

/// <summary>
/// Math functions related to integers
/// </summary>
public static partial class Euclid
{
    public static long LeastCommonMultiple(long a, long b)
    {
        if (a == 0 || b == 0)
            return 0;

        return System.Math.Abs((a / GreatestCommonDivisor(a, b)) * b);
    }

    public static long LeastCommonMultiple(params long[] integers)
    {
        if (integers.Length == 0)
            return 1;

        var lcm = System.Math.Abs(integers[0]);
        for (var i = 1; i < integers.Length; i++)
            lcm = LeastCommonMultiple(lcm, integers[i]);

        return lcm;
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var remainder = a % b;
            a = b;
            b = remainder;
        }

        return System.Math.Abs(a);
    }

    public static bool IsPowerOfTwo(int number) => number > 0 && (number & (number - 1)) == 0x0;
    public static bool IsPowerOfTwo(long number) => number > 0 && (number & (number - 1)) == 0x0;

    /// <summary>
    /// Calculates the triangular number of the given number.
    /// https://en.wikipedia.org/wiki/Triangular_number
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int TriangularNumber(int number) => (number * (number + 1)) / 2;

    public static int InverseTriangleNumber(int number) => (int)System.Math.Sqrt(number * 2);
}