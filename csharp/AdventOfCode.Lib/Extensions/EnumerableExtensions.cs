using System.Diagnostics.CodeAnalysis;
using Microsoft;

namespace AdventOfCode.Lib;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> QuickPerm<T>(this IEnumerable<T> set)
    {
        var list = set.ToList();
        var n = list.Count;
        var a = new int[n];
        var p = new int[n];

        var retVal = new T[n];

        int i;
        for (i = 0; i < n; i++)
        {
            a[i] = i + 1;
            p[i] = 0;
        }

        yield return list;

        i = 1;
        while (i < n)
        {
            if (p[i] < i)
            {
                var j = i % 2 * p[i];
                (a[j], a[i]) = (a[i], a[j]);

                for (var x = 0; x < n; x++)
                {
                    retVal[x] = list[a[x] - 1];
                }

                yield return retVal;

                p[i]++;
                i = 1;
            }
            else
            {
                p[i] = 0;
                i++;
            }
        }
    }

    public static IEnumerable<(T Current,T Next)> CurrentAndNext<T>(this IEnumerable<T> source)
    {
        using var e = source.GetEnumerator();
        if (!e.MoveNext())
            yield break;

        var previous = e.Current;
        while (e.MoveNext())
        {
            yield return (previous, e.Current);
            previous = e.Current;
        }
    }
}