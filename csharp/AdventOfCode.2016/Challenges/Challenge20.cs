using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var ranges = await inputReader.ParseLinesAsync(20, ParseLine).OrderBy(x => x.Start).ToListAsync();

        var i = 0L;
        for (var ri = 0; ri < ranges.Count; ri++)
        {
            if (i < ranges[ri].Start)
                return i.ToString();

            i = Math.Max(i, ranges[ri].End + 1);
        }

        return "no answer";
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var ranges = await inputReader.ParseLinesAsync(20, ParseLine).OrderBy(x => x.Start).ToListAsync();

        var (i, count) = (0L, 0L);
        for (var ri = 0; ri < ranges.Count; ri++)
        {
            if (i < ranges[ri].Start)
                count += ranges[ri].Start - i;

            i = Math.Max(i, ranges[ri].End + 1);
        }

        return count.ToString();
    }

    private LongRange ParseLine(string line) =>
        line.SplitBy("-").As<long>().Into(x => new LongRange(x.First(), x.Second()));

    private record LongRange(long Start, long End);
}