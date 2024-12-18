namespace AdventOfCode.Lib
{
    public readonly record struct Vector3(double X, double Y, double Z)
    {
        public static readonly Vector3 Zero = new();

        public double Length => System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        public double LengthSquared => (X * X) + (Y * Y) + (Z * Z);

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => new(
            v1.Y * v2.Z - v1.Z * v2.Y,
            v1.Z * v2.X - v1.X * v2.Z,
            v1.X * v2.Y - v1.Y * v2.X
        );

        public static double Dot(Vector3 v1, Vector3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        public bool Equals(Vector3 other) => X.IsSimilarTo(other.X) && Y.IsSimilarTo(other.Y) && Z.IsSimilarTo(other.Z);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"{X},{Y}, {Z}";


        #region Operators

        public static Vector3 operator +(Vector3 left, Vector3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static Vector3 operator -(Vector3 left, Vector3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public static Vector3 operator *(Vector3 vector, double scalar) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);

        public static Vector3 operator *(double scalar, Vector3 vector) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);

        #endregion
    }
}
