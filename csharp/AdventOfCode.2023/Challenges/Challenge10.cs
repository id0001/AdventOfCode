using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

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
        var start = FindStart(grid);
        grid[start.Y, start.X] = GetStartType(grid, start);
        return GetLoop(grid, start).Values.Max().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(10);
        var start = FindStart(grid);
        grid[start.Y, start.X] = GetStartType(grid, start);

        var pipeSegments = GetLoop(grid, start).Keys.ToHashSet();
        
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

                    if(grid[y,x] == '-') // Special case - does not cross any border
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
                
                visited.Add(adjacent, currentDistance+1);
                queue.Enqueue(adjacent);
            }
        }

        return visited;
    }

    private static IEnumerable<Point2> GetConnectingPipes(char[,] grid, Point2 p) =>
        grid[p.Y, p.X] switch
        {
            '|' => new[] {new Point2(p.X, p.Y - 1), new Point2(p.X, p.Y + 1)},
            '-' => new[] {new Point2(p.X - 1, p.Y), new Point2(p.X + 1, p.Y)},
            'L' => new[] {new Point2(p.X, p.Y - 1), new Point2(p.X + 1, p.Y)},
            'J' => new[] {new Point2(p.X - 1, p.Y), new Point2(p.X, p.Y - 1)},
            '7' => new[] {new Point2(p.X - 1, p.Y), new Point2(p.X, p.Y + 1)},
            'F' => new[] {new Point2(p.X, p.Y + 1), new Point2(p.X + 1, p.Y)},
            _ => throw new NotImplementedException()
        };

    private static char GetStartType(char[,] grid, Point2 start)
    {
        var bounds = new Rectangle(0, 0, grid.GetLength(1), grid.GetLength(0));
        var pipes = start
            .GetNeighbors()
            .Where(n => bounds.Contains(n) && grid[n.Y, n.X] != '.')
            .Select(n => (n, GetConnectingPipes(grid, n)))
            .Where(l => l.Item2.Contains(start)).Select(l => l.Item1)
            .ToHashSet();

        var neighbors = start.GetNeighbors().ToList();
        var bits = 0;
        for (var i = 0; i < 4; i++)
        {
            if (pipes.Contains(neighbors[i]))
                bits |= 1 << i;
        }

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

    private static Point2 FindStart(char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
            for (var x = 0; x < grid.GetLength(1); x++)
                if (grid[y, x] == 'S')
                    return new Point2(x, y);

        throw new InvalidOperationException("Starting point not found");
    }
}
