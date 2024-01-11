using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader InputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        return await InputReader
            .ReadLinesAsync(1)
            .ChunkBy(string.Empty)
            .Select(x => x
                .Select(int.Parse)
                .Sum()
            ).MaxAsync()
            .ToStringAsync();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        return await InputReader
            .ReadLinesAsync(1)
            .ChunkBy(string.Empty)
            .Select(x => x
                .Select(int.Parse)
                .Sum()
            ).OrderByDescending(x => x)
            .Take(3)
            .SumAsync()
            .ToStringAsync();
    }
}