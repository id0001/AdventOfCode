using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib
{
    public readonly struct Cube : IEquatable<Cube>
    {
        private static readonly Cube EmptyCube = new Cube();

        public Point3 Position { get; } = Point3.Empty;
        
        public Point3 Size { get; } = Point3.Empty;

        public static Cube Empty => EmptyCube;

        public Cube(int x, int y, int z, int width, int height, int depth) =>
            (Position, Size) = (new Point3(x, y, z), new Point3(width, height, depth));

        public Cube(int width, int height, int depth) => Size = new Point3(width, height, depth);

        public int Left => Position.X;

        public int Top => Position.Y;

        public int Front => Position.Z;

        public int Right => Position.X + Size.X;

        public int Bottom => Position.Y + Size.Y;

        public int Back => Position.Z + Size.Z;

        public int AreaFrontBack => Size.X * Size.Y;

        public int AreaLeftRight => Size.Y * Size.Z;

        public int AreaTopBottom => Size.X * Size.Z;

        public bool IsEmpty => Size == Point3.Empty;

        public int Volume => Size.X * Size.Y * Size.Z;

        public long LongVolume => (long)Size.X * (long)Size.Y * (long)Size.Z;
        
        public int SmallestArea => MathEx.Min(AreaFrontBack, AreaLeftRight, AreaTopBottom);

        public int SmallestPerimeter
        {
            get
            {
                int[] ordered = new[] { Size.X, Size.Y, Size.Z }.OrderBy(_ => _).ToArray();
                return ordered[0] + ordered[0] + ordered[1] + ordered[1];
            }
        }
        
        public int TotalSurfaceArea => (AreaFrontBack * 2) + (AreaLeftRight * 2) + (AreaTopBottom * 2);

        // public int X;
        //
        // public int Y;
        //
        // public int Z;
        //
        // public int Width;
        //
        // public int Height;
        //
        // public int Depth;
        //
        // public Cube(int x, int y, int z, int width, int height, int depth)
        // {
        //     X = x;
        //     Y = y;
        //     Z = z;
        //     Width = width;
        //     Height = height;
        //     Depth = depth;
        // }
        //
        // public Cube(Point3 location, Point3 size)
        // {
        //     X = location.X;
        //     Y = location.Y;
        //     Z = location.Z;
        //     Width = size.X;
        //     Height = size.Y;
        //     Depth = size.Z;
        // }
        //
        // public static Cube Empty => EmptyCube;
        //
        // internal string DebugDisplayString => $"{X}, {Y}, {Z}, {Width}, {Height}, {Depth}";
        //
        // public int Left => X;
        //
        // public int Right => X + Width;
        //
        // public int Top => Y;
        //
        // public int Bottom => Y + Height;
        //
        // public int Front => Z;
        //
        // public int Back => Z + Depth;
        //
        // public bool IsEmpty => X == 0 && Y == 0 && Z == 0 && Width == 0 && Height == 0 && Depth == 0;
        //
        // public Point3 Location
        // {
        //     get => new Point3(X, Y, Z);
        //     set => (X, Y, Z) = value;
        // }
        //
        // public Point3 Size
        // {
        //     get => new Point3(Width, Height, Depth);
        //     set => (Width, Height, Depth) = value;
        // }
        //
        // public long Volume => (long)Width * Height * Depth;
        //
        // public IEnumerable<Point3> Points
        // {
        //     get
        //     {
        //         for (int z = Front; z < Back; z++)
        //         {
        //             for (int y = Top; y < Bottom; y++)
        //             {
        //                 for (int x = Left; x < Right; x++)
        //                 {
        //                     yield return new Point3(x, y, z);
        //                 }
        //             }
        //         }
        //     }
        // }
        //
        // public int AreaDepthWidth => Depth * Width;
        // public int AreaWidthHeight => Width * Height;
        // public int AreaDepthHeight => Depth * Height;
        //
        // public int SmallestArea => MathEx.Min(AreaDepthHeight, AreaWidthHeight, AreaDepthWidth);
        //
        // public int SmallestPerimeter
        // {
        //     get
        //     {
        //         var ordered = new[] { Depth, Width, Height }.OrderBy(_ => _).ToArray();
        //         return ordered[0] + ordered[0] + ordered[1] + ordered[1];
        //     }
        // }
        //
        // public bool IntersectsWith(Cube other) => Intersects(this, other);
        //
        // public bool Contains(Point3 point) => point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom && point.Z >= Front && point.Z < Back;
        //
        // public bool Contains(Cube other)
        // {
        //     return (other.Left >= Left && other.Right <= Right)
        //         && (other.Top >= Top && other.Bottom <= Bottom)
        //         && (other.Front >= Front && other.Back <= Back);
        // }
        //
        // public bool Equals(Cube other) => other.X == X && other.Y == Y && other.Z == Z && other.Width == Width && other.Height == Height && other.Depth == Depth;
        //
        // public override bool Equals(object obj) => (obj is Cube) && Equals((Cube)obj);
        //
        // public override int GetHashCode() => HashCode.Combine(X, Y, Z, Width, Height, Depth);
        //
        // public override string ToString() => $"{{X: {X}, Y: {Y}, Z: {Z}, Width: {Width}, Height: {Height}, Depth: {Depth}}}";
        //
        // public static bool Intersects(Cube a, Cube b)
        // {
        //     return b.Left < a.Right
        //         && a.Left < b.Right
        //         && b.Top < a.Bottom
        //         && a.Top < b.Bottom
        //         && b.Front < a.Back
        //         && a.Front < b.Back;
        // }
        //
        // public static Cube Intersect(Cube a, Cube b)
        // {
        //     if (!a.IntersectsWith(b))
        //         return Empty;
        //
        //     int rightSide = Math.Min(a.Right, b.Right);
        //     int leftSide = Math.Max(a.Left, b.Left);
        //     int bottomSide = Math.Min(a.Bottom, b.Bottom);
        //     int topSide = Math.Max(a.Top, b.Top);
        //     int backSide = Math.Min(a.Back, b.Back);
        //     int frontSide = Math.Max(a.Front, b.Front);
        //     return new Cube(leftSide, topSide, frontSide, rightSide - leftSide, bottomSide - topSide, backSide - frontSide);
        // }
        //
        // public static bool operator ==(Cube left, Cube right)
        // {
        //     return left.Equals(right);
        // }
        //
        // public static bool operator !=(Cube left, Cube right)
        // {
        //     return !(left == right);
        // }

        public override string ToString() => $"({Position}, {Size})";

        public bool Equals(Cube other)
        {
            return Position.Equals(other.Position) && Size.Equals(other.Size);
        }

        public override bool Equals(object? obj)
        {
            return obj is Cube other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Size);
        }

        public static bool operator ==(Cube left, Cube right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Cube left, Cube right)
        {
            return !(left == right);
        }
    }
}
