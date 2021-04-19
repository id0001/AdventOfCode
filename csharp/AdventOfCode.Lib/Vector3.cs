using System;
using System.Diagnostics;

namespace AdventOfCode.Lib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Vector3 : IEquatable<Vector3>
	{
		private static readonly Vector3 zeroVector = new Vector3(0, 0, 0);
		private static readonly Vector3 rightVector = new Vector3(1, 0, 0);
		private static readonly Vector3 leftVector = new Vector3(-1, 0, 0);
		private static readonly Vector3 upVector = new Vector3(0, -1, 0);
		private static readonly Vector3 downVector = new Vector3(0, 1, 0);
		private static readonly Vector3 forwardVector = new Vector3(0, 0, 1);
		private static readonly Vector3 backVector = new Vector3(0, 0, -1);

		public double X;
		public double Y;
		public double Z;

		public Vector3(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3(double v)
		{
			X = v;
			Y = v;
			Z = v;
		}

		internal string DebugDisplayString => $"{X}, {Y}, {Z}";

		public static Vector3 Zero => zeroVector;

		public static Vector3 Right => rightVector;

		public static Vector3 Left => leftVector;

		public static Vector3 Up => upVector;

		public static Vector3 Down => downVector;

		public static Vector3 Forward => forwardVector;

		public static Vector3 Back => backVector;

		public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

		#region Operators

		public static Vector3 operator +(Vector3 value1, Vector3 value2) => new Vector3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);

		public static Vector3 operator -(Vector3 value1, Vector3 value2) => new Vector3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);

		public static Vector3 operator *(Vector3 value1, Vector3 value2) => new Vector3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);

		public static Vector3 operator *(Vector3 value, int multiplier) => new Vector3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);

		public static Vector3 operator /(Vector3 value, double divisor) => new Vector3(value.X / divisor, value.Y / divisor, value.Z / divisor);

		public static Vector3 operator /(Vector3 value1, Vector3 value2) => new Vector3(value1.X / value2.X, value1.Y / value2.Y, value1.Z / value2.Z);

		public static bool operator ==(Vector3 a, Vector3 b) => a.Equals(b);

		public static bool operator !=(Vector3 a, Vector3 b) => !a.Equals(b);

		#endregion

		#region Static vector methods

		public static double Distance(Vector3 a, Vector3 b)
		{
			double dy = b.Y - a.Y;
			double dx = b.X - a.X;
			double dz = b.Z - a.Z;
			return Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
		}

		#endregion

		#region Overrides 

		public override bool Equals(object obj) => (obj is Vector3) && Equals((Vector3)obj);

		public bool Equals(Vector3 other) => (X == other.X) && (Y == other.Y) && (Z == other.Z);

		public override int GetHashCode() => HashCode.Combine(X, Y, Z);

		public override string ToString() => "{X: " + X + " Y: " + Y + " Z: " + Z + "}";

		#endregion

		public void Deconstruct(out double x, out double y, out double z) => (x, y, z) = (X, Y, Z);

		public double Distance(Vector3 other) => Distance(this, other);

		public Vector3 Normalize() => this / Length;
	}
}
