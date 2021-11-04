using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using AdventOfCode.Lib.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(20)]
    public class Challenge20
    {
        private readonly IInputReader inputReader;
        private char[,] maze;

        public Challenge20(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            maze = await inputReader.ReadGridAsync(20);
        }

        [Part1]
        public string Part1()
        {
            var mazeData = AnalyzeMaze();
            var dijkstra = new BreadthFirstSearch<Point2>(p => GetNeighborsPart1(mazeData.Portals.Values.ToList(), p));

            if (dijkstra.TryPath(mazeData.Start.ToPoint2(), mazeData.End.ToPoint2(), out Point2[] path))
            {
                return path.Length.ToString();
            }

            return null;
        }

        private IEnumerable<Point2> GetNeighborsPart1(List<Portal> portals, Point2 p)
        {
            var portal = portals.SingleOrDefault(x => x.Inner == p || x.Outer == p);
            if (portal is object)
                yield return portal.Inner == p ? portal.Outer : portal.Inner;

            foreach (var neighbor in p.GetNeighbors().Where(o => o.X == p.X || o.Y == p.Y))
            {
                if (maze[neighbor.Y, neighbor.X] == '.')
                    yield return neighbor;
            }
        }

        [Part2]
        public string Part2()
        {
            var mazeData = AnalyzeMaze();

            var list = mazeData.Portals.Values.ToList();

            var bfs = new BreadthFirstSearch<Point3>(p => GetNeighborsPart2(list, p));
            if (bfs.TryPath(mazeData.Start, mazeData.End, out Point3[] path))
            {
                return path.Length.ToString();
            }

            return null;
        }

        private IEnumerable<Point3> GetNeighborsPart2(List<Portal> portals, Point3 p)
        {
            var p2d = p.ToPoint2();

            // Check if we're at a portal. At level 0 only inner portals are allowed.
            var portal = portals.SingleOrDefault(x => (p.Z > 0 && (x.Inner == p2d || x.Outer == p2d)) || x.Inner == p2d);

            if (portal is object)
            {
                if (portal.Inner == p2d)
                {
                    // Go down
                    if (p.Z + 1 < 30) // Set max level limit to speed up algorithm.
                        yield return new Point3(portal.Outer.X, portal.Outer.Y, p.Z + 1);
                }
                else if (p.Z > 0)
                {
                    // Go up. Not past level 0.
                    yield return new Point3(portal.Inner.X, portal.Inner.Y, p.Z - 1);
                }
            }

            foreach (var neighbor in p2d.GetNeighbors().Where(o => o.X == p2d.X || o.Y == p2d.Y))
            {
                if (maze[neighbor.Y, neighbor.X] == '.')
                    yield return new Point3(neighbor.X, neighbor.Y, p.Z);
            }
        }

        private MazeData AnalyzeMaze()
        {
            var portals = new Dictionary<string, Portal>();
            Point3 start = Point3.Zero;
            Point3 end = Point3.Zero;

            var dict = new Dictionary<string, (Point2 Point, bool Outer)>();

            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == '.')
                    {
                        var p = new Point2(x, y);
                        foreach (var neighbor in p.GetNeighbors())
                        {
                            if (char.IsUpper(maze[neighbor.Y, neighbor.X]))
                            {
                                string name = null;
                                Point2 direction = neighbor - p;
                                name = GetName(neighbor, direction);

                                if (name == "AA")
                                {
                                    start = new Point3(x, y, 0);
                                }
                                else if (name == "ZZ")
                                {
                                    end = new Point3(x, y, 0);
                                }
                                else if (dict.ContainsKey(name))
                                {
                                    var portal = dict[name].Outer ? new Portal(dict[name].Point, p) : new Portal(p, dict[name].Point);
                                    portals.Add(name, portal);
                                    dict.Remove(name);
                                }
                                else
                                {
                                    bool outer = p.X == 2 || p.X == maze.GetLength(1) - 3 || p.Y == 2 || p.Y == maze.GetLength(0) - 3;
                                    dict.Add(name, (p, outer));
                                }
                            }
                        }
                    }
                }
            }

            return new MazeData(start, end, portals);
        }

        private string GetName(Point2 p, Point2 direction)
        {
            if (direction.X > 0)
            {
                return string.Join(string.Empty, maze[p.Y, p.X], maze[p.Y, p.X + 1]);
            }
            else if (direction.X < 0)
            {
                return string.Join(string.Empty, maze[p.Y, p.X - 1], maze[p.Y, p.X]);
            }
            else if (direction.Y > 0)
            {
                return string.Join(string.Empty, maze[p.Y, p.X], maze[p.Y + 1, p.X]);
            }
            else if (direction.Y < 0)
            {
                return string.Join(string.Empty, maze[p.Y - 1, p.X], maze[p.Y, p.X]);
            }

            throw new InvalidOperationException();
        }

        private void VisualizeMaze(ISet<Point2> path)
        {
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    Console.ResetColor();
                    if (path.Contains(new Point2(x, y)))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }

                    Console.Write(maze[y, x]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private record Portal(Point2 Outer, Point2 Inner);
        private record MazeData(Point3 Start, Point3 End, Dictionary<string, Portal> Portals);
    }
}
