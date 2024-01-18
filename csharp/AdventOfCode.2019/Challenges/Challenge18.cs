using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2019.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        var maze = await inputReader.ReadGridAsync(18);

        var data = AnalyzeMaze(maze);
        var keyTotal = KeysToInt(data.Keys.Keys);

        var start = new State(data.Start, 0);

        var bfs = new BreadthFirstSearch<State>(x => GetNeighborsPart1(maze, x));
        return bfs.TryPath(start, s => s.ObtainedKeys == keyTotal, out var path) ? (path.Count() - 1).ToString() : null;
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var maze = await inputReader.ReadGridAsync(18);

        var data = AnalyzeMaze(maze);
        var keyTotal = KeysToInt(data.Keys.Keys);

        UpdateMazeForPart2(maze, data.Start);

        var p0 = data.Start - new Point2(-1, -1);
        var p1 = data.Start - new Point2(1, -1);
        var p2 = data.Start - new Point2(-1, 1);
        var p3 = data.Start - new Point2(1, 1);
        var starts = new[] {p0, p1, p2, p3};

        var vertices = GetVertices(data, starts).ToDictionary(kv => kv.Location);
        var edges = GetEdges(maze, data, vertices.Values).ToLookup(k => k.From);

        var start = new DroneState(new[] {p0, p1, p2, p3}, 0, 0);
        var astar = new AStar<DroneState>(s => GetNeighborsPart2(vertices, edges, s), (_, b) => b.Distance);
        return astar.TryPath(start, s => s.ObtainedKeys == keyTotal, out _, out var cost) ? cost.ToString() : null;
    }

    private IEnumerable<Point2> GetNeighborsSimple(char[,] maze, Point2 p)
    {
        foreach (var neighbor in p.GetNeighbors().Where(o => o.X == p.X || o.Y == p.Y))
        {
            var c = maze[neighbor.Y, neighbor.X];
            if (c != '#')
                yield return neighbor;
        }
    }

    private static IEnumerable<State> GetNeighborsPart1(char[,] maze, State state)
    {
        foreach (var neighbor in state.Location.GetNeighbors()
                     .Where(p => p.X == state.Location.X || p.Y == state.Location.Y))
        {
            var c = maze[neighbor.Y, neighbor.X];
            var keys = state.ObtainedKeys;

            if (char.IsLower(c) && !HasKey(keys, c))
                keys = AddKey(keys, c);

            if (c == '.'
                || c == '@'
                || char.IsLower(c)
                || (char.IsUpper(c) && HasKey(keys, char.ToLower(c))))
                yield return new State(neighbor, keys);
        }
    }

    private IEnumerable<DroneState> GetNeighborsPart2(IDictionary<Point2, Vertex> vertices,
        ILookup<Vertex, Edge> edges, DroneState state)
    {
        for (var i = 0; i < state.Locations.Length; i++)
        {
            var v = vertices[state.Locations[i]];
            foreach (var edge in edges[v])
                if (HasKeys(state.ObtainedKeys, edge.RequiredKeys))
                {
                    var c = edge.To.Key;
                    var keys = state.ObtainedKeys;

                    if (char.IsLower(c) && !HasKey(keys, c))
                        keys = AddKey(keys, c);

                    if (c == '.'
                        || c == '@'
                        || char.IsLower(c)
                        || (char.IsUpper(c) && HasKey(keys, char.ToLower(c))))
                    {
                        var newLocs = new Point2[4];
                        Array.Copy(state.Locations, newLocs, 4);
                        newLocs[i] = edge.To.Location;

                        yield return new DroneState(newLocs, keys, edge.Distance);
                    }
                }
        }
    }

    private IEnumerable<Vertex> GetVertices(MazeData data, Point2[] starts)
    {
        foreach (var start in starts)
            yield return new Vertex(start, '@');

        foreach (var key in data.Keys) yield return new Vertex(key.Value, key.Key);
    }

    private IEnumerable<Edge> GetEdges(char[,] maze, MazeData data, ICollection<Vertex> vertices)
    {
        var pointToDoor = data.Doors.ToDictionary(kv => kv.Value, kv => kv.Key);
        var bfs = new BreadthFirstSearch<Point2>(x => GetNeighborsSimple(maze, x));

        foreach (var v0 in vertices)
        foreach (var v1 in vertices)
        {
            if (v0 == v1)
                continue;

            if (v1.Key == '@')
                continue;

            if (!bfs.TryPath(v0.Location, t => t == v1.Location, out var path))
                continue;

            var list = path.ToList();
            var doors = KeysToInt(list.Where(p => pointToDoor.ContainsKey(p))
                .Select(p => char.ToLower(pointToDoor[p])));
            var edge = new Edge(v0, v1, doors, list.Count - 1);
            yield return edge;
        }
    }

    private static void UpdateMazeForPart2(char[,] maze, Point2 p)
    {
        for (var y = p.Y - 1; y <= p.Y + 1; y++)
        for (var x = p.X - 1; x <= p.X + 1; x++)
            if (x == p.X || y == p.Y)
                maze[y, x] = '#';
            else
                maze[y, x] = '@';
    }

    private static int AddKey(int obtainedKeys, char key)
    {
        var pos = key - 'a';
        return obtainedKeys | (1 << pos);
    }

    private static bool HasKey(int obtainedKeys, char key)
    {
        var pos = key - 'a';
        return (obtainedKeys & (1 << pos)) == 1 << pos;
    }

    private static bool HasKeys(int obtainedKeys, int requiredKeys)
    {
        return (obtainedKeys & requiredKeys) == requiredKeys;
    }

    private static int KeysToInt(IEnumerable<char> keys) => keys.Sum(key => 1 << (key - 'a'));

    private static MazeData AnalyzeMaze(char[,] maze)
    {
        var keys = new Dictionary<char, Point2>();
        var doors = new Dictionary<char, Point2>();
        var start = Point2.Zero;

        for (var y = 0; y < maze.GetLength(0); y++)
        for (var x = 0; x < maze.GetLength(1); x++)
        {
            var c = maze[y, x];
            switch (c)
            {
                case var _ when char.IsLower(c):
                    keys.Add(c, new Point2(x, y));
                    break;
                case var _ when char.IsUpper(c):
                    doors.Add(c, new Point2(x, y));
                    break;
                case '@':
                    start = new Point2(x, y);
                    break;
            }
        }

        return new MazeData(start, keys, doors);
    }

    private record Vertex(Point2 Location, char Key);

    private record Edge(Vertex From, Vertex To, int RequiredKeys, int Distance);

    private record MazeData(Point2 Start, IDictionary<char, Point2> Keys, IDictionary<char, Point2> Doors);

    private record State(Point2 Location, int ObtainedKeys);

    private class DroneState(Point2[] locations, int obtainedKeys, int distance) : IEquatable<DroneState>
    {
        public int Distance { get; } = distance;

        public Point2[] Locations { get; } = locations;

        public int ObtainedKeys { get; } = obtainedKeys;

        public bool Equals(DroneState? other) =>
            other != null && Locations.SequenceEqual(other.Locations) && ObtainedKeys == other.ObtainedKeys;

        public override bool Equals(object? obj) => obj is DroneState ds && Equals(ds);

        public override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(ObtainedKeys);
            foreach (var loc in Locations) hc.Add(loc);

            return hc.ToHashCode();
        }
    }
}