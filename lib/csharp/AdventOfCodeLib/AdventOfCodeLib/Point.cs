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

		public static Point operator *(Point value, int multiplier) => new Point(value.X * multiplier, value.Y * multiplier);

		public static Point operator /(Point source, Point divisor) => new Point(source.X / divisor.X, source.Y / divisor.Y);

		public static bool operator ==(Point a, Point b) => a.Equals(b);

		public static bool operator !=(Point a, Point b) => !a.Equals(b);

		public static double Distance(Point a, Point b)
		{
			int dy = b.Y - a.Y;
			int dx = b.X - a.X;
			return Math.Sqrt((dx * dx) + (dy * dy));
		}

		/// <summary>
		/// Rotates a point around a pivot on a circle by the amount defined by angle
		/// </summary>
		/// <param name="point">The point to move</param>
		/// <param name="pivot">The pivot to rotate around</param>
		/// <param name="angle">The angle in radians to rotate by</param>
		/// <returns>The new rotated point</returns>
		public static Point MoveOnCircle(Point point, Point pivot, double angle)
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			(int dx, int dy) = point - pivot;

			int x = pivot.X + (int)Math.Round((cos * dx) - (sin * dy));
			int y = pivot.Y + (int)Math.Round((sin * dx) + (cos * dy));
			return new Point(x, y);
		}

		public override bool Equals(object obj) => (obj is Point) && Equals((Point)obj);

		public bool Equals(Point other) => (X == other.X) && (Y == other.Y);

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public override string ToString() => "{X: " + X + " Y: " + Y + "}";

		public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

		public double Distance(Point other) => Distance(this, other);
	}
}
