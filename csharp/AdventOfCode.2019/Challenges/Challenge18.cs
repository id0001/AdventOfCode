using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;
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
        private char[,] map;


        public Challenge18(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            var stringList = new List<string>();
            await foreach (var line in inputReader.ReadLinesAsync(0))
            {
                stringList.Add(line);
            }

            map = new char[stringList.Count, stringList[0].Length];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    map[y, x] = stringList[y][x];
                }
            }
        }

        [Part1]
        public string Part1()
        {
            var startPos = GetStartPosition(map);
            var keys = GetKeys(map).ToList();
            keys.Add(new Key(startPos, '@'));

            var doors = GetDoors(map).ToDictionary(kv => kv.Location, kv => char.ToLowerInvariant(kv.Name));

            var dsmap = GetDijkstraMap(map);

            var graph = new BidirectionalGraph<Vertex, Edge>();

            for (int i = 0; i < keys.Count; i++)
            {
                graph.AddVertex(new Vertex(keys[i].Location, char.ToLowerInvariant(keys[i].Name)));
            }

            var verts = graph.Vertices.ToList();
            for (int y = 0; y < verts.Count; y++)
            {
                for (int x = y + 1; x < verts.Count; x++)
                {
                    var s = verts[y];
                    var t = verts[x];

                    var path = Dijkstra.Path(dsmap, s.Location, t.Location);

                    var keysRequired = new List<char>();
                    for (int i = 1; i < path.Length - 1; i++)
                    {
                        if (doors.TryGetValue(path[i], out char d))
                        {
                            keysRequired.Add(d);
                        }
                    }

                    graph.AddEdge(new Edge(s, t, keysRequired.ToHashSet(), path.Length - 1));
                }
            }

            return null;
        }

        [Part2]
        public string Part2()
        {
            return null;
        }

        private sealed record Node(Vertex Vertex, int Distance, char[] KeysFound)
        {
            public bool Equals(Node other) => Vertex == other.Vertex;

            public override int GetHashCode() => Vertex.GetHashCode();
        }

        private static Node Bfs(BidirectionalGraph<Vertex, Edge> graph, char[] keysToFind)
        {
            var startVert = graph.Vertices.Single(e => e.Character == '@');
            var root = new Node(startVert, 0, new char[0]);

            var queue = new Queue<Node>();
            var explored = new HashSet<Vertex>();
            explored.Add(root);
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (node.KeysFound.Length == keysToFind.Length)
                {
                    return node;
                }

                foreach(var edge in graph.OutEdges(node.Vertex))
                {
                    if (!explored.Contains(edge.Target))
                    {

                    }
                }
            }


            throw new InvalidOperationException("No path found");
        }

        private void PrintMap()
        {
            for (int y = 0; y < 81; y++)
            {
                for (int x = 0; x < 81; x++)
                {
                    char c = map[y, x];

                    Console.ResetColor();
                    if (c != '#' && c != '.' && c != ',')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    if (c == ',')
                        Console.BackgroundColor = ConsoleColor.Red;

                    Console.Write(c);
                }

                Console.WriteLine();
            }
        }

        private static Point2 GetStartPosition(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '@')
                        return new Point2(x, y);
                }
            }

            return Point2.Zero;
        }

        private static IEnumerable<Key> GetKeys(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != '@' && map[y, x] != '#' && map[y, x] != '.' && char.IsLower(map[y, x]))
                        yield return new Key(new Point2(x, y), char.ToLower(map[y, x]));
                }
            }
        }

        private static IEnumerable<Door> GetDoors(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != '@' && map[y, x] != '#' && map[y, x] != '.' && char.IsUpper(map[y, x]))
                        yield return new Door(new Point2(x, y), char.ToLower(map[y, x]));
                }
            }
        }

        private IDictionary<Point2, bool> GetDijkstraMap(char[,] map)
        {
            var dict = new Dictionary<Point2, bool>();
            for (int y = 0; y < 81; y++)
            {
                for (int x = 0; x < 81; x++)
                {
                    dict.Add(new Point2(x, y), map[y, x] != '#');
                }
            }

            return dict;
        }

        private record Key(Point2 Location, char Name);

        private record Door(Point2 Location, char Name);

        private class Vertex
        {
            public Vertex(Point2 location, char character)
            {
                Location = location;
                Character = character;
            }

            public Point2 Location { get; }

            public char Character { get; }
        }

        private class Edge : Edge<Vertex>
        {
            public Edge(Vertex source, Vertex target, HashSet<char> keysRequired, int distance) : base(source, target)
            {
                KeysRequired = keysRequired;
                Distance = distance;
            }

            public HashSet<char> KeysRequired { get; }

            public int Distance { get; }
        }
    }
}
