using System.Drawing;

namespace AdventOfCode.Lib;

public readonly struct Rectangle : IEquatable<Rectangle>
{
    public Rectangle(int x, int y, int width, int height) => (X, Y, Width, Height) = (x, y, width, height);

    public Rectangle(Point2 location, Point2 size) => (X, Y, Width, Height) = (location.X, location.Y, size.X, size.Y);

    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

    public int Left => X;

    public int Top => Y;

    public int Right => X + Width;

    public int Bottom => Y + Height;

    public static Rectangle Empty { get; } = new();

    public IEnumerable<Point2> AsGridPoints()
    {
        for (var y = Y; y < Y + Height; y++)
            for (var x = X; x < X + Width; x++)
                yield return new Point2(x, y);
    }

    public bool Equals(Rectangle other) =>
        X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is Rectangle other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    public bool Contains(Point2 p) => p.X >= Left && p.X < Right && p.Y >= Top && p.Y < Bottom;

    public bool IntersectsWith(Rectangle other) => Intersects(this, other);

    public static bool Intersects(Rectangle a, Rectangle b) => b.Left < a.Right && a.Left < b.Right && b.Top < a.Bottom && a.Top < b.Bottom;

    public static Rectangle Intersect(Rectangle a, Rectangle b)
    {
        if (!a.IntersectsWith(b))
            return Empty;

        var rightSide = System.Math.Min(a.Right, b.Right);
        var leftSide = System.Math.Max(a.Left, b.Left);
        var bottomSide = System.Math.Min(a.Bottom, b.Bottom);
        var topSide = System.Math.Max(a.Top, b.Top);

        return new Rectangle(leftSide, topSide, rightSide - leftSide, bottomSide - topSide);
    }

    public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

    public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);
}