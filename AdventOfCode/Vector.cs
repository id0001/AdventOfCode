using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode
{
	[DebuggerDisplay("({X},{Y})")]
	public struct Vector : IEquatable<Vector>
	{
		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double X;

		public double Y;

		public static Vector Zero => new Vector(0d, 0d);

		public double Length() => Math.Sqrt((X * X) + (Y * Y));

		public Vector Normalize()
		{
			double val = 1d / Length();
			return new Vector(X * val, Y * val);
		}

		public static double Distance(Vector a, Vector b)
		{
			double v1 = a.X - b.X;
			double v2 = a.Y - b.Y;

			return Math.Sqrt((v1 * v1) + (v2 * v2));
		}

		public static double Dot(Vector a, Vector b)
		{
			return (a.X * b.X) + (a.Y * b.Y);
		}

		#region Overrides

		public override bool Equals(object obj)
		{
			return obj is Vector vector && Equals(vector);
		}

		public bool Equals([AllowNull] Vector other)
		{
			return X == other.X &&
				   Y == other.Y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		#endregion

		#region Operators

		public static Vector Subtract(Vector left, Vector right)
		{
			return new Vector(left.X - right.X, left.Y - right.Y);
		}

		public static bool operator ==(Vector left, Vector right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector left, Vector right)
		{
			return !(left == right);
		}

		public static Vector operator -(Vector value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static Vector operator +(Vector left, Vector right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		public static Vector operator -(Vector left, Vector right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		public static Vector operator *(Vector left, Vector right)
		{
			left.X *= right.X;
			left.Y *= right.Y;
			return left;
		}

		public static Vector operator *(Vector value, double scaleValue)
		{
			value.X *= scaleValue;
			value.Y *= scaleValue;
			return value;
		}

		public static Vector operator *(double scaleValue, Vector value)
		{
			value.X *= scaleValue;
			value.Y *= scaleValue;
			return value;
		}

		public static Vector operator /(Vector left, Vector right)
		{
			left.X /= right.X;
			left.Y /= right.Y;
			return left;
		}

		public static Vector operator /(Vector value, double scaleValue)
		{
			value.X /= scaleValue;
			value.Y /= scaleValue;
			return value;
		}

		public static Vector operator /(double scaleValue, Vector value)
		{
			value.X /= scaleValue;
			value.Y /= scaleValue;
			return value;
		}

		#endregion

		public override string ToString()
		{
			return $"({X},{Y})";
		}
	}
}