using AdventOfCode.Core.Parsing;

namespace AdventOfCode.Core.Extensions;

public static class StringExtensions
{
    public static StringSplitCollection SplitBy(this string source, string splitter)
    {
        return new StringSplitCollection(source.Split(new[] {splitter},
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }
}