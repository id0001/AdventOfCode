using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(3)]
public class Challenge03
{
    private readonly IInputReader _inputReader;

    public Challenge03(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await _inputReader.ReadLinesAsync(3).ToArrayAsync();

        return TraverseSlope(input, 3, 1).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await _inputReader.ReadLinesAsync(3).ToArrayAsync();

        return (TraverseSlope(input, 1, 1) * TraverseSlope(input, 3, 1) * TraverseSlope(input, 5, 1) *
                TraverseSlope(input, 7, 1) *
                TraverseSlope(input, 1, 2)).ToString();
    }

    private static long TraverseSlope(IReadOnlyList<string> input, int dx, int dy)
    {
        var x = 0;
        var y = 0;

        var treeCount = 0;
        while (y < input.Count)
        {
            if (input[y][x] == '#')
                treeCount++;

            x = (x + dx) % input[0].Length;
            y += dy;
        }

        return treeCount;
    }
}