using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(1)]
public class Challenge01
{
    private readonly IInputReader _inputReader;

    public Challenge01(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        return await _inputReader
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
        return await _inputReader
            .ReadLinesAsync(1)
            .ChunkBy(string.Empty)
            .Select(x => x
                .Select(int.Parse)
                .Sum()
            ).OrderByDescending(_ => _)
            .Take(3)
            .SumAsync()
            .ToStringAsync();
    }
}