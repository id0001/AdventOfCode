using System.Numerics;

namespace AdventOfCode.Lib.Math;

public static partial class Euclid
{
	public static int Modulus(int dividend, int divisor) => (dividend % divisor + divisor) % divisor;

	public static long Modulus(long dividend, long divisor) => (dividend % divisor + divisor) % divisor;

	public static BigInteger Modulus(BigInteger dividend, BigInteger divisor) =>
		(dividend % divisor + divisor) % divisor;
}