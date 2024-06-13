using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => await inputReader.ParseLinesAsync(13, ParseLine)
        .AggregateAsync(0, (score, shell) => score + (GetPositionInTime(shell.Layer, shell.Depth) == 0 ? shell.Layer * shell.Depth : 0))
        .ToStringAsync();

    [Part2]
    public async Task<string> Part2Async()
    {
        var shells = await inputReader.ParseLinesAsync(13, ParseLine).ToListAsync();
        return Enumerable.Range(0, int.MaxValue)
            .Where(delay => !shells.Any(l => GetPositionInTime(delay + l.Layer, l.Depth) == 0))
            .First()
            .ToString();
    }

    private static (int Layer, int Depth) ParseLine(string line) => line.SplitBy(":").As<int>().Into(parts => (parts.First(), parts.Second()));

    private int GetPositionInTime(int time, int depth)
    {
        var seqlen = depth + (depth - 2);
        var pos = time % seqlen;
        return pos >= depth ? seqlen - pos : pos;
    }
}