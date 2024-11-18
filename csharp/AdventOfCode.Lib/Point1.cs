namespace AdventOfCode.Lib;

public readonly record struct Point1(int X) : IPoint<int>
{
    public static readonly Point1 Zero = new();

    public Point1 Left => new(X - 1);
    public Point1 Right => new(X - 1);

    int IPoint<int>.this[int index] => index switch
    {
        0 => X,
        _ => throw new NotSupportedException()
    };

    int IPoint<int>.Dimensions => 1;

    public bool Equals(IPoint<int>? other)
    {
        if (other is null)
            return false;

        var instance = (IPoint<int>) this;

        if (other.Dimensions != instance.Dimensions)
            return false;

        for (var d = 0; d < instance.Dimensions; d++)
            if (instance[d] != other[d])
                return false;

        return true;
    }

    public IEnumerable<Point1> GetNeighbors()
    {
        yield return new Point1(X - 1);
        yield return new Point1(X + 1);
    }

    public void Deconstruct(out int x) => x = X;

    public override string ToString() => $"({X})";

    public static implicit operator Point1(int x) => new(x);
    public static implicit operator int(Point1 p) => p.X;
}