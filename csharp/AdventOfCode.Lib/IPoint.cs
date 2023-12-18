using System.Numerics;
namespace AdventOfCode.Lib;

public interface IPoint<T> : IEquatable<IPoint<T>> where T : IBinaryInteger<T>, IMinMaxValue<T>
{
    T this[int index] { get; }

    int Dimensions { get; }

    IEnumerable<IPoint<T>> GetNeighbors(bool includeDiagonal = false);
}