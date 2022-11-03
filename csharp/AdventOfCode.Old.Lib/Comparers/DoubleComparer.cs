using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Lib.Comparers
{
	public class DoubleComparer : IEqualityComparer<double>
	{
		private readonly int precision;

		public DoubleComparer(int precision = 6)
		{
			this.precision = precision;
		}

		public bool Equals(double x, double y) => Math.Abs(x - y) < (1d / Math.Pow(10d, precision));

		public int GetHashCode([DisallowNull] double obj) => obj.GetHashCode();
	}
}
