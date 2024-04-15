using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2016.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var discs = await inputReader.ParseLinesAsync(15, ParseLine).ToArrayAsync();

        return Euclid.ChineseRemainderTheorem(discs.Select((d, i) => -((long) d.Position + i + 1)).ToArray(),
            discs.Select(d => (long) d.NumberOfPositions).ToArray()).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var discs = await inputReader.ParseLinesAsync(15, ParseLine).ToArrayAsync();
        discs = [.. discs, new Disc(0, 11)];

        return Euclid.ChineseRemainderTheorem(discs.Select((d, i) => -((long) d.Position + i + 1)).ToArray(),
            discs.Select(d => (long) d.NumberOfPositions).ToArray()).ToString();
    }

    private Disc ParseLine(string line)
    {
        return line
            .Extract(@"^Disc #\d has (\d+) positions; at time=0, it is at position (\d+)\.$")
            .As<int>()
            .Into(match => new Disc(match[1], match[0]));
    }

    private record Disc(int Position, int NumberOfPositions);
}