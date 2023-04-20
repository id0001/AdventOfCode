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

            var start = new Point2(500, 0);
            int s = 0;
            int bottomBound = map.Bounds.GetMax(1);
            while (true)
            {
                map[start] = 'o';

                while (true)
                {
                    var bottom = start + new Point2(0, 1);
                    var bottomLeft = start + new Point2(-1, 1);
                    var bottomRight = start + new Point2(1, 1);

                    if (bottom.Y > bottomBound)
                    {
                        PrintMap(map);
                        return s.ToString();
                    }

                    if (map.GetValueOrDefault(bottom, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottom, 'o');
                        start = bottom;
                    }
                    else if (map.GetValueOrDefault(bottomLeft, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottomLeft, 'o');
                        start = bottomLeft;
                    }
                    else if (map.GetValueOrDefault(bottomRight, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottomRight, 'o');
                        start = bottomRight;
                    }
                    else
                    {
                        start = new Point2(500, 0);
                        s++;
                        break;
                    }
                }
            }
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

            var start = new Point2(500, 0);
            int s = 0;
            int bottomBound = map.Bounds.GetMax(1) + 1;
            while (true)
            {
                map[start] = 'o';

                while (true)
                {
                    var bottom = start + new Point2(0, 1);
                    var bottomLeft = start + new Point2(-1, 1);
                    var bottomRight = start + new Point2(1, 1);

                    if(bottom.Y == bottomBound)
                    {
                        start = new Point2(500, 0);
                        s++;
                        break;
                    }
                    else if (map.GetValueOrDefault(bottom, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottom, 'o');
                        start = bottom;
                    }
                    else if (map.GetValueOrDefault(bottomLeft, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottomLeft, 'o');
                        start = bottomLeft;
                    }
                    else if (map.GetValueOrDefault(bottomRight, '.') == '.')
                    {
                        map.AddOrUpdate(start, '.');
                        map.AddOrUpdate(bottomRight, 'o');
                        start = bottomRight;
                    }
                    else if(start == new Point2(500, 0))
                    {
                        PrintMap(map);
                        s++;
                        return s.ToString();
                    }
                    else
                    {
                        start = new Point2(500, 0);
                        s++;
                        break;
                    }
                }
            }
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
