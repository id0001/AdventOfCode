using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2016.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        const int length = 8;
        var matrix = new int[length][].Select(_ => new int[26]).ToArray();
        await foreach (var line in inputReader.ReadLinesAsync(6))
            for (var i = 0; i < length; i++)
                matrix[i][line[i] - 'a']++;

        return string.Join(string.Empty,
            matrix.Select(arr => arr.Select((n, i) => (Char: (char) ('a' + i), Count: n)).MaxBy(x => x.Count).Char));
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        const int length = 8;
        var matrix = new int[length][].Select(_ => new int[26]).ToArray();
        await foreach (var line in inputReader.ReadLinesAsync(6))
            for (var i = 0; i < length; i++)
                matrix[i][line[i] - 'a']++;

        return string.Join(string.Empty,
            matrix.Select(arr => arr.Select((n, i) => (Char: (char) ('a' + i), Count: n)).MinBy(x => x.Count).Char));
    }
}