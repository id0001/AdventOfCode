using System;
using System.Diagnostics;

namespace AdventOfCodeLib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Vector2 : IEquatable<Vector2>
	{
		private static readonly Vector2 zeroVector = new Vector2(0, 0);

		public double X;
		public double Y;

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector2(double v)
		{
			X = v;
			Y = v;
		}

		internal string DebugDisplayString => $"{X}, {Y}";

		public static Vector2 Zero => zeroVector;

		#region Operators

		public static Vector2 operator +(Vector2 value1, Vector2 value2) => new Vector2(value1.X + value2.X, value1.Y + value2.Y);

		public static Vector2 operator -(Vector2 value1, Vector2 value2) => new Vector2(value1.X - value2.X, value1.Y - value2.Y);

		public static Vector2 operator *(Vector2 value1, Vector2 value2) => new Vector2(value1.X * value2.X, value1.Y * value2.Y);

		public static Vector2 operator *(Vector2 value, int multiplier) => new Vector2(value.X * multiplier, value.Y * multiplier);

		public static Vector2 operator /(Vector2 source, Vector2 divisor) => new Vector2(source.X / divisor.X, source.Y / divisor.Y);

		public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);

		public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);

		#endregion

		#region Static vector methods

		public static double Distance(Vector2 a, Vector2 b)
		{
			double dy = b.Y - a.Y;
			double dx = b.X - a.X;
			return Math.Sqrt((dx * dx) + (dy * dy));
		}

		/// <summary>
		/// Rotates a vector around a pivot on a circle by the amount defined by angle
		/// </summary>
		/// <param name="v">The vecttor to move</param>
		/// <param name="pivot">The pivot to rotate around</param>
		/// <param name="angle">The angle in radians to rotate by</param>
		/// <returns>The new rotated vector</returns>
		public static Vector2 MoveOnCircle(Vector2 v, Vector2 pivot, double angle)
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			(double dx, double dy) = v - pivot;

			double x = pivot.X + ((cos * dx) - (sin * dy));
			double y = pivot.Y + ((sin * dx) + (cos * dy));
			return new Vector2(x, y);
		}

		#endregion

		public override bool Equals(object obj) => (obj is Vector2) && Equals((Vector2)obj);

		public bool Equals(Vector2 other) => (X == other.X) && (Y == other.Y);

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public override string ToString() => "{X: " + X + " Y: " + Y + "}";

		public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

		public double Distance(Vector2 other) => Distance(this, other);
	}
}
