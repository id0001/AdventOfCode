namespace AdventOfCode.Lib
{
    public readonly record struct Line2(Vector2 Start, Vector2 End)
    {
        public double A { get; } = Start.Y - End.Y;

        public double B { get; } = End.X - Start.X;

        public double C { get; } = ((Start.X - End.X) * Start.Y) + ((End.Y - Start.Y) * Start.X);

        public bool IsVertical { get; } = Start.X == End.X;

        public bool IsHorizontal { get; } = Start.Y == End.Y;

        public double Slope { get; } = (End.Y - Start.Y) / (End.X - Start.X);

        public double Intercept { get; } = (Start.Y - (End.Y - Start.Y) / (End.X - Start.X) * Start.X);

        public static bool TryIntersect(Line2 line1, Line2 line2, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            var d = (line1.A * line2.B) - (line2.A * line1.B);
            if (d == 0)
                return false;

            var x = ((line1.B * line2.C) - (line2.B * line1.C)) / d;
            var y = ((line2.A * line1.C) - (line1.A * line2.C)) / d;
            intersection = new(x, y);
            return true;
        }
    }
}
