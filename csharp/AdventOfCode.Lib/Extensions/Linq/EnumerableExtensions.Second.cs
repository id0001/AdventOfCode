using Microsoft;

// ReSharper disable PossibleMultipleEnumeration

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static T Second<T>(this IEnumerable<T> source)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        if (!TryGetSecond(source, out var second))
            throw new InvalidOperationException("No elements found");

        return second!;
    }

    public static T SecondOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        if (!TryGetSecond(source, out var second))
            return defaultValue;

        return second!;
    }

    public static T Second<T>(this IList<T> source)
        where T : notnull
    {
        Requires.NotNull(source, nameof(source));

        return source[1];
    }

    public static T SecondOrDefault<T>(this IList<T> source, T defaultValue)
    {
        Requires.NotNull(source, nameof(source));

        return source.Count > 1 ? source[1] : defaultValue;
    }

    private static bool TryGetSecond<T>(IEnumerable<T> source, out T? second)
    {
        Requires.NotNull(source, nameof(source));

        if (source is IList<T> list)
        {
            if (list.Count > 1)
            {
                second = list[1];
                return true;
            }
        }
        else
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext() && e.MoveNext())
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