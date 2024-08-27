using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader inputReader)
{
    const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [Part1]
    public async Task<string> Part1Async()
    {
        var polymer = await inputReader.ReadAllTextAsync(5);
        return GetReducedLength(polymer).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var polymer = await inputReader.ReadAllTextAsync(5);

        return Alphabet
            .Select(c => GetReducedLength(polymer, c))
            .Min()
            .ToString();
    }

    private static bool IsPair(char a, char b)
    {
        var isAUpper = char.IsUpper(a);
        var isBUpper = char.IsUpper(b);

        if (!(isAUpper ^ isBUpper))
            return false;

        return char.ToUpperInvariant(a) == char.ToUpperInvariant(b);
    }

    private static int GetReducedLength(string polymer, char? toDestroy = null)
    {
        if (toDestroy.HasValue)
            polymer = Regex.Replace(polymer, toDestroy.Value.ToString(), string.Empty, RegexOptions.IgnoreCase);

        var stack = new Stack<char>();
        foreach(var c in polymer)
        {
            if(stack.Count == 0)
            {
                stack.Push(c);
                continue;
            }

            if (IsPair(stack.Peek(), c))
                stack.Pop();
            else
                stack.Push(c);
        }

        return stack.Count;
    }
}
