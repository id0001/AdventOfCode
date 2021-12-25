using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(25)]
    public class Challenge25
    {
        private readonly IInputReader inputReader;
        private char[,] grid;

        public Challenge25(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            grid = await inputReader.ReadGridAsync(25);
        }

        [Part1]
        public string Part1()
        {
            int width = grid.GetLength(1);
            int height = grid.GetLength(0);

            Dictionary<Point2, char> set1 = new Dictionary<Point2, char>();
            var set2 = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).Where(x => grid[y, x] != '.').Select(x => (Location: new Point2(x, y), Value: grid[y, x]))).ToDictionary(kv => kv.Location, kv => kv.Value);
            int step = 0;
            do
            {
                set1 = set2;
                set2 = new Dictionary<Point2, char>();

                foreach (var cucumber in set1.Where(kv => kv.Value == '>'))
                {
                    var next = GetPointEastOf(cucumber.Key, width);
                    next = set1.ContainsKey(next) ? cucumber.Key : next;
                    set2.Add(next, cucumber.Value);
                }

                foreach (var cucumber in set1.Where(kv => kv.Value == 'v'))
                {
                    var next = GetPointSouthOf(cucumber.Key, height);
                    next = (set1.ContainsKey(next) && set1[next] == 'v') || (set2.ContainsKey(next) && set2[next] == '>') ? cucumber.Key : next;
                    set2.Add(next, cucumber.Value);
                }

                step++;
            } while (!set1.Keys.ToHashSet().SetEquals(set2.Keys));

            return step.ToString();
        }

        [Part2]
        public string Part2()
        {
            return null;
        }

        private static Point2 GetPointEastOf(Point2 p, int width)
        {
            if (p.X + 1 == width)
                return new Point2(0, p.Y);

            return new Point2(p.X + 1, p.Y);
        }

        private static Point2 GetPointSouthOf(Point2 p, int height)
        {
            if (p.Y + 1 == height)
                return new Point2(p.X, 0);

            return new Point2(p.X, p.Y + 1);
        }

        private void Print(Dictionary<Point2, char> set, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (set.TryGetValue(new Point2(x, y), out char c))
                    {
                        Console.Write(c);
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
