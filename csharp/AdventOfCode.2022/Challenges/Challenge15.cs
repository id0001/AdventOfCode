using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(15)]
public class Challenge15
{
    private static readonly Regex LinePattern =
        new(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

    private readonly IInputReader _inputReader;

    public Challenge15(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var sensorsWithNearestBeacon = new Dictionary<Point2, Point2>();
        await foreach (var (sensor, beacon) in _inputReader.ParseLinesAsync(15, ParseLine))
            if (Covers(sensor, beacon, new Point2(sensor.X, 2000000)))
                sensorsWithNearestBeacon.Add(sensor, beacon);

        var bounds = GetCoverageBounds(sensorsWithNearestBeacon);

        return Enumerable
            .Range(bounds.X, bounds.Width + 1)
            .Count(n => CanNotContainBeacon(sensorsWithNearestBeacon, new Point2(n, 2000000))).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var sensorsWithNearestBeacon = new Dictionary<Point2, Point2>();
        await foreach (var (sensor, beacon) in _inputReader.ParseLinesAsync(15, ParseLine))
            sensorsWithNearestBeacon.Add(sensor, beacon);

        var borders = new HashSet<Point2>();
        foreach (var kv in sensorsWithNearestBeacon)
        {
            var md = Point2.ManhattanDistance(kv.Key, kv.Value) + 1;
            foreach (var p in Point2.ManhattanBorder(kv.Key, md))
            {
                if (p.X < 0 || p.Y < 0 || p.X > 4000000 || p.Y > 4000000)
                    continue;

                borders.Add(p);
            }
        }

        var b = borders.FirstOrDefault(p => !sensorsWithNearestBeacon.Any(s => Covers(s.Key, s.Value, p)));
        return (b.X * 4000000L + b.Y).ToString();
    }

    private static bool CanNotContainBeacon(Dictionary<Point2, Point2> sensorsWithNearestBeacon, Point2 point)
    {
        if (sensorsWithNearestBeacon.Values.Contains(point))
            return false;

        foreach (var kv in sensorsWithNearestBeacon)
        {
            var dist = Point2.ManhattanDistance(kv.Key, kv.Value);
            if (Point2.ManhattanDistance(kv.Key, point) <= dist)
                return true;
        }

        return false;
    }

    private static bool Covers(Point2 sensor, Point2 nearestBeacon, Point2 p)
    {
        var manhattanDistance = Point2.ManhattanDistance(sensor, nearestBeacon);
        return Point2.ManhattanDistance(sensor, p) <= manhattanDistance;
    }

    private static Rectangle GetCoverageBounds(Dictionary<Point2, Point2> sensorsWithNearestBeacon)
    {
        var left = int.MaxValue;
        var right = int.MinValue;
        var top = int.MaxValue;
        var bottom = int.MinValue;

        foreach (var kv in sensorsWithNearestBeacon)
        {
            var manhattanDistance = Point2.ManhattanDistance(kv.Key, kv.Value);

            left = Math.Min(left, kv.Key.X - manhattanDistance);
            right = Math.Max(right, kv.Key.X + manhattanDistance);
            top = Math.Min(top, kv.Key.Y - manhattanDistance);
            bottom = Math.Max(bottom, kv.Key.Y + manhattanDistance);
        }

        return new Rectangle(left, top, right - left, bottom - top);
    }

    private static (Point2, Point2) ParseLine(string line)
    {
        var match = LinePattern.Match(line);

        var sensor = new Point2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var beacon = new Point2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        return (sensor, beacon);
    }
}