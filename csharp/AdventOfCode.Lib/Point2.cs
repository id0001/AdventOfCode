namespace AdventOfCode.Lib
{
    public readonly record struct Point2(int X, int Y) : IPoint<int>, INeighbors<int>
    {
        public static readonly Point2 Zero = new();

        public Point2 Left => new(X - 1, Y);

        public Point2 Right => new(X + 1, Y);

        public Point2 Up => new(X, Y - 1);

        public Point2 Down => new(X, Y + 1);

        #region Interface implementations

        int IPoint<int>.this[int index] => index switch
        {
            0 => X,
            1 => Y,
            _ => throw new NotSupportedException()
        };

        int IPoint<int>.Dimensions => 2;

        IEnumerable<IPoint<int>> INeighbors<int>.GetNeighbors(bool includeDiagonal)
            => this.GetNeighbors(includeDiagonal).Cast<IPoint<int>>();

        #endregion

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"{X},{Y}".ToString();

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public static IEnumerable<Point2> BressenhamLine(Point2 from, Point2 to)
        {
            if (System.Math.Abs(to.Y - from.Y) < System.Math.Abs(to.X - from.X))
            {
                if (from.X > to.X)
                    return BresenhamLineLow(to.X, to.Y, from.X, from.Y).Reverse();
                return BresenhamLineLow(from.X, from.Y, to.X, to.Y);
            }

            if (from.Y > to.Y)
                return BresenhamLineHigh(to.X, to.Y, from.X, from.Y).Reverse();
            return BresenhamLineHigh(from.X, from.Y, to.X, to.Y);
        }

        public static int ManhattanDistance(Point2 p0, Point2 p1) 
            => System.Math.Abs(p1.X - p0.X) + System.Math.Abs(p1.Y - p0.Y);

        #region Operators

        public static Point2 operator +(Point2 left, Point2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Point2 operator -(Point2 left, Point2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Point2 operator *(Point2 left, int right) => new(left.X * right, left.Y * right);

        public static Point2 operator *(int left, Point2 right) => new(left * right.X, left * right.Y);

        #endregion

        private static IEnumerable<Point2> BresenhamLineHigh(int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            var d = 2 * dx - dy;
            var x = x0;


            for (var y = y0; y <= y1; y++)
            {
                yield return new Point2(x, y);
                if (d > 0)
                {
                    x = x + xi;
                    d = d + 2 * (dx - dy);
                }
                else
                {
                    d = d + 2 * dx;
                }
            }
        }

        private static IEnumerable<Point2> BresenhamLineLow(int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            var d = 2 * dy - dx;
            var y = y0;

            for (var x = x0; x <= x1; x++)
            {
                yield return new Point2(x, y);
                if (d > 0)
                {
                    y = y + yi;
                    d = d + 2 * (dy - dx);
                }
                else
                {
                    d = d + 2 * dy;
                }
            }
        }
    }
}
