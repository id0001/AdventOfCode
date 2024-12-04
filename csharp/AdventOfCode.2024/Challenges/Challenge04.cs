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

        return (CountXMasHorizontal(grid)
                + CountXMasVertical(grid)
                + CountXmasDownLeftDiagonal(grid)
                + CountXmasDownRightDiagonal(grid)
            ).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(4);
        return grid.Windowed(3, 3).Count(IsMas).ToString();
    }

    private static int CountXMasHorizontal(char[,] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.GetLength(0); i++)
        {
            var row = grid.GetRow(i);
            count += row.Windowed(4).Count(IsXmas);
        }

        return count;
    }

    private static int CountXMasVertical(char[,] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.GetLength(1); i++)
        {
            var col = grid.GetColumn(i);
            count += col.Windowed(4).Count(IsXmas);
        }

        return count;
    }

    private static int CountXmasDownRightDiagonal(char[,] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.GetDiagonalLength(); i++)
        {
            var diagonal = grid.GetDownRightDiagonal(i);
            count += diagonal.Windowed(4).Count(IsXmas);
        }

        return count;
    }

    private static int CountXmasDownLeftDiagonal(char[,] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.GetDiagonalLength(); i++)
        {
            var diagonal = grid.GetDownLeftDiagonal(i);
            count += diagonal.Windowed(4).Count(IsXmas);
        }

        return count;
    }

    private static bool IsXmas(IList<char> window) =>
        (window[0] == 'X' && window[1] == 'M' && window[2] == 'A' && window[3] == 'S') ||
        (window[3] == 'X' && window[2] == 'M' && window[1] == 'A' && window[0] == 'S');

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