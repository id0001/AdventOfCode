namespace AdventOfCode.Lib;

public readonly record struct Rectangle(int X, int Y, int Width, int Height)
{
    public static readonly Rectangle Empty = new();

    public Rectangle(Point2 position, Point2 size) : this(position.X, position.Y, size.X, size.Y)
    {
    }

    public int Left => X;

    public int Top => Y;

    public int Right => X + Width; // Exclusive

    public int Bottom => Y + Height; // Exclusive

    public override string ToString() => $"[X: {X}, Y: {Y}, Width: {Width}, Height: {Height}]";

    public static bool Intersects(Rectangle a, Rectangle b) =>
        b.Left < a.Right && a.Left < b.Right && b.Top < a.Bottom && a.Top < b.Bottom;

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
}