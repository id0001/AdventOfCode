namespace AdventOfCode.Lib
{
    public readonly record struct Line2(Vector2 Intercept, Vector2 Slope)
    {
        public static bool TryIntersect(Line2 line1, Line2 line2, out Vector2 intersection)
        {
            intersection = new();

            if (Vector2.Cross(line1.Slope, line2.Slope) == 0)
                return false;

            var diff = line2.Intercept - line1.Intercept;
            var t = (diff.X * line2.Slope.Y - diff.Y * line2.Slope.X) / (line1.Slope.X * line2.Slope.Y - line1.Slope.Y * line2.Slope.X);

            intersection = line1.Intercept + (t * line1.Slope);
            return true;
        }
    }
}
