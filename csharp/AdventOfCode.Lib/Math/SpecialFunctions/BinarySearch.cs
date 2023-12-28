namespace AdventOfCode.Lib.Math;

public static partial class SpecialFunctions
{
    public static long? BinarySearch(long min, long max, Func<long, long> compare)
    {
        while (min < max)
        {
            var m = (long) System.Math.Floor((min + max) / 2d);
            var cmp = compare(m);
            if (cmp < 0)
                min = m + 1;
            else if (cmp > 0)
                max = m - 1;
            else
                return m;
        }

        return null;
    }
}