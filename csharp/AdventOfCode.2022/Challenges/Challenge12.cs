using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2022.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (map, start, end) = ExtractInformation(await inputReader.ReadGridAsync(12));

        var bfs = new BreadthFirstSearch<Point2>(p => GetNeighbors(map, p));

        if (!bfs.TryPath(start, p => p == end, out var path))
            throw new InvalidOperationException("No path was found");

        return (path.Count() - 1).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (map, starts, end) = ExtractInformation2(await inputReader.ReadGridAsync(12));

        var bfs = new BreadthFirstSearch<Point2>(p => GetNeighbors(map, p));

        var lowest = int.MaxValue;
        foreach (var start in starts)
        {
            if (!bfs.TryPath(start, p => p == end, out var path))
                continue;

            lowest = Math.Min(path.Count() - 1, lowest);
        }

        return lowest.ToString();
    }

    private IEnumerable<Point2> GetNeighbors(int[,] grid, Point2 point)
    {
        foreach (var neighbor in point.GetNeighbors())
        {
            if (neighbor.X < 0 || neighbor.X >= grid.GetLength(1) || neighbor.Y < 0 || neighbor.Y >= grid.GetLength(0))
                continue;

            if (grid[neighbor.Y, neighbor.X] - grid[point.Y, point.X] > 1)
                continue;

            yield return neighbor;
        }
    }

    private static (int[,], Point2, Point2) ExtractInformation(char[,] originalMap)
    {
        var grid = new int[originalMap.GetLength(0), originalMap.GetLength(1)];
        var start = Point2.Zero;
        var end = Point2.Zero;

        for (var y = 0; y < originalMap.GetLength(0); y++)
        for (var x = 0; x < originalMap.GetLength(1); x++)
        {
            var c = originalMap[y, x];
            if (c == 'S')
            {
                start = new Point2(x, y);
                grid[y, x] = 0;
            }
            else if (c == 'E')
            {
                end = new Point2(x, y);
                grid[y, x] = 25;
            }
            else
            {
                grid[y, x] = c - 'a';
            }
        }

        return (grid, start, end);
    }

    private static (int[,], List<Point2>, Point2) ExtractInformation2(char[,] originalMap)
    {
        var grid = new int[originalMap.GetLength(0), originalMap.GetLength(1)];
        var starts = new List<Point2>();
        var end = Point2.Zero;

        for (var y = 0; y < originalMap.GetLength(0); y++)
        for (var x = 0; x < originalMap.GetLength(1); x++)
        {
            var c = originalMap[y, x];
            if (c == 'S')
            {
                starts.Add(new Point2(x, y));
                grid[y, x] = 0;
            }
            else if (c == 'E')
            {
                end = new Point2(x, y);
                grid[y, x] = 25;
            }
            else
            {
                grid[y, x] = c - 'a';
                if (grid[y, x] == 0)
                    starts.Add(new Point2(x, y));
            }
        }

        return (grid, starts, end);
    }
}