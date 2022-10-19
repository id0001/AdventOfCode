using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib;

public static class EnumerableExtensions
{
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source) =>
        Combinatorics.SelectAllPermutations(source);
    
    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, int k) =>
        Combinatorics.SelectAllCombinations(source, k);

    public static IEnumerable<(T Current,T Next)> CurrentAndNext<T>(this IEnumerable<T> source, bool wrapAround = false)
    {
        using var e = source.GetEnumerator();
        if (!e.MoveNext())
            yield break;

        var previous = e.Current;
        var first = previous;
        while (e.MoveNext())
        {
            yield return (previous, e.Current);
            previous = e.Current;
        }

        if (wrapAround)
        {
            yield return (previous, first);
        }
    }

    public static int Product(this IEnumerable<int> source) => source.Aggregate(1, (a, b) => a * b);
    
    public static long Product(this IEnumerable<long> source) => source.Aggregate(1L, (a, b) => a * b);
    
    public static double Product(this IEnumerable<double> source) => source.Aggregate(1d, (a, b) => a * b);
}