using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode.Lib.Pathfinding;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(15)]
    public class Challenge15
    {
        private readonly IInputReader inputReader;
        private long[] program;

        public Challenge15(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            program = await inputReader.ReadLineAsync<long>(15, ',').ToArrayAsync();

        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var space = await MapSpaceAsync();
            var target = space.Single(x => x.Value == 2).Key;

            var dijkstra = new Dijkstra<Point2>(x => GetNeighbors(space, x));
            if (dijkstra.TryPath(Point2.Zero, target, out DijkstraResult<Point2> result))
            {
                return result.Cost.ToString();
            }

            return "-1";
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

            int minutes = 0;
            while (fillMap.Any())
            {
                bool filledASpace = false;
                var points = fillMap.ToHashSet();

                foreach (var point in points)
                {
                    fillMap.Remove(point);
                    lockedMap.Add(point);

                    for (int y = point.Y - 1; y <= point.Y + 1; y++)
                    {
                        for (int x = point.X - 1; x <= point.X + 1; x++)
                        {
                            if (x == point.X ^ y == point.Y)
                            {
                                Point2 neighbor = new Point2(x, y);

                                if (!space.ContainsKey(neighbor) || space[neighbor] == 0)
                                    continue;

                                if (!points.Contains(neighbor) && !lockedMap.Contains(neighbor))
                                {
                                    fillMap.Add(neighbor);
                                    space[neighbor] = 3;
                                    filledASpace = true;
                                }
                            }
                        }
                    }
                }

                if (filledASpace)
                    minutes++;
            }

            return minutes.ToString();
        }

        private IEnumerable<(Point2, int)> GetNeighbors(IDictionary<Point2, int> visited, Point2 p)
        {
            foreach (var neighbor in p.GetNeighbors())
            {
                if (visited[neighbor] != 0)
                    yield return (neighbor, 1);
            }
        }

        private async Task<IDictionary<Point2, int>> MapSpaceAsync()
        {
            var visited = new Dictionary<Point2, int>();
            Point2 currentLocation = Point2.Zero;
            Point2 nextLocation = Point2.Zero;
            var dijkstra = new Dijkstra<Point2>(x => GetNeighbors(visited, x));

            visited.Add(currentLocation, 1);

            // moves to perform.
            var moves = new Queue<Point2>();
            var discovery = new Stack<Move>();

            ExpandDiscovery(discovery, visited, currentLocation);

            var cpu = new Cpu();
            cpu.SetProgram(program);
            cpu.RegisterOutput(o =>
            {
                if (!visited.ContainsKey(nextLocation))
                    visited.Add(nextLocation, (int)o);

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
                        if (!dijkstra.TryPath(currentLocation, nextMove.Source, out DijkstraResult<Point2> result))
                            throw new InvalidOperationException();

                        foreach (var p in result.Path)
                            moves.Enqueue(p);

                        nextLocation = moves.Dequeue();
                    }
                    else
                    {
                        discovery.Pop();
                    }
                }

                int dir = Direction(currentLocation, nextLocation);
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
            int y = Math.Abs(a.Y - b.Y);
            int x = Math.Abs(a.X - b.X);
            return (x == 1 && y == 0) || (x == 0 && y == 1);
        }

        private static void ExpandDiscovery(Stack<Move> discovery, Dictionary<Point2, int> visited, Point2 source)
        {
            Point2[] moves = new[] { new Point2(source.X, source.Y - 1), new Point2(source.X, source.Y + 1), new Point2(source.X - 1, source.Y), new Point2(source.X + 1, source.Y) };

            for (int i = 0; i < 4; i++)
            {
                if (!visited.ContainsKey(moves[i]))
                    discovery.Push(new Move(source, moves[i]));
            }
        }

        private static IDictionary<Point2, bool> ConvertSpaceForPathFinding(IDictionary<Point2, int> visited) => visited.Select(x => new KeyValuePair<Point2, bool>(x.Key, x.Value != 0)).ToDictionary(kv => kv.Key, kv => kv.Value);

        private record Move(Point2 Source, Point2 Target);
    }
}
