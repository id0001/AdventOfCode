using System;
using System.Diagnostics;


namespace AdventOfCodeLib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Point : IEquatable<Point>
	{
		private static readonly Point zeroPoint = new Point();

		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point(int value)
		{
			X = value;
			Y = value;
		}

		internal string DebugDisplayString
		{
			get
			{
				return $"{X}, {Y}";
			}
		}

		public static Point Zero => zeroPoint;

		public static Point operator +(Point value1, Point value2) => new Point(value1.X + value2.X, value1.Y + value2.Y);

		public static Point operator -(Point value1, Point value2) => new Point(value1.X - value2.X, value1.Y - value2.Y);

		public static Point operator *(Point value1, Point value2) => new Point(value1.X * value2.X, value1.Y * value2.Y);

		public static Point operator /(Point source, Point divisor) => new Point(source.X / divisor.X, source.Y / divisor.Y);

		public static bool operator ==(Point a, Point b) => a.Equals(b);

		public static bool operator !=(Point a, Point b) => !a.Equals(b);

		public override bool Equals(object obj) => (obj is Point) && Equals((Point)obj);

		public bool Equals(Point other) => (X == other.X) && (Y == other.Y);

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public override string ToString() => "{x: " + X + " Y: " + Y + "}";

		public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
	}
}
