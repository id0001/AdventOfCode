using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.Collections.Helpers;
using System.Diagnostics;
using System.Net.WebSockets;

namespace AdventOfCode2018.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var coords = await inputReader.ParseLinesAsync(6, ParseLine).ToListAsync();
        var cloud = new PointCloud<Point2, int>(coords);
        var areas = new Dictionary<Point2, int>();
        foreach (var p in cloud.Bounds.EnumeratePoints())
        {
            if (!TryGetClosestPoint(p, coords, out var closest))
                continue;

            areas.TryAdd(closest, 0);
            if (IsOnBorder(p, cloud.Bounds))
                areas[closest] = int.MaxValue;
            else
                areas[closest] = Math.Min(areas[closest] + 1, int.MaxValue);
        }

        return areas.Where(kv => kv.Value != int.MaxValue).Max(kv => kv.Value).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var coords = await inputReader.ParseLinesAsync(6, ParseLine).ToListAsync();
        var cloud = new PointCloud<Point2, int>(coords);
        int totalArea = 0;
        foreach (var p in cloud.Bounds.EnumeratePoints())
        {
            var sumOfDistances = coords.Sum(c => Point2.ManhattanDistance(p, c));
            if (sumOfDistances < 10000)
                totalArea++;
        }

        return totalArea.ToString();
    }

    private static bool IsOnBorder(Point2 p, BoundingBox<Point2, int> bounds)
        => p.X == bounds.GetMin(0)
        || p.X == bounds.GetMax(0)
        || p.Y == bounds.GetMin(1)
        || p.Y == bounds.GetMax(1);

    private static bool TryGetClosestPoint(Point2 point, IList<Point2> points, out Point2 closest)
    {
        closest = Point2.Zero;
        var first2ClosestPoints = points
            .Select(p => (Point: p, Distance: Point2.ManhattanDistance(point, p)))
            .OrderBy(p => p.Distance)
            .Take(2)
            .ToArray();

        // Ignore point if it's closest to at least 2 points
        if (first2ClosestPoints.First().Distance == first2ClosestPoints.Second().Distance)
            return false;

        closest = first2ClosestPoints.First().Point;
        return true;
    }

    private static Point2 ParseLine(string line) => line
        .SplitBy(",")
        .As<int>()
        .Into(coords => new Point2(coords.First(), coords.Second()));
}