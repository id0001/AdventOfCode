namespace AdventOfCode.Lib;

public readonly struct Vector2 : IEquatable<Vector2>
{
    public Vector2(double x, double y) => (X, Y) = (x, y);

    public double X { get; init; }

    public double Y { get; init; }
    
    public static Vector2 Zero { get; } = new();
    
    /// <summary>
    /// Calculates the angle of the vector on a circle.
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

    public bool Equals(Vector2 other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj) => obj is Vector2 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    public override string ToString() => $"({X},{Y})";
    
    public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
    
    public static Vector2 Subtract(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    public static Vector2 Add(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);
    
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

    public static Vector2 operator +(Vector2 left, Vector2 right) => Add(left, right);
    
    public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(left, right);
}