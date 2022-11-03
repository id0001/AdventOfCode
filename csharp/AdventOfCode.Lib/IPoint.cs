namespace AdventOfCode.Lib;

public interface IPoint : IEquatable<IPoint>
{
    int this[int index] { get; }

    int Dimensions { get; }

    IEnumerable<IPoint> GetNeighbors(bool includeDiagonal = false);
}