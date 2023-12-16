namespace AdventOfCode.Lib;

public static class StringExtensions
{
    public static string[] SplitBy(this string source, string separator)
    {
        return source.Split(new[] {separator},
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
    
    public static string[] SplitBy(this string source, params string[] separators)
    {
        return source.Split(separators,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}