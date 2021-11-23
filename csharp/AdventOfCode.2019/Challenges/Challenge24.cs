using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(24)]
    public class Challenge24
    {
        private readonly IInputReader inputReader;
        private char[] data;

        public Challenge24(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = new char[5 * 5];
            int i = 0;
            await foreach (string line in inputReader.ReadLinesAsync(24))
            {
                foreach (var c in line)
                {
                    data[i++] = c;
                }
            }
        }

        [Part1]
        public string Part1()
        {
            char[][] automata = new char[2][];
            automata[0] = data;
            automata[1] = new char[5 * 5];

            int current = 0; // current index
            int next = 1; // next index
            int gen = 0;

            //PrintGrid(gen, automata[current]);

            HashSet<string> history = new HashSet<string>();
            string dataString = new string(automata[current]);
            while (!history.Contains(dataString))
            {
                history.Add(dataString);

                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        var p = new Point2(x, y); // current point
                        int i = (y * 5) + x;
                        int bugCount = 0;

                        foreach (var neighbor in p.GetNeighbors())
                        {
                            if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= 5 || neighbor.Y >= 5)
                                continue;

                            if (!(neighbor.X == x ^ neighbor.Y == y))
                                continue;

                            int ni = (neighbor.Y * 5) + neighbor.X;

                            if (automata[current][ni] == '#')
                                bugCount++;
                        }

                        if (automata[current][i] == '#' && bugCount != 1)
                        {
                            automata[next][i] = '.';
                        }
                        else if (automata[current][i] == '.' && bugCount >= 1 && bugCount <= 2)
                        {
                            automata[next][i] = '#';
                        }
                        else
                        {
                            automata[next][i] = automata[current][i];
                        }
                    }
                }

                current = next;
                next = MathEx.Mod(next + 1, 2);
                dataString = new string(automata[current]);
                gen++;
                //PrintGrid(gen, automata[current]);
            }

            return GetBiodiversity(automata[current]).ToString();
        }

        [Part2]
        public string Part2()
        {
            int totalGen = 200;
            int upperBound = 1;
            int lowerBound = -1;

            IDictionary<int, char[]> current = new Dictionary<int, char[]>();
            current.Add(0, data);
            current.Add(-1, EmptyGrid());
            current.Add(1, EmptyGrid());

            string emptyGridString = new string(EmptyGrid());

            //PrintGrid(0, current, current);

            for (int gen = 0; gen < totalGen; gen++)
            {
                IDictionary<int, char[]> next = new Dictionary<int, char[]>();

                for (int level = lowerBound; level <= upperBound; level++)
                {
                    if (!next.ContainsKey(level))
                        next.Add(level, EmptyGrid());

                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            if (x == 2 && y == 2)
                                continue;

                            Point3 currentPoint = new Point3(x, y, level);
                            int bugCount = 0;

                            foreach (var neighbor in GetAdjecent(currentPoint))
                            {
                                if (!current.ContainsKey(neighbor.Z))
                                    current.Add(neighbor.Z, EmptyGrid());

                                if (current[neighbor.Z][neighbor.Y * 5 + neighbor.X] == '#')
                                    bugCount++;
                            }

                            if (current[level][y * 5 + x] == '#' && bugCount != 1)
                            {
                                next[level][y * 5 + x] = '.';
                            }
                            else if (current[level][y * 5 + x] == '.' && bugCount >= 1 && bugCount <= 2)
                            {
                                next[level][y * 5 + x] = '#';
                            }
                            else
                            {
                                next[level][y * 5 + x] = current[level][y * 5 + x];
                            }
                        }
                    }
                }

                lowerBound--;
                upperBound++;

                // reduce levels
                if (!current.ContainsKey(lowerBound + 1) || new string(current[lowerBound + 1]) == emptyGridString)
                {
                    current.Remove(lowerBound);
                    lowerBound++;
                }

                if (!current.ContainsKey(upperBound - 1) || new string(current[upperBound - 1]) == emptyGridString)
                {
                    current.Remove(upperBound);
                    upperBound--;
                }

                //PrintGrid(gen+1, current, next);
                current = next;
            }

            int bc = 0;
            foreach (var kv in current)
            {
                bc += kv.Value.Count(x => x == '#');
            }

            return bc.ToString();
        }

        private IEnumerable<Point3> GetAdjecent(Point3 p)
        {
            var p2 = new Point2(p.X, p.Y);
            foreach (var neighbor in p2.GetNeighbors())
            {
                if (!(neighbor.X == p2.X ^ neighbor.Y == p2.Y)) // No diagonals
                    continue;

                // Handle outer recursion
                if (neighbor.X == -1)
                {
                    yield return new Point3(1, 2, p.Z - 1);
                }
                else if (neighbor.X == 5)
                {
                    yield return new Point3(3, 2, p.Z - 1);
                }
                else if (neighbor.Y == -1)
                {
                    yield return new Point3(2, 1, p.Z - 1);
                }
                else if (neighbor.Y == 5)
                {
                    yield return new Point3(2, 3, p.Z - 1);
                }
                else if (neighbor.X == 2 && neighbor.Y == 2)
                {
                    if (p.X == 1)
                    {
                        for (int y = 0; y < 5; y++)
                            yield return new Point3(0, y, p.Z + 1);
                    }
                    else if (p.X == 3)
                    {
                        for (int y = 0; y < 5; y++)
                            yield return new Point3(4, y, p.Z + 1);
                    }
                    else if (p.Y == 1)
                    {
                        for (int x = 0; x < 5; x++)
                            yield return new Point3(x, 0, p.Z + 1);
                    }
                    else if (p.Y == 3)
                    {
                        for (int x = 0; x < 5; x++)
                            yield return new Point3(x, 4, p.Z + 1);
                    }
                }
                else
                {
                    yield return new Point3(neighbor.X, neighbor.Y, p.Z);
                }
            }
        }

        private int GetBiodiversity(char[] data)
        {
            int diversity = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '#')
                    diversity += (int)Math.Pow(2, i);
            }

            return diversity;
        }

        private void PrintGrid(int gen, char[] data)
        {
            Console.WriteLine($"Gen: {gen}");
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Console.Write(data[y * 5 + x]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private void PrintGrid(int gen, IDictionary<int, char[]> current, IDictionary<int, char[]> next)
        {
            Console.WriteLine($"Gen: {gen}");
            Console.WriteLine();

            foreach(var kv in next.OrderBy(kv => kv.Key))
            {
                string s = $"L: {kv.Key}".PadRight(7);
                Console.Write(s);
            }

            Console.WriteLine();

            for(int y = 0; y < 5; y++)
            {
                foreach (var kv in next.OrderBy(kv => kv.Key))
                {
                    for(int x = 0; x <5; x++)
                    {

                        char nc = kv.Value[y * 5 + x];
                        char cc = current[kv.Key][y * 5 + x];

                        if(cc == '#' && nc == '.')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if(cc == '.' && nc == '#')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        Console.Write(kv.Value[y * 5 + x]);
                    }

                    Console.Write("  ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.ResetColor();
        }

        private char[] EmptyGrid()
        {
            char[] grid = new char[5 * 5];
            Array.Fill(grid, '.');
            return grid;
        }
    }
}
