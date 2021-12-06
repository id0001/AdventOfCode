using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(5)]
    public class Challenge05
    {
        private readonly IInputReader inputReader;
        private Segment[] segments;

        public Challenge05(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            segments = await inputReader.ReadLinesAsync(5)
                .Select(line =>
                {
                    var match = Regex.Match(line, @"^(\d+),(\d+) -> (\d+),(\d+)$");
                    Point2 p1 = new Point2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    Point2 p2 = new Point2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                    return new Segment(p1, p2);
                }).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            var overlapDict = new Dictionary<Point2, int>();

            foreach (var segment in segments)
            {
                if (segment.Start.X != segment.End.X && segment.Start.Y != segment.End.Y)
                    continue;

                UpdateOverlapFromSegment(overlapDict, segment);
            }

            int overlapCount = overlapDict.Count(x => x.Value >= 2);

            return overlapCount.ToString();
        }

        [Part2]
        public string Part2()
        {
            var overlapDict = new Dictionary<Point2, int>();

            foreach (var segment in segments)
            {
                UpdateOverlapFromSegment(overlapDict, segment);
            }

            int overlapCount = overlapDict.Count(x => x.Value >= 2);

            return overlapCount.ToString();
        }

        private static void UpdateOverlapFromSegment(Dictionary<Point2, int> overlapDict, Segment segment)
        {
            int dy = Math.Sign(segment.End.Y - segment.Start.Y);
            int dx = Math.Sign(segment.End.X - segment.Start.X);

            for (int x = segment.Start.X, y = segment.Start.Y; x != segment.End.X + dx || y != segment.End.Y + dy; x += dx, y += dy)
            {
                overlapDict.AddOrUpdate(new Point2(x, y), v => v + 1);
            }
        }

        private record Segment(Point2 Start, Point2 End);
    }
}
