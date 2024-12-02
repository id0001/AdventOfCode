using System.Collections;
using System.Text;
using Microsoft;

// ReSharper disable PossibleMultipleEnumeration

namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static IList<T> As<T>(this IList<string> source)
        where T : IConvertible
    {
        Requires.NotNull(source, nameof(source));

        return source.Cast<IConvertible>().Select(x => x.As<T>()).ToList();
    }
    
    public static IEnumerable<T2> As<T1,T2>(this IEnumerable<T1> source)
        where T1 : IConvertible
        where T2 : IConvertible
    {
        Requires.NotNull(source, nameof(source));
        return source.Cast<IConvertible>().Select(x => x.As<T2>());
    }

    public static string AsString(this IEnumerable<char> source)
    {
        Requires.NotNull(source, nameof(source));

        var sb = new StringBuilder();
        foreach (var c in source)
            sb.Append(c);

        return sb.ToString();
    }
}