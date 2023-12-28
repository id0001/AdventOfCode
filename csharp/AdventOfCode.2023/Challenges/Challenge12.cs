using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        return await inputReader.ParseLinesAsync(12, ParseLine).SumAsync(x =>
            CountPossibilities(new Dictionary<(int, int, int), long>(), x.Item1, x.Item2, 0, 0, 0)).ToStringAsync();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        return await inputReader.ParseLinesAsync(12, ParseLine2).SumAsync(x =>
            CountPossibilities(new Dictionary<(int, int, int), long>(), x.Item1, x.Item2, 0, 0, 0)).ToStringAsync();
    }

    private long CountPossibilities(Dictionary<(int, int, int), long> cache, string springs, int[] groups, int si,
        int gi, int captureLength)
    {
        if (cache.TryGetValue((si, gi, captureLength), out var possibilities))
            return possibilities;

        if (si == springs.Length) // End of string
            return gi == groups.Length || (gi == groups.Length - 1 && groups[gi] == captureLength)
                ? 1 // All groups captured
                : 0; // Not all groups captured

        possibilities = 0L;

        if (springs[si] is '.' or '?') // Assuming '?' is '.'
        {
            if (captureLength == 0) // Before a capture
                possibilities +=
                    CountPossibilities(cache, springs, groups, si + 1, gi, 0); // Not in capture. Skip to next char
            else if (captureLength == groups[gi]) // After a capture
                possibilities +=
                    CountPossibilities(cache, springs, groups, si + 1, gi + 1, 0); // Captured group. Go to next group
        }

        if (springs[si] is '#' or '?' && gi < groups.Length && captureLength < groups[gi]) // Assuming '?' is '#'
            possibilities += CountPossibilities(cache, springs, groups, si + 1, gi, captureLength + 1); // In capture

        cache[(si, gi, captureLength)] = possibilities;
        return possibilities;
    }

    private (string, int[]) ParseLine(string line)
    {
        return line.SplitBy(" ")
            .Into(parts => (
                parts.First(),
                parts.Second().SplitBy(",").Select(int.Parse).ToArray()
            ));
    }

    private (string, int[]) ParseLine2(string line)
    {
        var parts = line.SplitBy(" ");
        var first = string.Join("?", parts.First(), parts.First(), parts.First(), parts.First(), parts.First());
        var second = parts.Second().SplitBy(",").Select(int.Parse).ToArray();

        return (first, second.Concat(second).Concat(second).Concat(second).Concat(second).ToArray());
    }
}