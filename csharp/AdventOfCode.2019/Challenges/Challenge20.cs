using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2019.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader InputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        var maze = await InputReader.ReadGridAsync(20);

        var mazeData = AnalyzeMaze(maze);
        var bfs = new BreadthFirstSearch<Point2>(p => GetNeighborsPart1(maze, mazeData.Portals.Values.ToList(), p));

        return bfs.TryPath(mazeData.Start.ToPoint2(), t => t == mazeData.End.ToPoint2(), out var path)
            ? (path.Count() - 1).ToString()
            : null;
    }

    private static IEnumerable<Point2> GetNeighborsPart1(char[,] maze, IEnumerable<Portal> portals, Point2 p)
    {
        var portal = portals.SingleOrDefault(x => x.Inner == p || x.Outer == p);
        if (portal != null)
            yield return portal.Inner == p ? portal.Outer : portal.Inner;

        foreach (var neighbor in p.GetNeighbors().Where(o => o.X == p.X || o.Y == p.Y))
            if (maze[neighbor.Y, neighbor.X] == '.')
                yield return neighbor;
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var maze = await InputReader.ReadGridAsync(20);

        var mazeData = AnalyzeMaze(maze);

        var list = mazeData.Portals.Values.ToList();

        var bfs = new BreadthFirstSearch<Point3>(p => GetNeighborsPart2(maze, list, p));
        return bfs.TryPath(mazeData.Start, t => t == mazeData.End, out var path) ? (path.Count() - 1).ToString() : null;
    }

    private static IEnumerable<Point3> GetNeighborsPart2(char[,] maze, IEnumerable<Portal> portals, Point3 p)
    {
        var p2d = p.ToPoint2();

        // Check if we're at a portal. At level 0 only inner portals are allowed.
        var portal = portals.SingleOrDefault(x => (p.Z > 0 && (x.Inner == p2d || x.Outer == p2d)) || x.Inner == p2d);

        if (portal != null)
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
            if (maze[neighbor.Y, neighbor.X] == '.')
                yield return new Point3(neighbor.X, neighbor.Y, p.Z);
    }

    private static MazeData AnalyzeMaze(char[,] maze)
    {
        var portals = new Dictionary<string, Portal>();
        var start = Point3.Zero;
        var end = Point3.Zero;

        var dict = new Dictionary<string, (Point2 Point, bool Outer)>();

        for (var y = 0; y < maze.GetLength(0); y++)
        for (var x = 0; x < maze.GetLength(1); x++)
        {
            if (maze[y, x] != '.') continue;

            var p = new Point2(x, y);
            foreach (var neighbor in p.GetNeighbors())
                if (char.IsUpper(maze[neighbor.Y, neighbor.X]))
                {
                    var direction = neighbor - p;
                    var name = GetName(maze, neighbor, direction);

                    switch (name)
                    {
                        case "AA":
                            start = new Point3(x, y, 0);
                            break;
                        case "ZZ":
                            end = new Point3(x, y, 0);
                            break;
                        default:
                        {
                            if (dict.ContainsKey(name))
                            {
                                var portal = dict[name].Outer
                                    ? new Portal(dict[name].Point, p)
                                    : new Portal(p, dict[name].Point);
                                portals.Add(name, portal);
                                dict.Remove(name);
                            }
                            else
                            {
                                var outer = p.X == 2 || p.X == maze.GetLength(1) - 3 || p.Y == 2 ||
                                            p.Y == maze.GetLength(0) - 3;
                                dict.Add(name, (p, outer));
                            }

                            break;
                        }
                    }
                }
        }

        return new MazeData(start, end, portals);
    }

    private static string GetName(char[,] maze, Point2 p, Point2 direction)
    {
        switch (direction.X)
        {
            case > 0:
                return string.Join(string.Empty, maze[p.Y, p.X], maze[p.Y, p.X + 1]);
            case < 0:
                return string.Join(string.Empty, maze[p.Y, p.X - 1], maze[p.Y, p.X]);
            default:
            {
                switch (direction.Y)
                {
                    case > 0:
                        return string.Join(string.Empty, maze[p.Y, p.X], maze[p.Y + 1, p.X]);
                    case < 0:
                        return string.Join(string.Empty, maze[p.Y - 1, p.X], maze[p.Y, p.X]);
                }

                break;
            }
        }

        throw new InvalidOperationException();
    }

    private record Portal(Point2 Outer, Point2 Inner);

    private record MazeData(Point3 Start, Point3 End, Dictionary<string, Portal> Portals);
}