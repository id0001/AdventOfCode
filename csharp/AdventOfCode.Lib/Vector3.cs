namespace AdventOfCode.Lib;

public readonly record struct Vector3(double X, double Y, double Z)
{
    public static readonly Vector3 Zero = new();

    public double Length => System.Math.Sqrt(X * X + Y * Y + Z * Z);

    public bool Equals(Vector3 other) =>
        X.MarginalEquals(other.X) && Y.MarginalEquals(other.Y) && Z.MarginalEquals(other.Z);

    public Vector2 ToVector2() => new(X, Y);

    public override string ToString() => $"({X},{Y}, {Z})";

    public static Vector3 Cross(Vector3 v1, Vector3 v2) => new(
        v1.Y * v2.Z - v1.Z * v2.Y,
        v1.Z * v2.X - v1.X * v2.Z,
        v1.X * v2.Y - v1.Y * v2.X
    );

    public static double Dot(Vector3 v1, Vector3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

    public static Vector3 Subtract(Vector3 left, Vector3 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static Vector3 Add(Vector3 left, Vector3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Vector3 Multiply(Vector3 left, double scalar) =>
        new(left.X * scalar, left.Y * scalar, left.Z * scalar);

    public static Vector3 operator +(Vector3 left, Vector3 right) => Add(left, right);

    public static Vector3 operator -(Vector3 left, Vector3 right) => Subtract(left, right);

    public static Vector3 operator *(Vector3 vector, double scalar) => Multiply(vector, scalar);

    public static Vector3 operator *(double scalar, Vector3 vector) => Multiply(vector, scalar);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}