using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader InputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        var count = 4;
        await foreach (var window in InputReader.ReadLineAsync(6).Windowed(4))
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
        await foreach (var window in InputReader.ReadLineAsync(6).Windowed(14))
        {
            if (window.Distinct().Count() == 14)
                return count.ToString();

            count++;
        }

        return null;
    }
}