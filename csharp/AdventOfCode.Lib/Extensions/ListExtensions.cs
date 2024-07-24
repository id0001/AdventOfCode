using System.Collections;

namespace AdventOfCode.Lib;

public static class ListExtensions
{
    public static T First<T>(this IList<T> source)
        where T : notnull
        => source[0];

    public static T Second<T>(this IList<T> source)
        where T : notnull
        => source[1];

    public static T SecondOrDefault<T>(this IList<T> source, T defaultValue)
        => source.Count > 1 ? source[1] : defaultValue;

    public static T Third<T>(this IList<T> source)
        where T : notnull
        => source[2];

    public static T ThirdOrDefault<T>(this IList<T> source, T defaultValue)
        => source.Count > 2 ? source[2] : defaultValue;

    public static IList<T> As<T>(this IList source)
        where T : IConvertible
    {
        return source.Cast<IConvertible>().Select(x => x.As<T>()).ToList();
    }
}