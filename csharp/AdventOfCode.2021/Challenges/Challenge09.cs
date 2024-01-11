using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await InputReader.ReadGridAsync<int>(9);
        return CalculateRiskLevel(grid).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await InputReader.ReadGridAsync<int>(9);

        var basins = new List<Point2[]>();
        foreach (var lowestPoint in LowestPoints(grid))
        {
            var set = new HashSet<Point2>();
            FloodFill(lowestPoint, grid, set);
            basins.Add(set.ToArray());
        }

        return basins
            .Select(list => list.Length)
            .OrderByDescending(x => x)
            .Take(3)
            .Product()
            .ToString();
    }

    private static IEnumerable<Point2> LowestPoints(int[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
        {
            if (grid[y, x] == 9)
                continue;

            var p = new Point2(x, y);
            if (IsLowerThanNeighbors(grid, p)) yield return p;
        }
    }

    private static void FloodFill(Point2 p, int[,] grid, ISet<Point2> points)
    {
        if (p.X < 0 || p.Y < 0 || p.X >= grid.GetLength(1) || p.Y >= grid.GetLength(0))
            return;

        if (points.Contains(p) || grid[p.Y, p.X] == 9)
            return;

        points.Add(p);
        foreach (var neighbor in p.GetNeighbors())
            FloodFill(neighbor, grid, points);
    }

    private static int CalculateRiskLevel(int[,] grid)
    {
        var risk = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
        {
            if (grid[y, x] == 9)
                continue;

            var p = new Point2(x, y);
            if (IsLowerThanNeighbors(grid, p)) risk += grid[y, x] + 1;
        }

        return risk;
    }

    private static bool IsLowerThanNeighbors(int[,] grid, Point2 p) => p.GetNeighbors()
        .All(n =>
        {
            if (n.X < 0 || n.Y < 0 || n.X >= grid.GetLength(1) || n.Y >= grid.GetLength(0))
                return true;

            return grid[p.Y, p.X] < grid[n.Y, n.X];
        });
}