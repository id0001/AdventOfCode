using Microsoft;

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T[]> CombinationsWithRepetition<T>(this T[] source, int k)
        => CombinationsWithRepetitionReq(source, [], k);

    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, int k)
    {
        var list = source.ToList();
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        if (list.Count == 0)
            yield break;

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

    private static IEnumerable<T[]> CombinationsWithRepetitionReq<T>(this T[] source, T[] result, int k)
    {
        if (k == 0)
        {
            yield return result;
            yield break;
        }

        foreach (var item in source)
        foreach (var combination in CombinationsWithRepetitionReq(source, [..result, item], k - 1))
            yield return combination;
    }
}