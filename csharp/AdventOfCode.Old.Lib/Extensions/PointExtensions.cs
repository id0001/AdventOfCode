namespace AdventOfCode.Lib.Extensions
{
    public static class PointExtensions
    {
        public static Point2 ToPoint2(this Point3 p) => new Point2(p.X, p.Y);
    }
}
