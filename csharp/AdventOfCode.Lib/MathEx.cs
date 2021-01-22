using System;
using System.Linq;

namespace AdventOfCode.Lib
{
	public static class MathEx
	{
		/// <summary>
		/// Convert degrees to radians
		/// </summary>
		/// <param name="degrees">The degee value</param>
		/// <returns>The radian value</returns>
		public static double ToRadians(double degrees) => (Math.PI / 180d) * degrees;

		/// <summary>
		/// Convert radians to degrees
		/// </summary>
		/// <param name="radians">The radian value</param>
		/// <returns>The degree value</returns>
		public static double ToDegrees(double radians, int decimals = 0) => Math.Round((180d / Math.PI) * radians, decimals);

		//-----------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the modulo of x and d.
		/// </summary>
		/// <param name="x">The value</param>
		/// <param name="d">The divisor</param>
		/// <returns>The modulo of x and d</returns>
		public static int Mod(int x, int d) => (x % d + d) % d;

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
			int m0 = m;
			int y = 0;
			int x = 1;

			if (m == 1)
				return 0;

			while (a > 1)
			{
				// q is quotient
				int q = a / m;

				int t = m;

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
			long m0 = m;
			long y = 0;
			long x = 1;

			if (m == 1)
				return 0;

			while (a > 1)
			{
				// q is quotient
				long q = a / m;

				long t = m;

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

		/// <summary>
		/// Returns the product of the provided values.
		/// </summary>
		/// <param name="values">A list of values</param>
		/// <returns>The product</returns>
		public static int Product(params int[] values) => values.Aggregate(1, (a, b) => a * b);

		/// <summary>
		/// Returns the product of the provided values.
		/// </summary>
		/// <param name="values">A list of values</param>
		/// <returns>The product</returns>
		public static long Product(params long[] values) => values.Aggregate(1L, (a, b) => a * b);
	}
}
