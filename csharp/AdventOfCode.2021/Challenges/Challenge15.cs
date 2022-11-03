using AdventOfCode.Lib;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2021.Challenges;

[Challenge(15)]
public class Challenge15
{
    private readonly IInputReader _inputReader;

    public Challenge15(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync<int>(15);

        var start = Point2.Zero;
        var end = new Point2(grid.GetLength(0) - 1, grid.GetLength(1) - 1);

        var astar = new AStar<Point2>(p => GetAdjacentNodes(grid, p), (_, p) => grid[p.Y, p.X]);
        astar.TryPath(start, p => p == end, out _,
            out var distance);

        return distance.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync<int>(15);

        var start = Point2.Zero;
        var end = new Point2(grid.GetLength(0) * 5 - 1, grid.GetLength(1) * 5 - 1);

        var astar = new AStar<Point2>(p => GetAdjacentNodes2(grid, p), (_, n) => GetWeight2(grid, n));
        astar.TryPath(start, p => p == end, out _,
            out var distance);

        return distance.ToString();
    }

    private static IEnumerable<Point2> GetAdjacentNodes(int[,] grid, Point2 currentNode)
    {
        foreach (var neighbor in currentNode.GetNeighbors())
        {
            if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= grid.GetLength(1) || neighbor.Y >= grid.GetLength(0))
                continue;

            yield return neighbor;
        }
    }

    private static int GetWeight2(int[,] grid, Point2 neighbor)
    {
        var rx = neighbor.X % grid.GetLength(1);
        var ry = neighbor.Y % grid.GetLength(0);
        var fx = neighbor.X / grid.GetLength(1);
        var fy = neighbor.Y / grid.GetLength(0);
        return (grid[ry, rx] + fx + fy - 1) % 9 + 1;
    }

    private static IEnumerable<Point2> GetAdjacentNodes2(int[,] grid, Point2 currentNode)
    {
        var width = grid.GetLength(1) * 5;
        var height = grid.GetLength(0) * 5;

        foreach (var neighbor in currentNode.GetNeighbors())
        {
            if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= width || neighbor.Y >= height)
                continue;

            yield return neighbor;
        }
    }
}