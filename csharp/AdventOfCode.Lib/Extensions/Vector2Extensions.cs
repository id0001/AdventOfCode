namespace AdventOfCode.Lib
{
    public static class Vector2Extensions
    {
        public static double GetAngleOnCircle(this Vector2 source) => GetAngleOnCircle(source, Vector2.Zero);

        public static double GetAngleOnCircle(this Vector2 source, Vector2 pivot)
        {
            var angle = System.Math.Atan2(source.Y - pivot.Y, source.X - pivot.X);
            if (angle < 0)
                angle += System.Math.PI * 2d;

            return angle;
        }

        public static Vector2 Normalize(this Vector2 source)
        {
            var length = source.Length;
            if (length == 0) return Vector2.Zero;

            return new(source.X / length, source.Y / length);
        }

        public static bool IsInSameDirection(this Vector2 source, Vector2 other)
        {
            var dot = Vector2.Dot(source, other);
            var theta = dot / (source.Length * other.Length);
            return System.Math.Abs(theta - 1).IsSimilarTo(0);
        }
    }
}
