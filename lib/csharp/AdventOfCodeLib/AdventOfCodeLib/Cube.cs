using System;
using System.Diagnostics;

namespace AdventOfCodeLib
{
	[DebuggerDisplay("{DebugDisplayString, nq}")]
	public struct Cube : IEquatable<Cube>
	{
		private static readonly Cube emptyCube = new Cube();

		public int X;

		public int Y;

		public int Z;

		public int Width;

		public int Height;

		public int Depth;

		public Cube(int x, int y, int z, int width, int height, int depth)
		{
			X = x;
			Y = y;
			Z = z;
			Width = width;
			Height = height;
			Depth = depth;
		}

		public Cube(Point3 location, Point3 size)
		{
			X = location.X;
			Y = location.Y;
			Z = location.Z;
			Width = size.X;
			Height = size.Y;
			Depth = size.Z;
		}

		public static Cube Empty => emptyCube;

		internal string DebugDisplayString => $"{X}, {Y}, {Z}, {Width}, {Height}, {Depth}";

		public int Left => X;

		public int Right => X + Width;

		public int Top => Y;

		public int Bottom => Y + Height;

		public int Front => Z;

		public int Back => Z + Depth;

		public bool IsEmpty => X == 0 && Y == 0 && Z == 0 && Width == 0 && Height == 0 && Depth == 0;

		public Point3 Location
		{
			get => new Point3(X, Y, Z);
			set => (X, Y, Z) = value;
		}

		public Point3 Size
		{
			get => new Point3(Width, Height, Depth);
			set => (Width, Height, Depth) = value;
		}

		public bool Contains(Point3 point) => point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom && point.Z >= Front && point.Z < Back;

		public bool Equals(Cube other) => other.X == X && other.Y == Y && other.Z == Z && other.Width == Width && other.Height == Height && other.Depth == Depth;

		public override bool Equals(object obj) => (obj is Cube) && Equals((Cube)obj);

		public override int GetHashCode() => HashCode.Combine(X, Y, Z, Width, Height, Depth);

		public override string ToString() => $"{{X: {X}, Y: {Y}, Z: {Z}, Width: {Width}, Height: {Height}, Depth: {Depth}}}";
	}
}
