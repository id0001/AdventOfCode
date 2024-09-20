namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
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
}