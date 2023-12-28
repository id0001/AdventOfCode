using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static partial class Euclid
{
    public static int Modulus(int dividend, int divisor) => (dividend % divisor + divisor) % divisor;

    public static long Modulus(long dividend, long divisor) => (dividend % divisor + divisor) % divisor;

    public static BigInteger Modulus(BigInteger dividend, BigInteger divisor) =>
        (dividend % divisor + divisor) % divisor;

    public static (long, long) ChineseRemainderTheorem(long[] dividends, long[] divisors)
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

        return (total % totalMod, totalMod);
    }
}