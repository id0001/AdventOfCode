namespace AdventOfCode.Lib
{
    public readonly record struct Point3(int X, int Y, int Z) : IPoint<int>, INeighbors<int>
    {
        public static readonly Point3 Zero = new();

        public Point3 Left => new(X - 1, Y, Z);

        public Point3 Right => new(X + 1, Y, Z);

        public Point3 Up => new(X, Y - 1, Z);

        public Point3 Down => new(X, Y + 1, Z);

        public Point3 Backward => new(X, Y, Z - 1);

        public Point3 Forward => new(X, Y, Z + 1);

        #region Interface implementations

        int IPoint<int>.this[int index] => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            _ => throw new NotSupportedException()
        };

        int IPoint<int>.Dimensions => 3;

        IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal)
            => this.GetNeighbors(includeDiagonal).Cast<IPoint<int>>();

        #endregion

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"{X},{Y},{Z}".ToString();

        public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

        public static int ManhattanDistance(Point3 p0, Point3 p1)
            => System.Math.Abs(p1.X - p0.X) + System.Math.Abs(p1.Y - p0.Y) + System.Math.Abs(p1.Z - p0.Z);

        #region Operators

        public static Point3 operator +(Point3 left, Point3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static Point3 operator -(Point3 left, Point3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public static Point3 operator *(Point3 left, int right) => new(left.X * right, left.Y * right, left.Z * right);

        public static Point3 operator *(int left, Point3 right) => new(left * right.X, left * right.Y, left * right.Z);

        #endregion
    }
}
