using System;

namespace AdventOfCodeLib
{
	public static class MathEx
	{
		public static double ToRadians(double degrees) => (Math.PI / 180d) * degrees;

		public static double ToDegrees(double radians) => (180d / Math.PI) * radians;

		public static int Mod(int x, int d) => (x % d + d) % d;
	}
}
