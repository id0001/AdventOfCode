namespace AdventOfCode.Lib;

public readonly struct Point3 : IPoint<Point3>, IEquatable<Point3>
{
    private static Point3 EmptyPoint => new Point3();
    
    public Point3(int x, int y, int z) => (X, Y, Z) = (x, y, z);
    
    public int X { get; }

    public int Y { get; }

    public int Z { get; }

    public int this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new NotSupportedException()
    };

    public int Dimensions => 3;

    public static Point3 Empty => EmptyPoint;
    
    public IEnumerable<Point3> GetNeighbors()
    {
        throw new NotImplementedException();
    }

    public bool Equals(Point3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Point3 left, Point3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point3 left, Point3 right)
    {
        return !(left == right);
    }
}