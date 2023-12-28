using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2023.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(10);
        var start = grid.FindPosition(c => c == 'S');
        grid[start.Y, start.X] = GetStartType(grid, start);
        var bfs = new BreadthFirstSearch<Point2>(n => GetConnectingPipes(grid, n));
        return bfs.FloodFill(start).MaxBy(x => x.Distance).Distance.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(10);
        var start = grid.FindPosition(c => c == 'S');
        grid[start.Y, start.X] = GetStartType(grid, start);
        var pipeSegments = WalkPath(grid, start).ToList();

        return Polygon.CountInteriorPoints((long) Polygon.ShoelaceArea(pipeSegments), pipeSegments.Count).ToString();
    }

    private static IEnumerable<Point2> WalkPath(char[,] grid, Point2 start)
    {
        var (next, end) = GetConnectingPipes(grid, start).ToArray().Into(x => (x[0], x[1]));
        var visited = new HashSet<Point2>();

        yield return start;

        var current = next;
        while (current != end)
        {
            yield return current;
            visited.Add(current);

            current = GetConnectingPipes(grid, current).Where(n => !visited.Contains(n)).First();
        }

        yield return end;
    }

    private static IEnumerable<Point2> GetConnectingPipes(char[,] grid, Point2 p) =>
        grid[p.Y, p.X] switch
        {
            '|' => [p.Up, p.Down],
            '-' => [p.Left, p.Right],
            'L' => [p.Up, p.Right],
            'J' => [p.Up, p.Left],
            '7' => [p.Left, p.Down],
            'F' => [p.Right, p.Down],
            _ => throw new NotImplementedException()
        };

    private static char GetStartType(char[,] grid, Point2 start)
    {
        var bounds = grid.Bounds();
        var pipes = start
            .GetNeighbors()
            .Where(n => bounds.Contains(n) && grid[n.Y, n.X] != '.')
            .Select(n => (n, GetConnectingPipes(grid, n)))
            .Where(l => l.Item2.Contains(start)).Select(l => l.Item1)
            .ToHashSet();

        var neighbors = start.GetNeighbors().ToList();
        var bits = 0;
        for (var i = 0; i < 4; i++)
            if (pipes.Contains(neighbors[i]))
                bits |= 1 << i;

        return bits switch
        {
            5 => 'L',
            9 => '|',
            3 => 'J',
            12 => 'F',
            6 => '-',
            10 => '7',
            _ => throw new NotImplementedException()
        };
    }
}