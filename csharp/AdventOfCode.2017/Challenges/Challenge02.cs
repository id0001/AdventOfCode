using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => (await inputReader.ReadLinesAsync(2).ToListAsync())
            .Select(line => line.SplitBy("\t")
                .As<int>()
                .Into(row => row.Max() - row.Min()))
            .Sum()
            .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadLinesAsync(2).ToListAsync())
            .Select(line => line.SplitBy("\t")
                .As<int>()
                .Combinations(2)
                .Where(pair => Math.Max(pair[0], pair[1]) % Math.Min(pair[0], pair[1]) == 0)
                .Select(pair => Math.Max(pair[0], pair[1]) / Math.Min(pair[0], pair[1]))
                .Single()
                )
            .Sum()
            .ToString();
}
