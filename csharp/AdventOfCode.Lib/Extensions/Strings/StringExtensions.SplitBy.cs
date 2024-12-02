namespace AdventOfCode.Lib;

public static partial class StringExtensions
{
    public static IList<string> SplitBy(this string source, params string[] separators)
    {
        return source.Split(separators,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static (T1 First, T2 Second) SplitBy<T1, T2>(this string source, string separator)
        where T1 : IConvertible
        where T2 : IConvertible
    {
        var items = source.SplitBy(separator);
        if (items.Count > 2)
            throw new InvalidOperationException($"Incorrect amount of split items. Expected 2, got {items.Count}");

        return (items[0].As<T1>(), items[1].As<T2>());
    }

    public static (T1 First, T2 Second, T3 Third) SplitBy<T1, T2, T3>(this string source, string separator)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
    {
        var items = source.SplitBy(separator);
        if (items.Count > 3)
            throw new InvalidOperationException($"Incorrect amount of split items. Expected 3, got {items.Count}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>());
    }

    public static (T1 First, T2 Second, T3 Third, T4 Fourth) SplitBy<T1, T2, T3, T4>(this string source,
        string separator)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
        where T4 : IConvertible
    {
        var items = source.SplitBy(separator);
        if (items.Count > 4)
            throw new InvalidOperationException($"Incorrect amount of split items. Expected 4, got {items.Count}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>());
    }

    public static (T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth) SplitBy<T1, T2, T3, T4, T5>(this string source,
        string separator)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
        where T4 : IConvertible
        where T5 : IConvertible
    {
        var items = source.SplitBy(separator);
        if (items.Count > 5)
            throw new InvalidOperationException($"Incorrect amount of split items. Expected 5, got {items.Count}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>(), items[4].As<T5>());
    }
}