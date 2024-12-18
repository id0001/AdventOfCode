namespace AdventOfCode.Lib
{
    public readonly record struct Pose2(Point2 Position, Point2 Face)
    {
        public Point2 Ahead => Position + Face;

        public Point2 Behind => Position - Face;

        public Point2 Left => Position + new Point2(Face.Y, -Face.X);

        public Point2 Right => Position + new Point2(-Face.Y, Face.X);

        public Pose2 Step(int amount = 1) => this with { Position = Position + amount * Face };

        public Pose2 TurnRight() => this with { Face = new Point2(-Face.Y, Face.X) };

        public Pose2 TurnLeft() => this with { Face = new Point2(Face.Y, -Face.X) };

        public Pose2 Turn(double angle)
        {
            var sin = System.Math.Sin(angle);
            var cos = System.Math.Cos(angle);
            var x = (int)System.Math.Round(cos * Face.X - sin * Face.Y);
            var y = (int)System.Math.Round(sin * Face.X + cos * Face.Y);
            return this with { Face = new Point2(x, y) };
        }
    }
}
