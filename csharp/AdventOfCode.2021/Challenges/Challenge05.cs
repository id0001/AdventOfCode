using AdventOfCode.Lib;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(5)]
public class Challenge05
{
    private readonly IInputReader _inputReader;
    private Segment[] _segments;

    public Challenge05(IInputReader inputReader)
    {
        _inputReader = inputReader;
        _segments = Array.Empty<Segment>();
    }

    [Setup]
    public async Task SetupAsync()
    {
        _segments = await _inputReader.ReadLinesAsync(5)
            .Select(line =>
            {
                var match = Regex.Match(line, @"^(\d+),(\d+) -> (\d+),(\d+)$");
                var p1 = new Point2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                var p2 = new Point2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                return new Segment(p1, p2);
            }).ToArrayAsync();
    }

    [Part1]
    public string Part1()
    {
        var overlapDict = new Dictionary<Point2, int>();

        foreach (var segment in _segments)
        {
            if (segment.Start.X != segment.End.X && segment.Start.Y != segment.End.Y)
                continue;

            UpdateOverlapFromSegment(overlapDict, segment);
        }

        var overlapCount = overlapDict.Count(x => x.Value >= 2);

        return overlapCount.ToString();
    }

    [Part2]
    public string Part2()
    {
        var overlapDict = new Dictionary<Point2, int>();

        foreach (var segment in _segments) UpdateOverlapFromSegment(overlapDict, segment);

        var overlapCount = overlapDict.Count(x => x.Value >= 2);

        return overlapCount.ToString();
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