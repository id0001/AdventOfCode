using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2021.Challenges;

[Challenge(7)]
public class Challenge07
{
    private readonly IInputReader _inputReader;

    public Challenge07(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var data = await _inputReader.ReadLineAsync<int>(7, ',').ToArrayAsync();

        var min = data.Min();
        var max = data.Max();

        var lowest = int.MaxValue;
        for (var pos = min; pos < max; pos++)
        {
            var test = data.Select(x => Math.Abs(x - pos)).Sum();
            if (test < lowest)
                lowest = test;
        }

        return lowest.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var data = await _inputReader.ReadLineAsync<int>(7, ',').ToArrayAsync();

        var min = data.Min();
        var max = data.Max();

        var lowest = int.MaxValue;
        for (var pos = min; pos < max; pos++)
        {
            var test = data.Select(x => Euclid.TriangularNumber(Math.Abs(x - pos))).Sum();
            if (test < lowest)
                lowest = test;
        }

        return lowest.ToString();
    }
}