using Microsoft;
// ReSharper disable PossibleMultipleEnumeration

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static T Third<T>(this IEnumerable<T> source)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        if (!TryGetThird(source, out var second))
            throw new InvalidOperationException("No elements found");

        return second!;
    }

    public static T ThirdOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        if (!TryGetThird(source, out var second))
            return defaultValue;

        return second!;
    }

    public static T Third<T>(this IList<T> source)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        return source[2];
    }

    public static T ThirdOrDefault<T>(this IList<T> source, T defaultValue)
    {
        Requires.NotNull(source, nameof(source));

        return source.Count > 2 ? source[2] : defaultValue;
    }

    private static bool TryGetThird<T>(IEnumerable<T> source, out T? second)
    {
        Requires.NotNull(source, nameof(source));

        if (source is IList<T> list)
        {
            if (list.Count > 2)
            {
                second = list[2];
                return true;
            }
        }
        else
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext() && e.MoveNext() && e.MoveNext())
                {
                    second = e.Current;
                    return true;
                }
            }
        }

        second = default;
        return false;
    }
}