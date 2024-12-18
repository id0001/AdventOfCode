namespace AdventOfCode.Lib
{
    public readonly record struct Point4(int X, int Y, int Z, int W) : IPoint<int>, INeighbors<int>
    {
        public static readonly Point4 Zero = new();

        public Point4 Left => new(X - 1, Y, Z, W);

        public Point4 Right => new(X + 1, Y, Z, W);

        public Point4 Up => new(X, Y - 1, Z, W);

        public Point4 Down => new(X, Y + 1, Z, W);

        public Point4 Backward => new(X, Y, Z - 1, W);

        public Point4 Forward => new(X, Y, Z + 1, W);

        public Point4 Retreat => new(X, Y, Z, W - 1);

        public Point4 Advance => new(X, Y, Z, W + 1);

        #region Interface implementations

        int IPoint<int>.this[int index] => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            _ => throw new NotSupportedException()
        };

        int IPoint<int>.Dimensions => 4;

        IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal)
            => this.GetNeighbors(includeDiagonal).Cast<IPoint<int>>();

        #endregion

        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        public override string ToString() => $"{X},{Y},{Z},{W}".ToString();

        public void Deconstruct(out int x, out int y, out int z, out int w) => (x, y, z, w) = (X, Y, Z, W);

        public static int ManhattanDistance(Point4 p0, Point4 p1)
            => System.Math.Abs(p1.X - p0.X) + System.Math.Abs(p1.Y - p0.Y) + System.Math.Abs(p1.Z - p0.Z) + System.Math.Abs(p1.W - p0.W);

        #region Operators

        public static Point4 operator +(Point4 left, Point4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

        public static Point4 operator -(Point4 left, Point4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

        public static Point4 operator *(Point4 left, int right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);

        public static Point4 operator *(int left, Point4 right) => new(left * right.X, left * right.Y, left * right.Z, left * right.W);

        #endregion
    }
}
