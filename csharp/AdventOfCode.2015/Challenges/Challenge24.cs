using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var weights = await inputReader.ReadLinesAsync<long>(24).OrderByDescending(x => x).ToListAsync();
        var targetTotal = weights.Sum() / 3;

        return Enumerable.Range(4, weights.Count)
            .SelectMany(i => weights
                .Combinations(i)
                .Where(x => x.Sum() == targetTotal)
                .Select(x => x.Product())
                .Order())
            .First()
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var weights = await inputReader.ReadLinesAsync<long>(24).OrderByDescending(x => x).ToListAsync();
        var targetTotal = weights.Sum() / 4;

        return Enumerable.Range(4, weights.Count)
            .SelectMany(i => weights
                .Combinations(i)
                .Where(x => x.Sum() == targetTotal)
                .Select(x => x.Product())
                .Order())
            .First()
            .ToString();
    }
}
