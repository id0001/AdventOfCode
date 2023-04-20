using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Challenges
{
    [Challenge(14)]
    public class Challenge14
    {
        private readonly IInputReader _inputReader;

        public Challenge14(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var map = new SparseSpatialMap<Point2, char>();

            await foreach (var points in _inputReader.ParseLinesAsync(14, ParseLine))
            {
                var iterator = points.GetEnumerator();
                foreach (var (current, next) in points.CurrentAndNext())
                {
                    foreach (var p in Point2.BresenhamLine(current, next))
                    {
                        map.AddOrUpdate(p, '#');
                    }
                }
            }

            var down = new Point2(0, 1);
            var downLeft = new Point2(-1, 1);
            var downRight = new Point2(1, 1);
            var start = new Point2(500, 0);

            int s = 0;
            int bottomBound = map.Bounds.GetMax(1);
            Point2 curr = start;

            while (curr.Y < bottomBound)
            {
                if (map.GetValueOrDefault(curr + down, '.') == '.')
                {
                    curr += down;
                    continue;
                }

                if (map.GetValueOrDefault(curr + downLeft, '.') == '.')
                {
                    curr += downLeft;
                    continue;
                }

                if (map.GetValueOrDefault(curr + downRight, '.') == '.')
                {
                    curr += downRight;
                    continue;
                }

                s++;
                map.AddOrUpdate(curr, 'o');
                curr = start;
            }

            return s.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var map = new SparseSpatialMap<Point2, char>();

            await foreach (var points in _inputReader.ParseLinesAsync(14, ParseLine))
            {
                var iterator = points.GetEnumerator();
                foreach (var (current, next) in points.CurrentAndNext())
                {
                    foreach (var p in Point2.BresenhamLine(current, next))
                    {
                        map.AddOrUpdate(p, '#');
                    }
                }
            }

            var down = new Point2(0, 1);
            var downLeft = new Point2(-1, 1);
            var downRight = new Point2(1, 1);
            var start = new Point2(500, 0);

            int s = 0;
            int bottomBound = map.Bounds.GetMax(1) + 1;
            Point2 curr = start;

            while (true)
            {
                if ((curr + down).Y != bottomBound)
                {
                    if (map.GetValueOrDefault(curr + down, '.') == '.')
                    {
                        curr += down;
                        continue;
                    }

                    if (map.GetValueOrDefault(curr + downLeft, '.') == '.')
                    {
                        curr += downLeft;
                        continue;
                    }

                    if (map.GetValueOrDefault(curr + downRight, '.') == '.')
                    {
                        curr += downRight;
                        continue;
                    }
                }

                // At rest
                s++;
                map.AddOrUpdate(curr, 'o');

                if (curr == start)
                    break;

                curr = start;
            }

            return s.ToString();
        }

        private static IEnumerable<Point2> ParseLine(string line)
        {
            string[] split = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in split)
            {
                var arr = part.Split(',').Select(int.Parse).ToArray();
                yield return new Point2(arr[0], arr[1]);
            }
        }

        private static void PrintMap(SparseSpatialMap<Point2, char> map)
        {
            var sb = new StringBuilder(); ;

            for (int y = map.Bounds.GetMin(1); y <= map.Bounds.GetMax(1); y++)
            {
                for (int x = map.Bounds.GetMin(0); x <= map.Bounds.GetMax(0); x++)
                {
                    var p = new Point2(x, y);
                    if (map.ContainsKey(p))
                        sb.Append(map[p]);
                    else
                        sb.Append('.');
                }

                sb.AppendLine();
            }

            File.WriteAllText(@"C:\Temp\output.txt", sb.ToString());
        }
    }
}
