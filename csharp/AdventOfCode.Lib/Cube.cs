namespace AdventOfCode.Lib;

public readonly struct Cube : IEquatable<Cube>
{
    public Cube(int x, int y, int z, int width, int height, int depth) =>
        (Position, Size) = (new Point3(x, y, z), new Point3(width, height, depth));

    public Cube(int width, int height, int depth) => Size = new Point3(width, height, depth);

    public Point3 Position { get; } = Point3.Zero;

    public Point3 Size { get; } = Point3.Zero;

    public static Cube Empty { get; } = new();

    public int Left => Position.X;

    public int Top => Position.Y;

    public int Front => Position.Z;

    public int Right => Position.X + Size.X;

    public int Bottom => Position.Y + Size.Y;

    public int Back => Position.Z + Size.Z;

    public int AreaFrontBack => Size.X * Size.Y;

    public int AreaLeftRight => Size.Y * Size.Z;

    public int AreaTopBottom => Size.X * Size.Z;

    public bool IsEmpty => Size == Point3.Zero;

    public int Volume => Size.X * Size.Y * Size.Z;

    public long LongVolume => (long)Size.X * Size.Y * Size.Z;

    public int SmallestArea => new[] { AreaFrontBack, AreaLeftRight, AreaTopBottom }.Min();

    public int SmallestPerimeter
    {
        get
        {
            var ordered = new[] { Size.X, Size.Y, Size.Z }.OrderBy(x => x).ToArray();
            return ordered[0] + ordered[0] + ordered[1] + ordered[1];
        }
    }

    public int TotalSurfaceArea => AreaFrontBack * 2 + AreaLeftRight * 2 + AreaTopBottom * 2;

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

    public bool IntersectsWith(Cube other) => Intersects(this, other);

    public static bool Intersects(Cube a, Cube b)
    {
        return b.Left < a.Right
               && a.Left < b.Right
               && b.Top < a.Bottom
               && a.Top < b.Bottom
               && b.Front < a.Back
               && a.Front < b.Back;
    }

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

    public override string ToString() => $"({Position}, {Size})";

    public bool Equals(Cube other) => Position.Equals(other.Position) && Size.Equals(other.Size);

    public override bool Equals(object? obj) => obj is Cube other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Position, Size);

    public static bool operator ==(Cube left, Cube right) => left.Equals(right);

    public static bool operator !=(Cube left, Cube right) => !(left == right);
}