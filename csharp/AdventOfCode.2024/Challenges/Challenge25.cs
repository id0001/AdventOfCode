using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (locks, keys) = await inputReader.ParseTextAsync(25, ParseInput);

        var fitCount = 0;
        foreach (var @lock in locks)
        foreach (var key in keys)
            if (@lock.Zip(key).Select(z => z.First + z.Second).All(z => z < 6))
                fitCount++;

        return fitCount.ToString();
    }

    private static (List<int[]> Locks, List<int[]> Keys) ParseInput(string input)
    {
        var paragraphs = input.SelectParagraphs();

        var locks = new List<int[]>();
        var keys = new List<int[]>();

        foreach (var paragraph in paragraphs)
        {
            var lines = paragraph.SelectLines();
            if (lines[0] == "#####")
                locks.Add(GetLock(lines));
            else
                keys.Add(GetKey(lines));
        }

        return (locks, keys);
    }

    private static int[] GetLock(IList<string> lines)
        => Enumerable.Range(0, 5)
            .Select(i => lines.Skip(1).Take(5).Aggregate(0, (a, v) => a + (v[i] == '#' ? 1 : 0)))
            .ToArray();

    private static int[] GetKey(IList<string> lines)
        => Enumerable
            .Range(0, 5).Select(i => lines.Skip(1).Take(5).Aggregate(5, (a, v) => a - (v[i] == '.' ? 1 : 0)))
            .ToArray();
}