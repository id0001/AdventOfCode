namespace AdventOfCode.Lib;

public readonly struct Rectangle : IEquatable<Rectangle>
{
    public Rectangle(int x, int y, int width, int height) => (X, Y, Width, Height) = (x, y, width, height);

    public Rectangle(Point2 location, Point2 size) => (X, Y, Width, Height) = (location.X, location.Y, size.X, size.Y);

    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

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

    public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

    public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);
}