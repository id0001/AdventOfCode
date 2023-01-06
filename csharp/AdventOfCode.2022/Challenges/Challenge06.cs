using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(6)]
public class Challenge06
{
    private readonly IInputReader _inputReader;

    public Challenge06(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var count = 4;
        await foreach (var window in _inputReader.ReadLineAsync(6).Windowed(4))
        {
            if (window.Distinct().Count() == 4)
                return count.ToString();

            count++;
        }

        return null;
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var count = 14;
        await foreach (var window in _inputReader.ReadLineAsync(6).Windowed(14))
        {
            if (window.Distinct().Count() == 14)
                return count.ToString();

            count++;
        }

        return null;
    }
}