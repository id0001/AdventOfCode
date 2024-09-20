using System.Numerics;

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static TNumber Product<TNumber>(this IEnumerable<TNumber> source)
        where TNumber : IMultiplyOperators<TNumber, TNumber, TNumber>
        => source.Aggregate((a, b) => a * b);
}