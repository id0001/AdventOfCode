namespace AdventOfCode.Lib
{
    public static class CubeExtensions
    {
        public static bool Contains(this Cube source, Point3 p)
            => p.X >= source.Left && p.X < source.Right && p.Y >= source.Top && p.Y < source.Bottom && p.Z >= source.Front && p.Z < source.Back;

        public static bool IntersectsWith(this Cube source, Cube other) => Cube.Intersects(source, other);
    }
}