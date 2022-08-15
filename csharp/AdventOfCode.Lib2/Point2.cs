namespace AdventOfCode.Lib;

public readonly struct Point2 : IPoint<Point2>
{
    private static readonly Point2 EmptyPoint = new Point2();

    public Point2(int x, int y) => (X, Y) = (x, y);
    
    public int this[int index] => index switch
    {
        0 => X,
        1 => Y,
        _ => throw new NotSupportedException()
    };
    
    public int X { get; }

    public int Y { get; }
    
    public int Dimensions => 2;

    public static Point2 Empty => EmptyPoint;

    public IEnumerable<Point2> GetNeighbors()
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"({X},{Y})";
}