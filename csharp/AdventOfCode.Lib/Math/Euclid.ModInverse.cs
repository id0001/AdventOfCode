using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static partial class Euclid
{
    //-----------------------------------------------------------------------------------------
    /// <summary>
    ///     Find the modular inverse using the extended euclid algorithm.
    ///     Assumes a and m are coprimes, i.e., gcd(a,m) = 1
    /// </summary>
    /// <param name="a">The value</param>
    /// <param name="m">The modulo</param>
    /// <returns></returns>
    public static int ModInverse(int a, int m)
    {
        var m0 = m;
        var y = 0;
        var x = 1;

        if (m == 1)
            return 0;

        while (a > 1)
        {
            // q is quotient
            var q = a / m;

            var t = m;

            // m is remainder now, process same as Euclid's algorithm.
            m = a % m;
            a = t;
            t = y;

            // Update x and y
            y = x - q * y;
            x = t;
        }

        // Make positive
        if (x < 0)
            x = x + m0;

        return x;
    }

    //-----------------------------------------------------------------------------------------
    /// <summary>
    ///     Find the modular inverse using the extended euclid algorithm.
    ///     Assumes a and m are coprimes, i.e., gcd(a,m) = 1
    /// </summary>
    /// <param name="a">The value</param>
    /// <param name="m">The modulo</param>
    /// <returns></returns>
    public static long ModInverse(long a, long m)
    {
        var m0 = m;
        var y = 0L;
        var x = 1L;

        if (m == 1)
            return 0;

        while (a > 1)
        {
            // q is quotient
            var q = a / m;

            var t = m;

            // m is remainder now, process same as Euclid's algorithm.
            m = a % m;
            a = t;
            t = y;

            // Update x and y
            y = x - q * y;
            x = t;
        }

        // Make positive
        if (x < 0)
            x = x + m0;

        return x;
    }

    //-----------------------------------------------------------------------------------------
    /// <summary>
    ///     Find the modular inverse using the extended euclid algorithm.
    ///     Assumes a and m are coprimes, i.e., gcd(a,m) = 1
    /// </summary>
    /// <param name="a">The value</param>
    /// <param name="m">The modulo</param>
    /// <returns></returns>
    public static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        var m0 = m;
        var y = BigInteger.Zero;
        var x = BigInteger.One;

        if (m == BigInteger.One)
            return BigInteger.Zero;

        while (a > BigInteger.One)
        {
            // q is quotient
            var q = a / m;

            var t = m;

            // m is remainder now, process same as Euclid's algorithm.
            m = a % m;
            a = t;
            t = y;

            // Update x and y
            y = x - q * y;
            x = t;
        }

        // Make positive
        if (x < BigInteger.Zero)
            x = x + m0;

        return x;
    }
}