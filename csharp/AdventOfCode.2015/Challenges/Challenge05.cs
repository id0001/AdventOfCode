using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(05)]
public class Challenge05
{
    private readonly IInputReader _inputReader;

    public Challenge05(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var pattern1 = new Regex("a|e|i|o|u");
        var pattern2 = new Regex(@"(\w)\1");
        var pattern3 = new Regex(@"ab|cd|pq|xy");

        var niceCount = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(5))
        {
            if (pattern1.Matches(line).Count < 3)
                continue;

            if (!pattern2.IsMatch(line))
                continue;

            if (pattern3.IsMatch(line))
                continue;

            niceCount++;
        }

        return niceCount.ToString();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var niceCount = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(5))
        {
            if (ContainsPairThatAppearsTwice(line) && ContainsRepeatingLetterWithOneLetterCap(line))
                niceCount++;
        }

        return niceCount.ToString();
    }

    private bool ContainsPairThatAppearsTwice(string value) =>
        FilterPairs(value).GroupBy(x => x).Any(x => x.Count() >= 2);

    private bool ContainsRepeatingLetterWithOneLetterCap(string value)
    {
        for (var i = 2; i < value.Length; i++)
        {
            if (value[i - 2] == value[i])
                return true;
        }

        return false;
    }

    private IEnumerable<string> FilterPairs(string value)
    {
        string? prevPair = null;
        var lastAddedIndex = -1;
        for (var i = 1; i < value.Length; i++)
        {
            var pair = value[(i - 1)..(i + 1)];
            if (prevPair is not null && prevPair == pair && i - lastAddedIndex < 2)
                continue;

            prevPair = pair;
            lastAddedIndex = i;
            yield return pair;
        }
    }
}