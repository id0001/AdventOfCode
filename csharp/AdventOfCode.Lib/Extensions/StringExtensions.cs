using AdventOfCode.Lib.Parsing;
using System.Text.RegularExpressions;

namespace AdventOfCode.Lib;

public static class StringExtensions
{
    public static StringTokenCollection SplitBy(this string source, string separator)
    {
        return new StringTokenCollection(source.Split(new[] { separator },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }

    public static StringTokenCollection SplitBy(this string source, params string[] separators)
    {
        return new StringTokenCollection(source.Split(separators,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }

    public static T SplitBy<T>(this string source, string separator, Func<StringTokenCollection, T> selector)
    {
        return selector(new StringTokenCollection(source.Split(new[] { separator },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));
    }

    public static T SplitBy<T>(this string source, string[] separators, Func<StringTokenCollection, T> selector)
    {
        return selector(new StringTokenCollection(source.Split(separators,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));
    }

    public static MatchTokenCollection MatchBy(this string source, string pattern)
    {
        return new MatchTokenCollection(Regex.Matches(source, pattern).ToArray());
    }

    public static T MatchBy<T>(this string source, string pattern, Func<MatchTokenCollection, T> selector)
    {
        return selector(new MatchTokenCollection(Regex.Matches(source, pattern).ToArray()));
    }

    public static MatchTokenCollection MatchBy(this string source, Regex pattern)
    {
        return new MatchTokenCollection(pattern.Matches(source).ToArray());
    }

    public static T MatchBy<T>(this string source, Regex pattern, Func<MatchTokenCollection, T> selector)
    {
        return selector(new MatchTokenCollection(pattern.Matches(source).ToArray()));
    }
}