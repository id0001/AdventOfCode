using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => (await inputReader.ReadLinesAsync(1)
            .Select(int.Parse)
            .AggregateAsync(0, (a, b) => a + (b / 3 - 2)))
        .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadLinesAsync(1)
            .Select(int.Parse)
            .AggregateAsync(0, (a, b) => a + CalculateFuelRequirement(b)))
        .ToString();

    private static int CalculateFuelRequirement(int mass)
    {
        if (mass == 0) return 0;
        return Math.Max(0, mass / 3 - 2) + CalculateFuelRequirement(Math.Max(0, mass / 3 - 2));
    }
}