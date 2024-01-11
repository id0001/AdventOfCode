using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;
using AdventOfCode2019.IntCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var space = await MapSpaceAsync();
        var target = space.Single(x => x.Value == 2).Key;

        var astar = new AStar<Point2>(x => GetNeighbors(space, x), (_, _) => 1);
        return astar.TryPath(Point2.Zero, p => p == target, out _, out var distance) ? distance.ToString() : "-1";
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var fillMap = new HashSet<Point2>();
        var lockedMap = new HashSet<Point2>();

        var space = await MapSpaceAsync();
        var target = space.Single(x => x.Value == 2).Key;

        fillMap.Add(target);
        space[target] = 3;

        var minutes = 0;
        while (fillMap.Any())
        {
            var filledASpace = false;
            var points = fillMap.ToHashSet();

            foreach (var point in points)
            {
                fillMap.Remove(point);
                lockedMap.Add(point);

                for (var y = point.Y - 1; y <= point.Y + 1; y++)
                for (var x = point.X - 1; x <= point.X + 1; x++)
                {
                    if (!((x == point.X) ^ (y == point.Y))) continue;
                    var neighbor = new Point2(x, y);

                    if (!space.ContainsKey(neighbor) || space[neighbor] == 0)
                        continue;

                    if (points.Contains(neighbor) || lockedMap.Contains(neighbor)) continue;
                    fillMap.Add(neighbor);
                    space[neighbor] = 3;
                    filledASpace = true;
                }
            }

            if (filledASpace)
                minutes++;
        }

        return minutes.ToString();
    }

    private static IEnumerable<Point2> GetNeighbors(IDictionary<Point2, int> visited, Point2 p)
    {
        return p.GetNeighbors().Where(neighbor => visited[neighbor] != 0);
    }

    private async Task<IDictionary<Point2, int>> MapSpaceAsync()
    {
        var program = await InputReader.ReadLineAsync<long>(15, ',').ToArrayAsync();

        var visited = new Dictionary<Point2, int>();
        var currentLocation = Point2.Zero;
        var nextLocation = Point2.Zero;

        var astar = new AStar<Point2>(x => GetNeighbors(visited, x), (_, _) => 1);

        visited.Add(currentLocation, 1);

        // moves to perform.
        var moves = new Queue<Point2>();
        var discovery = new Stack<Move>();

        ExpandDiscovery(discovery, visited, currentLocation);

        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o =>
        {
            visited.TryAdd(nextLocation, (int) o);

            switch (o)
            {
                case 0:
                    break;
                case 1:
                case 2:
                    currentLocation = nextLocation;
                    ExpandDiscovery(discovery, visited, currentLocation);
                    break;
            }
        });

        cpu.RegisterInput(() =>
        {
            if (moves.Any())
            {
                nextLocation = moves.Dequeue();
            }
            else if (discovery.Count == 0)
            {
                cpu.Halt();
                return;
            }
            else
            {
                var nextMove = discovery.Peek();
                nextLocation = nextMove.Target;
                if (!NextTo(currentLocation, nextLocation))
                {
                    if (!astar.TryPath(currentLocation, t => t == nextMove.Source, out var path, out _))
                        throw new InvalidOperationException();

                    foreach (var p in path.Skip(1))
                        moves.Enqueue(p);

                    nextLocation = moves.Dequeue();
                }
                else
                {
                    discovery.Pop();
                }
            }

            var dir = Direction(currentLocation, nextLocation);
            cpu.WriteInput(dir);
        });

        await cpu.StartAsync();
        return visited;
    }

    private static int Direction(Point2 source, Point2 target)
    {
        var delta = target - source;

        return delta switch
        {
            (0, -1) => 1,
            (0, 1) => 2,
            (-1, 0) => 3,
            (1, 0) => 4,
            _ => throw new NotImplementedException()
        };
    }

    private static bool NextTo(Point2 a, Point2 b)
    {
        var y = Math.Abs(a.Y - b.Y);
        var x = Math.Abs(a.X - b.X);
        return (x == 1 && y == 0) || (x == 0 && y == 1);
    }

    private static void ExpandDiscovery(Stack<Move> discovery, IReadOnlyDictionary<Point2, int> visited, Point2 source)
    {
        var moves = new[]
        {
            new Point2(source.X, source.Y - 1), new Point2(source.X, source.Y + 1), new Point2(source.X - 1, source.Y),
            new Point2(source.X + 1, source.Y)
        };

        for (var i = 0; i < 4; i++)
            if (!visited.ContainsKey(moves[i]))
                discovery.Push(new Move(source, moves[i]));
    }

    private record Move(Point2 Source, Point2 Target);
}