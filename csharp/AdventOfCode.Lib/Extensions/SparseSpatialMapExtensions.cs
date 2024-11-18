using System.Numerics;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode.Lib;

public static class SparseSpatialMapExtensions
{
    public static TValue GetOrAdd<TKey, TNumber, TValue>(this SparseSpatialMap<TKey, TNumber, TValue> source, TKey key,
        TValue newValue)
        where TKey : IPoint<TNumber>, new()
        where TNumber : IBinaryInteger<TNumber>, IMinMaxValue<TNumber>
    {
        if (!source.ContainsKey(key))
            source.Set(key, newValue);

        return source[key]!;
    }
}