using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib;

public readonly struct Cube : IEquatable<Cube>
{
    public Cube(int x, int y, int z, int width, int height, int depth) =>
        (Position, Size) = (new Point3(x, y, z), new Point3(width, height, depth));

    public Cube(int width, int height, int depth) => Size = new Point3(width, height, depth);

    public Point3 Position { get; } = Point3.Empty;

    public Point3 Size { get; } = Point3.Empty;

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

    public bool IsEmpty => Size == Point3.Empty;

    public int Volume => Size.X * Size.Y * Size.Z;

    public long LongVolume => (long)Size.X * (long)Size.Y * (long)Size.Z;

    public int SmallestArea => MathEx.Min(AreaFrontBack, AreaLeftRight, AreaTopBottom);

    public int SmallestPerimeter
    {
        get
        {
            var ordered = new[] { Size.X, Size.Y, Size.Z }.OrderBy(_ => _).ToArray();
            return ordered[0] + ordered[0] + ordered[1] + ordered[1];
        }
    }

    public int TotalSurfaceArea => AreaFrontBack * 2 + AreaLeftRight * 2 + AreaTopBottom * 2;

    public override string ToString() => $"({Position}, {Size})";

    public bool Equals(Cube other) => Position.Equals(other.Position) && Size.Equals(other.Size);

    public override bool Equals(object? obj) => obj is Cube other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Position, Size);

    public static bool operator ==(Cube left, Cube right) => left.Equals(right);

    public static bool operator !=(Cube left, Cube right) => !(left == right);
}