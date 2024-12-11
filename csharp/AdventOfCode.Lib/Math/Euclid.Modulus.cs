using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static partial class Euclid
{
    public static TNumber Modulus<TNumber>(TNumber divident, TNumber divisor)
        where TNumber : IBinaryInteger<TNumber>
        => (divident % divisor + divisor) % divisor;

    public static long ChineseRemainderTheorem(long[] dividends, long[] divisors)
    {
        var totalMod = divisors.Product();
        var total = 0L;
        for (var i = 0; i < divisors.Length; i++)
        {
            var ni = totalMod / divisors[i];
            var xi = ModInverse(ni, divisors[i]);
            var bi = dividends[i];
            total += ni * xi * bi;
        }

        return Modulus(total, totalMod);
    }
}