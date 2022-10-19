using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static class Euclid
{
    public static int Remainder(int dividend, int divisor) => dividend % divisor;
    
    public static long Remainder(long dividend, long divisor) => dividend % divisor;

    public static double Remainder(double dividend, double divisor) => dividend % divisor;

    public static int Modulus(int dividend, int divisor) => (dividend % divisor + divisor) % divisor;
    
    public static long Modulus(long dividend, long divisor) => (dividend % divisor + divisor) % divisor;

    public static double Modulus(double dividend, double divisor) => (dividend % divisor + divisor) % divisor;

    public static BigInteger Modulus(BigInteger dividend, BigInteger divisor) =>
        (dividend % divisor + divisor) % divisor;

    public static long LeastCommonMultiple(long a, long b)
    {
        if (a == 0 || b == 0)
            return 0;

        return System.Math.Abs((a/GreatestCommonDivisor(a, b))*b);
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
            var remainder = a%b;
            a = b;
            b = remainder;
        }

        return System.Math.Abs(a);
    }
    
    //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Find the modular inverse using the extended euclid algorithm.
        /// Assumes a and m are coprimes, i.e., gcd(a,m) = 1
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
        /// Find the modular inverse using the extended euclid algorithm.
        /// Assumes a and m are coprimes, i.e., gcd(a,m) = 1
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
        /// Find the modular inverse using the extended euclid algorithm.
        /// Assumes a and m are coprimes, i.e., gcd(a,m) = 1
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
        
        public static bool IsPowerOfTwo(int number) => number > 0 && (number & (number - 1)) == 0x0;
        public static bool IsPowerOfTwo(long number) => number > 0 && (number & (number - 1)) == 0x0;
}