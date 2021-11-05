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
            var startNode = FindStartPoint(maze);

            var allKeys = FindAllKeys(maze);
            int keyTotal = allKeys.Keys.Sum(c => 1 << (c - 'a'));

            var start = new State(startNode, 0);

            var bfs = new BreadthFirstSearch<State>(GetNeighbors);
            if (bfs.TryPath(start, s => s.ObtainedKeys == keyTotal, out State[] path))
                return path.Length.ToString();

            return null;
        }

        private IEnumerable<State> GetNeighbors(State state)
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

        private record State(Point2 Location, int ObtainedKeys);

        [Part2]
        public string Part2()
        {
            var startNode = FindStartPoint(maze);
            UpdateMazeForPart2(startNode);
            PrintMaze();

            var allKeys = FindAllKeys(maze);
            int keyTotal = allKeys.Keys.Sum(c => 1 << (c - 'a'));

            Point2 p0 = startNode - new Point2(-1, -1);
            Point2 p1 = startNode - new Point2(1, -1);
            Point2 p2 = startNode - new Point2(-1, 1);
            Point2 p3 = startNode - new Point2(1, 1);



            return null;
        }

        private IEnumerable<State2> GetNeighbors2(State2 state)
        {
            for (int i = 0; i < state.Locations.Length; i++)
            {
                Point2 location = state.Locations[i];
                foreach (var neighbor in location.GetNeighbors().Where(p => p.X == location.X || p.Y == location.Y))
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
                        Point2[] newLocs = new Point2[4];
                        Array.Copy(state.Locations, newLocs, 4);
                        newLocs[i] = neighbor;
                        yield return new State2(newLocs, keys);
                    }
                }
            }
        }

        private record State2(Point2[] Locations, int ObtainedKeys);

        private void PrintMaze()
        {
            for (int y = 0; y < 81; y++)
            {
                for (int x = 0; x < 81; x++)
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

        private Dictionary<char, Point2> FindAllKeys(char[,] map)
        {
            var dict = new Dictionary<char, Point2>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (char.IsLower(map[y, x]))
                    {
                        dict.Add(map[y, x], new Point2(x, y));
                    }
                }
            }

            return dict;
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
    }
}
