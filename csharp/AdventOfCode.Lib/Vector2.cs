using System;
using System.Diagnostics;

namespace AdventOfCode.Lib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Vector2 : IEquatable<Vector2>
	{
		private static readonly Vector2 zeroVector = new Vector2(0, 0);
		private static readonly Vector2 rightVector = new Vector2(1, 0);
		private static readonly Vector2 leftVector = new Vector2(-1, 0);
		private static readonly Vector2 upVector = new Vector2(0, -1);
		private static readonly Vector2 downVector = new Vector2(0, 1);

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

		public static Vector2 Right => rightVector;

		public static Vector2 Left => leftVector;

		public static Vector2 Up => upVector;

		public static Vector2 Down => downVector;

		public double Length => Math.Sqrt(X * X + Y * Y);

		#region Operators

		public static Vector2 operator +(Vector2 value1, Vector2 value2) => new Vector2(value1.X + value2.X, value1.Y + value2.Y);

		public static Vector2 operator -(Vector2 value1, Vector2 value2) => new Vector2(value1.X - value2.X, value1.Y - value2.Y);

		public static Vector2 operator *(Vector2 value1, Vector2 value2) => new Vector2(value1.X * value2.X, value1.Y * value2.Y);

		public static Vector2 operator *(Vector2 value, int multiplier) => new Vector2(value.X * multiplier, value.Y * multiplier);

		public static Vector2 operator /(Vector2 value, double divisor) => new Vector2(value.X / divisor, value.Y / divisor);

		public static Vector2 operator /(Vector2 value1, Vector2 value2) => new Vector2(value1.X / value2.X, value1.Y / value2.Y);

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
		/// <param name="v">The vector to move</param>
		/// <param name="pivot">The pivot to rotate around</param>
		/// <param name="angle">The angle in radians to rotate by</param>
		/// <returns>The new rotated vector</returns>
		public static Vector2 Turn(Vector2 v, Vector2 pivot, double angle)
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			(double dx, double dy) = v - pivot;

			double x = pivot.X + ((cos * dx) - (sin * dy));
			double y = pivot.Y + ((sin * dx) + (cos * dy));
			return new Vector2(x, y);
		}

		public static double Dot(Vector2 a, Vector2 b) => (a.X * b.X + a.Y * b.Y);

		public static double Cross(Vector2 a, Vector2 b) => (a.X * b.Y) - (a.Y * b.X);

		/// <summary>
		/// Calculates the angle of the vector on a circle.
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="pivot">The center of the circle</param>
		/// <returns>The angle</returns>
		public static double AngleOnCircle(Vector2 v, Vector2 pivot)
		{
			double angle = Math.Atan2(v.Y - pivot.Y, v.X - pivot.X);
			if (angle < 0)
				angle += Math.PI * 2;

			return angle;
		}

		/// <summary>
		/// Calculates the angle between 2 vectors.
		/// </summary>
		/// <param name="a">Vector 1</param>
		/// <param name="b">Vector 2</param>
		/// <returns>Angle between vectors</returns>
		public static double Angle(Vector2 a, Vector2 b)
		{
			double dot = Dot(a, b);
			return Math.Acos(dot / (a.Length * b.Length));
		}

		#endregion

		#region Overrides 

		public override bool Equals(object obj) => (obj is Vector2) && Equals((Vector2)obj);

		public bool Equals(Vector2 other) => (X == other.X) && (Y == other.Y);

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public override string ToString() => "{X: " + X + " Y: " + Y + "}";

		#endregion

		public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

		public double Distance(Vector2 other) => Distance(this, other);

		public Vector2 Normalize() => this / Length;
	}
}
