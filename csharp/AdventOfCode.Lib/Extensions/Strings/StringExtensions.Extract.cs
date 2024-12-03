using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode.Lib;

public static partial class StringExtensions
{
    public static string[] Extract(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        return match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
    }

    public static T[] Extract<T>(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        where T : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        return match.Groups.Values.Skip(1).Select(g => (T) Convert.ChangeType(g.Value, typeof(T))).ToArray();
    }

    public static (T1 First, T2 Second) Extract<T1, T2>(this string source,
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern)
        where T1 : IConvertible
        where T2 : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        if (items.Length != 2)
            throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 2, got {items.Length}");

        return (items[0].As<T1>(), items[1].As<T2>());
    }

    public static (T1 First, T2 Second, T3 Third) Extract<T1, T2, T3>(this string source,
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        if (items.Length != 3)
            throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 3, got {items.Length}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>());
    }

    public static (T1 First, T2 Second, T3 Third, T4 Fourth) Extract<T1, T2, T3, T4>(this string source,
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
        where T4 : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        if (items.Length != 4)
            throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 4, got {items.Length}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>());
    }

    public static (T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth) Extract<T1, T2, T3, T4, T5>(this string source,
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern)
        where T1 : IConvertible
        where T2 : IConvertible
        where T3 : IConvertible
        where T4 : IConvertible
        where T5 : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
            throw new InvalidOperationException("Regex was unsuccessful");

        var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        if (items.Length != 5)
            throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 5, got {items.Length}");

        return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>(), items[4].As<T5>());
    }

    public static bool TryExtract(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        out string[] matches)
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
        {
            matches = Array.Empty<string>();
            return false;
        }

        matches = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
        return true;
    }

    public static bool TryExtract<T>(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        out T[] matches)
        where T : IConvertible
    {
        var match = Regex.Match(source, pattern);
        if (!match.Success)
        {
            matches = Array.Empty<T>();
            return false;
        }

        matches = match.Groups.Values.Skip(1).Select(g => (T) Convert.ChangeType(g.Value, typeof(T))).ToArray();
        return true;
    }

    public static string[][] ExtractAll(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
    {
        var matches = Regex.Matches(source, pattern);
        if (matches.Count == 0)
            throw new InvalidOperationException("Regex was unsuccessful");

        return matches.Select(match => match.Groups.Values.Skip(1).Select(g => g.Value).ToArray()).ToArray();
    }
}