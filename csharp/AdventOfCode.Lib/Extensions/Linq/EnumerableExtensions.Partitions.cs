using AdventOfCode.Lib.Math;
using Microsoft;

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
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