using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static partial class SpecialFunctions
{
    public static TNumber? BinarySearch<TNumber>(TNumber min, TNumber max, Func<TNumber, TNumber> compare)
        where TNumber : IBinaryNumber<TNumber>
    {
        while (min < max)
        {
            var m = (min + max) / TNumber.CreateChecked(2);
            var cmp = compare(m);
            if (cmp < TNumber.Zero)
                min = m + TNumber.One;
            else if (cmp > TNumber.Zero)
                max = m - TNumber.One;
            else
                return m;
        }

        return default;
    }
}