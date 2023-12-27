using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2023.Challenges;

[Challenge(10)]
public class Challenge10
{
    private readonly IInputReader _inputReader;

    public Challenge10(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(10);
        var start = grid.FindPosition(c => c == 'S');
        grid[start.Y, start.X] = GetStartType(grid, start);
        var bfs = new BreadthFirstSearch<Point2>(n => GetConnectingPipes(grid, n));
        return bfs.FloodFill(start).MaxBy(x => x.Distance).Distance.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(10);
        var start = grid.FindPosition(c => c == 'S');
        grid[start.Y, start.X] = GetStartType(grid, start);
        var bfs = new BreadthFirstSearch<Point2>(n => GetConnectingPipes(grid, n));

        var pipeSegments = bfs.FloodFill(start).Select(x => x.Value).ToList();

        // Loop over every row and column and count how many vertical borders are crossed.
        // A point is inside the polygon when the amount of crossed borders is uneven.
        var inside = 0;
        var prev = '.';
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            var bordersCrossed = 0;
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                var p = new Point2(x, y);
                if (pipeSegments.Contains(p))
                {
                    if (grid[y, x] == '7' && prev == 'L') // Special case L7 does not cross borders twice
                    {
                        prev = grid[y, x];
                        continue;
                    }

                    if (grid[y, x] == 'J' && prev == 'F') // Special case FJ does not cross borders twice
                    {
                        prev = grid[y, x];
                        continue;
                    }

                    if (grid[y, x] == '-') // Special case - does not cross any border
                        continue;

                    prev = grid[y, x];
                    bordersCrossed++;
                    continue;
                }

                if (bordersCrossed % 2 == 1)
                    inside++;
            }
        }

        return inside.ToString();
    }

    private Dictionary<Point2, int> GetLoop(char[,] grid, Point2 start)
    {
        var queue = new Queue<Point2>();
        var visited = new Dictionary<Point2, int> {{start, 0}};
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var currentDistance = visited[currentNode];

            foreach (var adjacent in GetConnectingPipes(grid, currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, currentDistance + 1);
                queue.Enqueue(adjacent);
            }
        }

        return visited;
    }

    private static IEnumerable<Point2> GetConnectingPipes(char[,] grid, Point2 p) =>
        grid[p.Y, p.X] switch
        {
            '|' => new[] {p.Up, p.Down},
            '-' => new[] {p.Left, p.Right},
            'L' => new[] {p.Up, p.Right},
            'J' => new[] {p.Up, p.Left},
            '7' => new[] {p.Left, p.Down},
            'F' => new[] {p.Right, p.Down},
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