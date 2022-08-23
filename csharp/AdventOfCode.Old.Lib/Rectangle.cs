using System;

namespace AdventOfCode.Lib
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int Left => X;

        public int Right => X + Width;

        public int Top => Y;

        public int Bottom => Y + Height;

        public Point2 Location
        {
            get => new Point2(X, Y);
            set => (X, Y) = value;
        }

        public Point2 Size
        {
            get => new Point2(Width, Height);
            set => (Width, Height) = value;
        }

        public bool Contains(Point2 point) => point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;

        public bool Equals(Rectangle other) => other.X == X && other.Y == Y && other.Width == Width && other.Height == Height;

        public override bool Equals(object obj) => (obj is Cube) && Equals((Cube)obj);

        public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        public override string ToString() => $"{{X: {X}, Y: {Y}, Width: {Width}, Height: {Height}}}";
    }
}
