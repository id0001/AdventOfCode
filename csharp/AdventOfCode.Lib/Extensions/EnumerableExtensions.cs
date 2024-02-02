using AdventOfCode.Lib.Math;
using Microsoft;

namespace AdventOfCode.Lib;

public static class EnumerableExtensions
{
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();

        Requires.NotNullOrEmpty(list, nameof(source));

        var a = Enumerable.Range(0, list.Count).ToArray();

        while (true)
        {
            yield return a.Select(i => list[i]).ToArray();
            var j = list.Count - 1;

            while (j > 0 && a[j - 1] >= a[j])
                j--;

            if (j == 0)
                yield break;

            var l = list.Count;
            while (a[j - 1] >= a[l - 1])
                l--;

            (a[j - 1], a[l - 1]) = (a[l - 1], a[j - 1]);
            Array.Reverse(a, j, list.Count - j);
        }
    }

    /// <summary>
    ///     Returns all combinations of k elements where the order does not matter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, int k)
    {
        var list = source.ToList();
        Requires.NotNullOrEmpty(list, nameof(source));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        var a = new T[k];
        var c = Enumerable.Range(0, k + 2).ToArray();
        c[k] = list.Count;
        c[k + 1] = 0;

        var j = k;
        if (k > list.Count)
            yield break;

        if (k == list.Count)
            yield return c.Take(k).Select(i => list[i]).ToArray();
        else
            while (true)
            {
                for (var i = 0; i < k; i++)
                    a[i] = list[c[i]];

                yield return (T[]) a.Clone();

                int x;
                if (j > 0)
                {
                    x = j;
                }
                else
                {
                    if (c[0] + 1 < c[1])
                    {
                        c[0] += 1;
                        continue;
                    }

                    j = 1;
                    do
                    {
                        j++;
                        c[j - 2] = j - 2;
                        x = c[j - 1] + 1;
                    } while (x == c[j]);

                    if (j > k)
                        break;
                }

                c[j - 1] = x;
                j--;
            }
    }

    public static IEnumerable<T[][]> Partitions<T>(this IEnumerable<T> source, int k, int minPartitionSize = 0)
    {
        Requires.Argument(k > 0, nameof(k), "Argument must be greater than 0");

        if (k == 1)
        {
            yield return [source.ToArray()];
            yield break;
        }

        var list = source.ToList();
        var n = list.Count;

        foreach (var distribution in Combinatorics.GeneratePartitions(n, k, minPartitionSize))
        {
            var emptyPartition = new T[k].Select(_ => Array.Empty<T>()).ToArray();
            foreach (var partition in PartitionRec(list, emptyPartition, distribution, 0))
                yield return partition;
        }
    }

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

    private static IEnumerable<T[][]> PartitionRec<T>(IList<T> list, T[][] partitions, int[] distribution, int pi)
    {
        if (pi == partitions.Length)
        {
            yield return partitions;
            yield break;
        }

        foreach (var combination in list.Except(partitions.SelectMany(x => x)).Combinations(distribution[pi]))
        {
            var np = new T[partitions.Length][];
            for (var i = 0; i < partitions.Length; i++)
            {
                np[i] = new T[partitions[i].Length];
                Array.Copy(partitions[i], np[i], partitions[i].Length);
            }

            np[pi] = combination.ToArray();
            foreach (var partition in PartitionRec(list, np, distribution, pi + 1))
                yield return partition;
        }
    }
}