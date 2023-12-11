using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(11)]
public class Challenge11
{
    private readonly IInputReader _inputReader;

    public Challenge11(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(11);
        var galaxies = GetGalaxies(grid, 1).ToList();

        int totalDistance = 0;
        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                totalDistance += Point2.ManhattanDistance(galaxies[i], galaxies[j]);
            }
        }

        return totalDistance.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(11);
        var galaxies = GetGalaxies(grid, 999999).ToList();

        long totalDistance = 0;
        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                totalDistance += Point2.ManhattanDistance(galaxies[i], galaxies[j]);
            }
        }

        return totalDistance.ToString();
    }

    private IEnumerable<int> GetEmptyRows(char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            if (!Enumerable.Range(0, grid.GetLength(1))
                .Any(x => grid[y, x] == '#'))
                yield return y;
        }
    }

    private IEnumerable<int> GetEmptyColumns(char[,] grid)
    {
        for (var x = 0; x < grid.GetLength(1); x++)
        {
            if (!Enumerable.Range(0, grid.GetLength(0))
                .Any(y => grid[y, x] == '#'))
                yield return x;
        }
    }

    private IEnumerable<Point2> GetGalaxies(char[,] grid, int expansion)
    {
        var emptyRows = GetEmptyRows(grid).ToList();
        var emptyColumns = GetEmptyColumns(grid).ToList();

        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[y, x] == '#')
                    yield return ExpandGalaxyPosition(new Point2(x, y), emptyRows, emptyColumns, expansion);
            }
        }
    }

    private Point2 ExpandGalaxyPosition(Point2 p, IList<int> emptyRows, IList<int> emptyColumns, int expansion)
    {
        var x = p.X + (expansion * emptyColumns.Count(c => c < p.X));
        var y = p.Y + (expansion * emptyRows.Count(r => r < p.Y));
        return new Point2(x, y);
    }
}
