using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Point class TODO: Describe class here
	/// </summary>
	[DebuggerDisplay("({X},{Y}) = {ManhattanDistance}")]
	internal struct Point : IEquatable<Point>
	{
		public static readonly Point Zero = new Point(0, 0);

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

		public override bool Equals(object obj)
		{
			return obj is Point point && Equals(point);
		}

		public bool Equals([AllowNull] Point other)
		{
			return X == other.X &&
				   Y == other.Y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}
	}
}
