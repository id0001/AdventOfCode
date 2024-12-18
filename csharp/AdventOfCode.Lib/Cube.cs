namespace AdventOfCode.Lib
{
    public readonly record struct Cube(int X, int Y, int Z, int Width, int Height, int Depth)
    {
        public static readonly Cube Empty = new();

        public Cube(Point3 Position, Point3 Size)
        : this(Position.X, Position.Y, Position.Z, Size.X, Size.Y, Size.Z)
        {
        }

        public Cube(int width, int height, int depth)
            : this(0, 0, 0, width, height, depth)
        {
        }

        public Point3 Position => new(X, Y, Z);

        public Point3 Size => new(Width, Height, Depth);

        public int Left => X;

        public int Top => Y;

        public int Front => Z;

        public int Right => X + Width; // Exclusive

        public int Bottom => Y + Height; // Exclusive

        public int Back => Z + Depth; // Exclusive

        public int AreaFrontBack => Width * Height;

        public int AreaLeftRight => Height * Depth;

        public int AreaTopBottom => Width * Depth;

        public bool IsEmpty => Width == 0 && Height == 0 && Depth == 0;

        public int Volume => Width * Height * Depth;

        public long LongVolume => (long)Width * Height * Depth;

        public int SmallestArea => new[] { AreaFrontBack, AreaLeftRight, AreaTopBottom }.Min();

        public int SmallestPerimeter
        {
            get
            {
                var ordered = new[] { Width, Height, Depth }.OrderBy(x => x).ToArray();
                return ordered[0] + ordered[0] + ordered[1] + ordered[1];
            }
        }

        public int TotalSurfaceArea => AreaFrontBack * 2 + AreaLeftRight * 2 + AreaTopBottom * 2;

        public IEnumerable<Point3> Vertices
        {
            get
            {
                yield return new(X, Y, Z);
                yield return new(X + Width, Y, Z);
                yield return new(X + Width, Y + Height, Z);
                yield return new(X, Y + Height, Z);
                yield return new(X, Y, Z + Depth);
                yield return new(X + Width, Y, Z + Depth);
                yield return new(X + Width, Y + Height, Z + Depth);
                yield return new(X, Y + Height, Z + Depth);
            }
        }

        public IEnumerable<Point3> Points
        {
            get
            {
                for (var z = Front; z < Back; z++)
                    for (var y = Top; y < Bottom; y++)
                        for (var x = Left; x < Right; x++)
                            yield return new Point3(x, y, z);
            }
        }

        public static bool Intersects(Cube a, Cube b)
            => b.Left < a.Right
            && a.Left < b.Right
            && b.Top < a.Bottom
            && a.Top < b.Bottom
            && b.Front < a.Back
            && a.Front < b.Back;

        public static Cube Intersect(Cube a, Cube b)
        {
            if (!a.IntersectsWith(b))
                return Empty;

            var rightSide = System.Math.Min(a.Right, b.Right);
            var leftSide = System.Math.Max(a.Left, b.Left);
            var bottomSide = System.Math.Min(a.Bottom, b.Bottom);
            var topSide = System.Math.Max(a.Top, b.Top);
            var backSide = System.Math.Min(a.Back, b.Back);
            var frontSide = System.Math.Max(a.Front, b.Front);
            return new Cube(leftSide, topSide, frontSide, rightSide - leftSide, bottomSide - topSide, backSide - frontSide);
        }

        public override string ToString() => $"[X: {X}, Y: {Y}, Z: {Z}, Width: {Width}, Height: {Height}, Depth: {Depth}";
    }
}
