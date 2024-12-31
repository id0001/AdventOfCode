using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(20);
        var start = grid.Find((_, c) => c == 'S');
        var end = grid.Find((_, c) => c == 'E');

        var pathNoCheats = grid
            .Path(start, GetAdjacent)
            .FindShortest(n => n == end);

        var c = 0;
        for (var i = 0; i < pathNoCheats.Path.Count; i++)
        for (var j = 0; j < i; j++)
        {
            var dist = Point2.ManhattanDistance(pathNoCheats.Path[i],
                pathNoCheats.Path[j]); // Distance between 2 points on the path
            var saves = i - (j + dist); // distance to i - (distance to j - dist)

            if (dist <= 2 && saves >= 100) // dist <= 2 = .#. (2 parts of the path with a wall in between
                c++;
        }

        return c.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(20);
        var start = grid.Find((_, c) => c == 'S');
        var end = grid.Find((_, c) => c == 'E');

        var pathNoCheats = grid
            .Path(start, GetAdjacent)
            .FindShortest(n => n == end);

        var c = 0;
        for (var i = 0; i < pathNoCheats.Path.Count; i++)
        for (var j = 0; j < i; j++)
        {
            var dist = Point2.ManhattanDistance(pathNoCheats.Path[i],
                pathNoCheats.Path[j]); // Distance between 2 points on the path
            var saves = i - (j + dist); // distance to i - (distance to j - dist)

            if (dist <= 20 && saves >= 100)
                c++;
        }

        return c.ToString();
    }

    private static IEnumerable<Point2> GetAdjacent(char[,] grid, Point2 current)
    {
        var bounds = grid.Bounds();
        return current.GetNeighbors().Where(n => bounds.Contains(n) && grid[n.Y, n.X] != '#');
    }
}