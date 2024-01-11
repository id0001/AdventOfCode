using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var overlapDict = new Dictionary<Point2, int>();

        await foreach (var segment in InputReader.ParseLinesAsync(5, ParseLine))
        {
            if (segment.Start.X != segment.End.X && segment.Start.Y != segment.End.Y)
                continue;

            UpdateOverlapFromSegment(overlapDict, segment);
        }

        var overlapCount = overlapDict.Count(x => x.Value >= 2);

        return overlapCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var overlapDict = new Dictionary<Point2, int>();

        await foreach (var segment in InputReader.ParseLinesAsync(5, ParseLine))
            UpdateOverlapFromSegment(overlapDict, segment);

        var overlapCount = overlapDict.Count(x => x.Value >= 2);

        return overlapCount.ToString();
    }

    private Segment ParseLine(string line)
    {
        var match = Regex.Match(line, @"^(\d+),(\d+) -> (\d+),(\d+)$");
        var p1 = new Point2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var p2 = new Point2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        return new Segment(p1, p2);
    }

    private static void UpdateOverlapFromSegment(Dictionary<Point2, int> overlapDict, Segment segment)
    {
        var dy = Math.Sign(segment.End.Y - segment.Start.Y);
        var dx = Math.Sign(segment.End.X - segment.Start.X);

        for (int x = segment.Start.X, y = segment.Start.Y;
             x != segment.End.X + dx || y != segment.End.Y + dy;
             x += dx, y += dy)
        {
            var key = new Point2(x, y);
            overlapDict.TryAdd(key, 0);
            overlapDict[key]++;
        }
    }

    private record Segment(Point2 Start, Point2 End);
}