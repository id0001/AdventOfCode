namespace AdventOfCode.Lib;

public interface IPoint
{
    int this[int index] { get; }

    int Dimensions { get; }
}

public interface IPoint<out T> : IPoint
    where T : struct, IPoint<T>
{
    IEnumerable<T> GetNeighbors();
}