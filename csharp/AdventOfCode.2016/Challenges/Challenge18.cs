using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2016.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var line = await inputReader.ReadLineAsync(18).ToListAsync();

        var safeCount = line.Count(c => c == '.');
        for (var row = 1; row < 40; row++)
        {
            line = Enumerable.Range(0, line.Count).Select(i =>
            {
                var leftTrap = i > 0 && line[i - 1] == '^';
                var rightTrap = i < line.Count - 1 && line[i + 1] == '^';

                return leftTrap ^ rightTrap ? '^' : '.';
            }).ToList();
            safeCount += line.Count(c => c == '.');
        }

        return safeCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var line = await inputReader.ReadLineAsync(18).ToListAsync();

        var safeCount = line.Count(c => c == '.');
        for (var row = 1; row < 400000; row++)
        {
            line = Enumerable.Range(0, line.Count).Select(i =>
            {
                var leftTrap = i > 0 && line[i - 1] == '^';
                var rightTrap = i < line.Count - 1 && line[i + 1] == '^';

                return leftTrap ^ rightTrap ? '^' : '.';
            }).ToList();
            safeCount += line.Count(c => c == '.');
        }

        return safeCount.ToString();
    }
}