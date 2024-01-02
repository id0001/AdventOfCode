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

    public static double Cross(Vector2 v1, Vector2 v2) => v1.X * v2.Y - v1.Y * v2.X;

    public static double Dot(Vector2 v1, Vector2 v2) => v1.X * v2.X + v1.Y * v2.Y;

    public static Vector2 Subtract(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    public static Vector2 Add(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

    public static Vector2 Multiply(Vector2 vector, double scalar) => new(vector.X * scalar, vector.Y * scalar);

    public static Vector2 operator +(Vector2 left, Vector2 right) => Add(left, right);

    public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(left, right);

    public static Vector2 operator *(Vector2 vector, double scalar) => Multiply(vector, scalar);

    public static Vector2 operator *(double scalar, Vector2 vector) => Multiply(vector, scalar);

    public bool Equals(Vector2 other) => X.MarginalEquals(other.X) && Y.MarginalEquals(other.Y);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}