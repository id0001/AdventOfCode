using Microsoft;

namespace AdventOfCode.Lib;

public static partial class StringExtensions
{
    public static string CaesarShift(this string source, int shift)
    {
        return string.Join(string.Empty, source.ToLowerInvariant().Select(c => (char) ('a' + (c - 'a' + shift) % 26)));
    }

    public static bool IsAnagramOf(this string source, string other) => source.Order().SequenceEqual(other.Order());

    /// <summary>
    ///     Calculate the amount of letters that are different in a sequence of equal length.
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

    public static int IndexOfClosingPair(this string source, int startingIndex, char openingCharacter,
        char closingCharacter)
    {
        Requires.Argument(openingCharacter != closingCharacter, nameof(closingCharacter),
            "Parameters cannot be the same");
        Requires.Range(startingIndex < source.Length - 1, nameof(startingIndex));

        var depth = 0;
        for (var i = startingIndex; i < source.Length; i++)
            switch (source[i])
            {
                case '(':
                    depth++;
                    break;
                case ')' when depth == 0:
                    return i;
                case ')':
                    depth--;
                    break;
            }

        return -1;
    }
}