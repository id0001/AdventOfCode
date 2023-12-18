using System.Numerics;
namespace AdventOfCode.Lib;

public interface IPoint<TNumber> : IEquatable<IPoint<TNumber>> where TNumber : IBinaryInteger<TNumber>
{
    TNumber this[int index] { get; }

    int Dimensions { get; }

    IEnumerable<IPoint<TNumber>> GetNeighbors(bool includeDiagonal = false);
}