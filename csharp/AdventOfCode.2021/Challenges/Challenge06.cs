using AdventOfCode.Lib;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(6)]
public class Challenge06
{
    private readonly IInputReader _inputReader;

    public Challenge06(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var data = await _inputReader.ReadLineAsync<int>(6, ',').ToListAsync();

        var groups = new ulong[9];
        foreach (var i in data)
            groups[i]++;

        return CalculateFishies(80, groups).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var data = await _inputReader.ReadLineAsync<int>(6, ',').ToListAsync();

        var groups = new ulong[9];
        foreach (var i in data)
            groups[i]++;

        return CalculateFishies(256, groups).ToString();
    }

    private ulong CalculateFishies(int totalDays, ulong[] groups) => Breed(0, totalDays, groups).Sum(x => x);

    private static ulong[] Breed(int dayFrom, int totalDays, ulong[] groups)
    {
        if (dayFrom == totalDays)
            return groups;

        return Breed(dayFrom + 1, totalDays,
            new[]
            {
                groups[1], groups[2], groups[3], groups[4], groups[5], groups[6], groups[0] + groups[7], groups[8],
                groups[0]
            });
    }
}