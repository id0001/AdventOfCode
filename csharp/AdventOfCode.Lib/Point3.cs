using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode.Lib
{
    [DebuggerDisplay("{DebugDisplayString, nq}")]
    public struct Point3 : IEquatable<Point3>, IPoint
    {
        private static readonly Point3 zeroPoint = new Point3(0, 0, 0);

        public int X;
        public int Y;
        public int Z;

        public Point3(int value)
            : this(value, value, value)
        {
        }

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        internal string DebugDisplayString => $"{X}, {Y}, {Z}";

        public static Point3 Zero => zeroPoint;

        public int Dimensions => 3;

        public static Point3 operator +(Point3 value1, Point3 value2) => new Point3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);

        public static Point3 operator -(Point3 value1, Point3 value2) => new Point3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);

        public static Point3 operator *(Point3 value1, Point3 value2) => new Point3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);

        public static Point3 operator *(Point3 value, int multiplier) => new Point3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);

        public static bool operator ==(Point3 a, Point3 b) => a.Equals(b);

        public static bool operator !=(Point3 a, Point3 b) => !a.Equals(b);

        public bool Equals(Point3 other) => X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object obj) => (obj is Point3) && Equals((Point3)obj);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"{{X: {X}, Y: {Y}, Z: {Z}}}";

        public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

        public int GetValue(int dimension)
        {
            return dimension switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException()
            };
        }

        public IEnumerable<Point3> GetNeighbors()
        {
            for (int z = -1; z <= 1; z++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        yield return new Point3(X + x, Y + y, Z + z);
                    }
                }
            }
        }

        IEnumerable<IPoint> IPoint.GetNeighbors()
        {
            foreach (var neighbor in GetNeighbors())
                yield return neighbor;
        }
    }
}
