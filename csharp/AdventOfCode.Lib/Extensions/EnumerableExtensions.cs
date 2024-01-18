using System.Numerics;
using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib;

public static class EnumerableExtensions
{
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source) =>
        Combinatorics.SelectAllPermutations(source);

    /// <summary>
    ///     Returns all combinations of k elements where the order does not matter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, int k) =>
        Combinatorics.SelectAllCombinations(source, k);

    public static IEnumerable<(T Current, T Next)> CurrentAndNext<T>(this IEnumerable<T> source,
        bool wrapAround = false)
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

        if (wrapAround) yield return (previous, first);
    }

    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, IEnumerable<T> other) =>
        source.SelectMany(_ => other, (a, b) => new[] {a, b});

    public static IEnumerable<TResult> Combinations<T1, T2, TResult>(this IEnumerable<T1> source, IEnumerable<T2> other,
        Func<T1, T2, TResult> selector) => source.SelectMany(_ => other, selector);

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, T separator)
    {
        var list = new List<T>();
        foreach (var item in source)
            if (!EqualityComparer<T>.Default.Equals(item, separator))
            {
                list.Add(item);
            }
            else
            {
                yield return list;
                list = new List<T>();
            }
    }

    public static IEnumerable<IList<T>> Windowed<T>(this IEnumerable<T> source, int windowSize)
    {
        var windows = Enumerable.Range(0, windowSize)
            .Select(_ => new List<T>())
            .ToList();

        var i = 0;
        using var iter = source.GetEnumerator();
        while (iter.MoveNext())
        {
            var c = System.Math.Min(i + 1, windowSize);
            for (var j = 0; j < c; j++)
                windows[(i - j) % windowSize].Add(iter.Current);

            if (i >= windowSize - 1)
            {
                var previous = (i + 1) % windowSize;
                yield return windows[previous];
                windows[previous] = new List<T>();
            }

            i++;
        }
    }

    public static int Product(this IEnumerable<int> source) => source.Aggregate(1, (a, b) => a * b);

    public static long Product(this IEnumerable<long> source) => source.Aggregate(1L, (a, b) => a * b);

    public static double Product(this IEnumerable<double> source) => source.Aggregate(1d, (a, b) => a * b);

    public static ulong Product(this IEnumerable<ulong> source) => source.Aggregate(1UL, (a, b) => a * b);

    public static BigInteger Product(this IEnumerable<BigInteger> source) =>
        source.Aggregate(BigInteger.One, (a, b) => a * b);

    public static long Product<T>(this IEnumerable<T> source, Func<T, long> selector) =>
        source.Aggregate(1L, (a, b) => a * selector(b));

    public static ulong Sum<T>(this IEnumerable<T> source, Func<T, ulong> selector) =>
        source.Aggregate<T, ulong>(0, (current, item) => current + selector(item));
}