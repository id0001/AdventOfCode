using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.DataStructures
{
	[DebuggerDisplay("({X},{Y})")]
	internal struct Point : IEquatable<Point>
	{
		public static readonly Point Zero = new Point(0, 0);

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X;

		public int Y;

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

		public override string ToString() => $"({X},{Y})";

		public static bool operator ==(Point left, Point right) => left.Equals(right);

		public static bool operator !=(Point left, Point right) => !(left == right);

		public static Point operator +(Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);

		public static Point operator -(Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);

		public static int DistanceSquared(Point a, Point b)
		{
			int x = Math.Abs(a.X - b.X);
			int y = Math.Abs(a.Y - b.Y);

			return (x * x) + (y * y);
		}
	}
}
