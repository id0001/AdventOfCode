namespace AdventOfCode.Lib;

public readonly record struct Vector2(double X, double Y)
{
    public static readonly Vector2 Zero = new();

    /// <summary>
    ///     Calculates the angle of the vector on a circle.
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="pivot">The center of the circle</param>
    /// <returns>The angle</returns>
    public static double AngleOnCircle(Vector2 v, Vector2 pivot)
    {
        var angle = System.Math.Atan2(v.Y - pivot.Y, v.X - pivot.X);
        if (angle < 0)
            angle += System.Math.PI * 2d;

        return angle;
    }

    public override string ToString() => $"({X},{Y})";

    public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

    public static Vector2 Subtract(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    public static Vector2 Add(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

    public static Vector2 operator +(Vector2 left, Vector2 right) => Add(left, right);

    public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(left, right);
}