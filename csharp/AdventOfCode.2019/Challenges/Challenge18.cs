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
            var startNode = FindStartPoint(map);

            var allKeys = FindAllKeys(map);
            int keyAmount = allKeys.Count;

            var ds = new Dijkstra<Point2>(GetNeighbors);
            var edges = new List<Edge>();

            foreach (var from in allKeys.Keys)
            {
                foreach (var to in allKeys.Keys)
                {
                    if (to == '@' || from == to)
                        continue;

                    if (ds.TryPath(allKeys[from], allKeys[to], out Point2[] path))
                    {
                        int keysNeeded = 0;
                        foreach (var p in path)
                        {
                            char c = map[p.Y, p.X];
                            if (c != '#' && c != '@' && c != '.' && char.IsUpper(c))
                            {
                                keysNeeded = AddKey(keysNeeded, char.ToLower(map[p.Y, p.X]));
                            }
                        }

                        var edge = new Edge(from, to, keysNeeded, path.Length - 1);
                        edges.Add(edge);
                    }
                }
            }

            int target = 0;
            foreach (var c in allKeys.Keys)
            {
                if (c == '@')
                    continue;

                target = AddKey(target, c);
            }

            var queue = new Queue<(char, int, int)>();
            var visited = new Dictionary<(char, int), int>();

            queue.Enqueue(('@', 0, 0));
            visited[('@', 0)] = 0;

            int min = int.MaxValue;
            while (queue.Any())
            {
                var (c, keys, dist) = queue.Dequeue();



                var neighbors = edges.Where(x => x.From == c && !HasKey(keys, x.To) && HasAllKeys(x.Keys, keys));

                foreach (var neighbor in neighbors)
                {
                    int k = AddKey(keys, neighbor.To);
                    if (k == target)
                    {
                        min = Math.Min(min, dist + neighbor.Distance);
                        continue;
                    }

                    var n = (To: neighbor.To, Keys: k, Distance: dist + neighbor.Distance);

                    if (visited.TryGetValue((n.To, n.Keys), out int d))
                    {
                        if (d < n.Distance)
                            continue;
                    }

                    queue.Enqueue(n);
                    visited[(n.To, n.Keys)] = n.Distance;
                }
            }

            return min.ToString();
        }

        private IEnumerable<(Point2, int)> GetNeighbors(Point2 p)
        {
            for (int y = p.Y - 1; y <= p.Y + 1; y++)
            {
                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    if (x >= 0 && x < map.GetLength(1) && y >= 0 && y < map.GetLength(0))
                    {
                        if (map[y, x] != '#')
                            yield return (new Point2(x, y), 1);
                    }
                }
            }
        }

        private record Edge(char From, char To, int Keys, int Distance);

        [Part2]
        public string Part2()
        {
            return null;
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

        private Point2 FindStartPoint(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '@')
                    {
                        return new Point2(x, y);
                    }
                }
            }

            return default;
        }

        private Dictionary<char, Point2> FindAllKeys(char[,] map)
        {
            var dict = new Dictionary<char, Point2>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != '#' && (map[y, x] == '@' || char.IsLower(map[y, x])))
                    {
                        dict.Add(map[y, x], new Point2(x, y));
                    }
                }
            }

            return dict;
        }

        private Dictionary<Point2, bool> GetDijkstraMap(char[,] map, ISet<char> obtainedKeys)
        {
            Dictionary<Point2, bool> map2 = new Dictionary<Point2, bool>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '.' || map[y, x] == '@' || char.IsLower(map[y, x]) || (char.IsUpper(map[y, x]) && obtainedKeys.Contains(char.ToLower(map[y, x]))))
                    {
                        map2.Add(new Point2(x, y), true);
                    }
                    else
                    {
                        map2.Add(new Point2(x, y), false);
                    }
                }
            }

            return map2;
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

        private bool HasAllKeys(int keysNeeded, int obtainedKeys)
        {
            return (obtainedKeys & keysNeeded) == keysNeeded;
        }
    }
}
