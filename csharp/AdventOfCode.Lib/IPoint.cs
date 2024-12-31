using System.Numerics;

namespace AdventOfCode.Lib;

public interface IPoint<T> : IEquatable<IPoint<T>>
    where T : IBinaryInteger<T>
{
    T this[int index] { get; }
    int Dimensions { get; }

    bool IEquatable<IPoint<T>>.Equals(IPoint<T>? other)
    {
        if (other is null)
            return false;

        if (other.Dimensions != Dimensions)
            return false;

        for (var d = 0; d < Dimensions; d++)
            if (this[d] != other[d])
                return false;

        return true;
    }
}

public interface INeighbors<T>
    where T : IBinaryInteger<T>
{
    IEnumerable<IPoint<T>> GetNeighbors(bool includeDiagonal = false);
}