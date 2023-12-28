using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(9)]
public class Challenge09
{
    private readonly IInputReader _inputReader;

    public Challenge09(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        return await _inputReader.ReadLinesAsync(9)
            .Select(line => NextNumber(line.SplitBy(" ").As<int>().ToArray()))
            .SumAsync()
            .ToStringAsync();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        return await _inputReader.ReadLinesAsync(9)
            .Select(line => NextNumber(line.SplitBy(" ").As<int>().Reverse().ToArray()))
            .SumAsync()
            .ToStringAsync();
    }

    private int NextNumber(int[] sequence)
    {
        if (sequence.All(x => x == 0))
            return 0;

        var newSequence = sequence.Windowed(2).Select(pair => pair[1] - pair[0]).ToArray();
        var next = NextNumber(newSequence);
        return next + sequence[^1];
    }
}