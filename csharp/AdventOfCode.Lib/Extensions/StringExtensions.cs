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

    public static string[][] ExtractAll(this string source, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
    {
        var matches = Regex.Matches(source, pattern);
        if (!matches.Any())
            throw new InvalidOperationException("Regex was unsuccessful");

        return matches.Select(match => match.Groups.Values.Skip(1).Select(g => g.Value).ToArray()).ToArray();
    }
}