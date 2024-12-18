namespace AdventOfCode.Lib
{
    public readonly record struct Vector2(double X, double Y)
    {
        public static readonly Vector2 Zero = new();

        public double Length => System.Math.Sqrt((X * X) + (Y * Y));

        public double LengthSquared => (X * X) + (Y * Y);

        public static double Cross(Vector2 v1, Vector2 v2) => v1.X * v2.Y - v1.Y * v2.X;

        public static double Dot(Vector2 v1, Vector2 v2) => v1.X * v2.X + v1.Y * v2.Y;

        public bool Equals(Vector2 other) => X.IsSimilarTo(other.X) && Y.IsSimilarTo(other.Y);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"{X},{Y}";


        #region Operators

        public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator *(Vector2 vector, double scalar) => new(vector.X * scalar, vector.Y * scalar);

        public static Vector2 operator *(double scalar, Vector2 vector) => new(vector.X * scalar, vector.Y * scalar);

        #endregion
    }
}
