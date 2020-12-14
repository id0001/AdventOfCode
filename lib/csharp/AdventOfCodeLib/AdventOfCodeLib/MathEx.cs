using System;

namespace AdventOfCodeLib
{
	public static class MathEx
	{
		public static double ToRadians(double degrees) => (Math.PI / 180d) * degrees;

		public static double ToDegrees(double radians) => (180d / Math.PI) * radians;

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

		public static int Product(params int[] values)
		{
			int p = values[0];
			for (int i = 1; i < values.Length; i++)
				p *= values[i];

			return p;
		}

		public static long Product(params long[] values)
		{
			long p = values[0];
			for (long i = 1; i < values.Length; i++)
				p *= values[i];

			return p;
		}
	}
}
