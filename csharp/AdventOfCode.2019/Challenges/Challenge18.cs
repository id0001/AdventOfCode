using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode.Lib.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(18)]
    public class Challenge18
    {
        private readonly IInputReader inputReader;
        private char[,] maze;


        public Challenge18(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            maze = await inputReader.ReadGridAsync(18);
        }

        [Part1]
        public string Part1()
        {
            var data = AnalyzeMaze();
            int keyTotal = KeysToInt(data.Keys.Keys);

            var start = new State(data.Start, 0);

            var bfs = new BreadthFirstSearch<State>(GetNeighborsPart1);
            if (bfs.TryPath(start, s => s.ObtainedKeys == keyTotal, out State[] path))
                return path.Length.ToString();

            return null;
        }

        [Part2]
        public string Part2()
        {
            var data = AnalyzeMaze();
            int keyTotal = KeysToInt(data.Keys.Keys);

            UpdateMazeForPart2(data.Start);

            Point2 p0 = data.Start - new Point2(-1, -1);
            Point2 p1 = data.Start - new Point2(1, -1);
            Point2 p2 = data.Start - new Point2(-1, 1);
            Point2 p3 = data.Start - new Point2(1, 1);
            var starts = new[] { p0, p1, p2, p3 };

            var vertices = GetVertices(data, starts).ToDictionary(kv => kv.Location);
            var edges = GetEdges(data, vertices.Values).ToLookup(k => k.From);

            var start = new DroneState(new Point2[] { p0, p1, p2, p3 }, 0);
            var bfs = new Dijkstra<DroneState>(s => GetNeighborsPart2(vertices, edges, s));
            if (bfs.TryPath(start, s => s.ObtainedKeys == keyTotal, out DroneState[] path, out int length))
            {
                return length.ToString();
            }

            return null;
        }

        private IEnumerable<Point2> GetNeighborsSimple(Point2 p)
        {
            foreach (var neighbor in p.GetNeighbors().Where(o => o.X == p.X || o.Y == p.Y))
            {
                char c = maze[neighbor.Y, neighbor.X];
                if (c != '#')
                    yield return neighbor;
            }
        }

        private IEnumerable<State> GetNeighborsPart1(State state)
        {
            foreach (var neighbor in state.Location.GetNeighbors().Where(p => p.X == state.Location.X || p.Y == state.Location.Y))
            {
                char c = maze[neighbor.Y, neighbor.X];
                int keys = state.ObtainedKeys;

                if (char.IsLower(c) && !HasKey(keys, c))
                    keys = AddKey(keys, c);

                if (c == '.'
                    || c == '@'
                    || (char.IsLower(c))
                    || (char.IsUpper(c) && HasKey(keys, char.ToLower(c))))
                {
                    yield return new State(neighbor, keys);
                }
            }
        }

        private IEnumerable<(DroneState, int)> GetNeighborsPart2(IDictionary<Point2, Vertex> vertices, ILookup<Vertex, Edge> edges, DroneState state)
        {
            for (int i = 0; i < state.Locations.Length; i++)
            {
                Vertex v = vertices[state.Locations[i]];
                foreach (var edge in edges[v])
                {
                    if (HasKeys(state.ObtainedKeys, edge.RequiredKeys))
                    {
                        char c = edge.To.Key;
                        int keys = state.ObtainedKeys;

                        if (char.IsLower(c) && !HasKey(keys, c))
                            keys = AddKey(keys, c);

                        if (c == '.'
                            || c == '@'
                            || (char.IsLower(c))
                            || (char.IsUpper(c) && HasKey(keys, char.ToLower(c))))
                        {
                            Point2[] newLocs = new Point2[4];
                            Array.Copy(state.Locations, newLocs, 4);
                            newLocs[i] = edge.To.Location;

                            yield return (new DroneState(newLocs, keys), edge.Distance);
                        }
                    }
                }
            }
        }

        private record Vertex(Point2 Location, char Key);

        private record Edge(Vertex From, Vertex To, int RequiredKeys, int Distance);

        private IEnumerable<Vertex> GetVertices(MazeData data, Point2[] starts)
        {
            foreach (var start in starts)
                yield return new Vertex(start, '@');

            foreach (var key in data.Keys)
            {
                yield return new Vertex(key.Value, key.Key);
            }
        }

        private IEnumerable<Edge> GetEdges(MazeData data, IEnumerable<Vertex> vertices)
        {
            var pointToDoor = data.Doors.ToDictionary(kv => kv.Value, kv => kv.Key);
            var bfs = new BreadthFirstSearch<Point2>(GetNeighborsSimple);

            foreach (Vertex v0 in vertices)
            {
                foreach (Vertex v1 in vertices)
                {
                    if (v0 == v1)
                        continue;

                    if (v1.Key == '@')
                        continue;

                    if (bfs.TryPath(v0.Location, v1.Location, out Point2[] path))
                    {
                        int doors = KeysToInt(path.Where(p => pointToDoor.ContainsKey(p)).Select(p => char.ToLower(pointToDoor[p])));
                        var edge = new Edge(v0, v1, doors, path.Length);
                        yield return edge;
                    }
                }
            }
        }

        private void PrintMaze()
        {
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    char c = maze[y, x];

                    Console.ResetColor();

                    Console.ForegroundColor = c switch
                    {
                        '#' => ConsoleColor.DarkYellow,
                        '@' => ConsoleColor.Cyan,
                        _ when char.IsLower(c) => ConsoleColor.Green,
                        _ when char.IsUpper(c) => ConsoleColor.Red,
                        _ => Console.ForegroundColor
                    };

                    Console.Write(c);
                }

                Console.WriteLine();
            }
        }

        private int GetTotalDistance(IDictionary<Point2, Vertex> vertices, ILookup<Vertex,Edge> edges, Point2[] previous, DroneState[] path)
        {
            int totalDistance = 0;

            foreach(var state in path)
            {
                for(int i = 0; i < 4; i++)
                {
                    Vertex v0 = vertices[previous[i]];
                    Vertex v1 = vertices[state.Locations[i]];

                    if (v0 == v1)
                        continue;

                    var edge = edges[v0].Single(e => e.To == v1);
                    totalDistance += edge.Distance;
                }

                previous = state.Locations;
            }

            return totalDistance;
        }

        private void UpdateMazeForPart2(Point2 p)
        {
            for (int y = p.Y - 1; y <= p.Y + 1; y++)
            {
                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    if (x == p.X || y == p.Y)
                    {
                        maze[y, x] = '#';
                    }
                    else
                    {
                        maze[y, x] = '@';
                    }
                }
            }
        }

        private static int AddKey(int obtainedKeys, char key)
        {
            int pos = key - 'a';
            return obtainedKeys | 1 << pos;
        }

        private bool HasKey(int obtainedKeys, char key)
        {
            int pos = key - 'a';
            return (obtainedKeys & (1 << pos)) == (1 << pos);
        }

        private bool HasKeys(int obtainedKeys, int requiredKeys)
        {
            return (obtainedKeys & requiredKeys) == requiredKeys;
        }

        private static int KeysToInt(IEnumerable<char> keys)
        {
            int k = 0;
            foreach (var key in keys)
                k += 1 << (key - 'a');

            return k;
        }
        private string IntToKeys(int keys)
        {
            string s = string.Empty;
            for (int i = 0; i < 26; i++)
            {
                if (HasKey(keys, (char)('a' + i)))
                    s += (char)('a' + i);
            }

            return s;
        }


        private MazeData AnalyzeMaze()
        {
            var keys = new Dictionary<char, Point2>();
            var doors = new Dictionary<char, Point2>();
            Point2 start = Point2.Zero;

            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    char c = maze[y, x];
                    switch (c)
                    {
                        case char key when char.IsLower(key):
                            keys.Add(key, new Point2(x, y));
                            break;
                        case char door when char.IsUpper(door):
                            doors.Add(door, new Point2(x, y));
                            break;
                        case '@':
                            start = new Point2(x, y);
                            break;
                        default:
                            break;
                    }
                }
            }

            return new MazeData(start, keys, doors);
        }

        private record MazeData(Point2 Start, IDictionary<char, Point2> Keys, IDictionary<char, Point2> Doors);

        private record State(Point2 Location, int ObtainedKeys);

        private class DroneState : IEquatable<DroneState>
        {
            public DroneState(Point2[] locations, int obtainedKeys)
            {
                Locations = locations;
                ObtainedKeys = obtainedKeys;
            }

            public Point2[] Locations { get; }

            public int ObtainedKeys { get; }

            public bool Equals(DroneState other) => Locations.SequenceEqual(other.Locations) && ObtainedKeys == other.ObtainedKeys;

            public override bool Equals(object obj) => obj is DroneState ds && Equals(ds);

            public override int GetHashCode()
            {
                var hc = new HashCode();
                hc.Add(ObtainedKeys);
                foreach (var loc in Locations)
                {
                    hc.Add(loc);
                }

                return hc.ToHashCode();
            }
        }
    }
}
