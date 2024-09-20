using System.Collections.Immutable;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var bridges = await inputReader.ParseLinesAsync(24, ParseLine).ToListAsync();

        var max = int.MinValue;
        var starts = bridges.Where(b => b.A == 0 || b.B == 0);
        foreach (var start in starts)
            max = Math.Max(Strongest(bridges, start.A == 0 ? start.B : start.A, [start]), max);

        return max.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var bridges = await inputReader.ParseLinesAsync(24, ParseLine).ToListAsync();

        var max = int.MinValue;
        var length = 0;
        var starts = bridges.Where(b => b.A == 0 || b.B == 0);
        foreach (var start in starts)
        {
            var (m, l) = LongestAndStrongest(bridges, start.A == 0 ? start.B : start.A, [start]);
            if (l > length || (l == length && m > max))
                (max, length) = (m, l);
        }

        return max.ToString();
    }

    private int Strongest(List<Bridge> all, int outputPins, ImmutableHashSet<Bridge> visited)
    {
        var options = all.Where(b => !visited.Contains(b) && (b.A == outputPins || b.B == outputPins)).ToList();
        if (options.Count == 0)
            return visited.Sum(b => b.Strength);

        var max = int.MinValue;
        foreach (var bridge in options)
        {
            var newVisited = visited.Add(bridge);
            max = Math.Max(Strongest(all, bridge.A == outputPins ? bridge.B : bridge.A, newVisited), max);
        }

        return max;
    }

    private (int, int) LongestAndStrongest(List<Bridge> all, int outputPins, ImmutableHashSet<Bridge> visited)
    {
        var options = all.Where(b => !visited.Contains(b) && (b.A == outputPins || b.B == outputPins)).ToList();
        if (options.Count == 0)
            return (visited.Sum(x => x.Strength), visited.Count);

        var max = int.MinValue;
        var length = 0;
        foreach (var bridge in options)
        {
            var newVisited = visited.Add(bridge);
            var (m, l) = LongestAndStrongest(all, bridge.A == outputPins ? bridge.B : bridge.A, newVisited);
            if (l > length || (l == length && m > max))
                (max, length) = (m, l);
        }

        return (max, length);
    }

    private Bridge ParseLine(string line) => line.SplitBy("/").As<int>().Into(x => new Bridge(x.First(), x.Second()));

    private record Bridge(int A, int B)
    {
        public int Strength => A + B;
    }
}