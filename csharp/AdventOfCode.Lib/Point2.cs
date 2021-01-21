using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace AdventOfCode.Lib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Point2 : IEquatable<Point2>, IPoint
	{
		private static readonly Point2 zeroPoint = new Point2();

		public int X;
		public int Y;

		public Point2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point2(int value)
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

		public static Point2 Zero => zeroPoint;

		public int Dimensions => 2;

		public static Point2 operator +(Point2 value1, Point2 value2) => new Point2(value1.X + value2.X, value1.Y + value2.Y);

		public static Point2 operator -(Point2 value1, Point2 value2) => new Point2(value1.X - value2.X, value1.Y - value2.Y);

		public static Point2 operator *(Point2 value1, Point2 value2) => new Point2(value1.X * value2.X, value1.Y * value2.Y);

		public static Point2 operator *(Point2 value, int multiplier) => new Point2(value.X * multiplier, value.Y * multiplier);

		public static bool operator ==(Point2 a, Point2 b) => a.Equals(b);

		public static bool operator !=(Point2 a, Point2 b) => !a.Equals(b);

		public static double Distance(Point2 a, Point2 b)
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
		public static Point2 Turn(Point2 point, Point2 pivot, double angle)
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			(int dx, int dy) = point - pivot;

			int x = pivot.X + (int)Math.Round((cos * dx) - (sin * dy));
			int y = pivot.Y + (int)Math.Round((sin * dx) + (cos * dy));
			return new Point2(x, y);
		}

		public override bool Equals(object obj) => (obj is Point2) && Equals((Point2)obj);

		public bool Equals(Point2 other) => (X == other.X) && (Y == other.Y);

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public override string ToString() => "{X: " + X + " Y: " + Y + "}";

		public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

		public double Distance(Point2 other) => Distance(this, other);

		public int GetValue(int dimension)
		{
			return dimension switch
			{
				0 => X,
				1 => Y,
				_ => throw new IndexOutOfRangeException()
			};
		}

		public IEnumerable<IPoint> GetNeighbors()
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (x == 0 && y == 0)
						continue;

					yield return new Point2(X + x, Y + y);
				}
			}
		}
	}
}
