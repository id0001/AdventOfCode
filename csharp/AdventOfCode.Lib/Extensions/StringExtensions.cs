using Microsoft;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode.Lib;

public static class StringExtensions
{
    public static string[] SplitBy(this string source, string separator)
    {
        return source.Split(new[] { separator },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitBy(this string source, params string[] separators)
    {
        return source.Split(separators,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string CaesarShift(this string source, int shift)
    {
        return string.Join(string.Empty, source.ToLowerInvariant().Select(c => (char)('a' + (c - 'a' + shift) % 26)));
    }

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

        return match.Groups.Values.Skip(1).Select(g => (T)Convert.ChangeType(g.Value, typeof(T))).ToArray();
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

        matches = match.Groups.Values.Skip(1).Select(g => (T)Convert.ChangeType(g.Value, typeof(T))).ToArray();
        return true;
    }

    public static string[][] ExtractAll(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
    {
        var matches = Regex.Matches(source, pattern);
        if (!matches.Any())
            throw new InvalidOperationException("Regex was unsuccessful");

        return matches.Select(match => match.Groups.Values.Skip(1).Select(g => g.Value).ToArray()).ToArray();
    }

    public static bool IsAnagramOf(this string source, string other) => source.Order().SequenceEqual(other.Order());

    /// <summary>
    /// Calculate the amount of letters that are different in a sequence of equal length.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static int HammingDistance(this string source, string other)
    {
        Requires.NotNull(source, nameof(source));
        Requires.NotNull(other, nameof(other));
        Requires.Argument(source.Length == other.Length, nameof(other), "Parameters must be of equal length");

        return source.Zip(other).Count(pair => pair.First != pair.Second);
    }
}