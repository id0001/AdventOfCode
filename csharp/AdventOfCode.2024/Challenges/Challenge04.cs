using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(4)]
public class Challenge04(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(4);

        string match = "XMAS";
        string matchReverse = "SAMX";

        int count = 0;
        count += grid.EnumerateRows().Sum(row => row.Windowed(4).Count(r => r.SequenceEqual(match) || r.SequenceEqual(matchReverse)));
        count += grid.EnumerateColumns().Sum(col => col.Windowed(4).Count(r => r.SequenceEqual(match) || r.SequenceEqual(matchReverse)));
        count += grid.EnumerateDownLeftDiagonals().Sum(diagonal => diagonal.Windowed(4).Count(r => r.SequenceEqual(match) || r.SequenceEqual(matchReverse)));
        count += grid.EnumerateDownRightDiagonals().Sum(diagonal => diagonal.Windowed(4).Count(r => r.SequenceEqual(match) || r.SequenceEqual(matchReverse)));
        return count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(4);
        return grid.Windowed(3, 3).Count(IsMas).ToString();
    }

    private static bool IsMas(char[,] grid)
    {
        if (grid[1, 1] != 'A')
            return false;

        var count = 0;
        if ((grid[0, 0] == 'M' && grid[2, 2] == 'S') || (grid[0, 0] == 'S' && grid[2, 2] == 'M'))
            count++;

        if ((grid[0, 2] == 'M' && grid[2, 0] == 'S') || (grid[0, 2] == 'S' && grid[2, 0] == 'M'))
            count++;

        return count == 2;
    }
}