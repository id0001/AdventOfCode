using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(4)]
public class Challenge04(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => await inputReader
        .ReadLinesAsync(4)
        .Where(x => x.SplitBy(" ").Into(s => s.Length == s.Distinct().Count()))
        .CountAsync()
        .ToStringAsync();

    [Part2]
    public async Task<string> Part2Async() => await inputReader
        .ReadLinesAsync(4)
        .Where(x => !x.SplitBy(" ").Combinations(2).Any(pair => pair.First().IsAnagramOf(pair.Second())))
        .CountAsync()
        .ToStringAsync();
}