using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var set = await inputReader.ParseLinesAsync(10, ParseLine).ToHashSetAsync();

        while (true)
        {
            set = CalculateNextPositions(set);
            if (TryGetResult(set.Select(x => x.Position), out var value))
            {
                var ocr = value.Ocr();
                if (ocr.Length > 0)
                    return ocr;
            }
        }
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var set = await inputReader.ParseLinesAsync(10, ParseLine).ToHashSetAsync();

        for (var i = 1;; i++)
        {
            set = CalculateNextPositions(set);
            if (TryGetResult(set.Select(x => x.Position), out var value))
            {
                var ocr = value.Ocr();
                if (ocr.Length > 0)
                    return i.ToString();
            }
        }
    }

    private bool TryGetResult(IEnumerable<Point2> points, out string result)
    {
        result = string.Empty;
        var cloud = new PointCloud<Point2, int>(points);
        if (cloud.Bounds.GetMax(0) - cloud.Bounds.GetMin(0) > 100)
            return false;

        var sb = new StringBuilder();
        for (var y = cloud.Bounds.GetMin(1); y <= cloud.Bounds.GetMax(1); y++)
        {
            for (var x = cloud.Bounds.GetMin(0); x <= cloud.Bounds.GetMax(0); x++)
                sb.Append(cloud.Contains(new Point2(x, y)) ? '#' : '.');

            sb.AppendLine();
        }

        result = sb.ToString();
        return true;
    }

    private static HashSet<PosVel> CalculateNextPositions(HashSet<PosVel> items)
        => items.Select(x => x with {Position = x.Position + x.Velocity}).ToHashSet();

    private PosVel ParseLine(string line) => line
        .Extract<int, int, int, int>(@"position=< ?(-?\d+),  ?(-?\d+)> velocity=< ?(-?\d+),  ?(-?\d+)>")
        .Into(matches =>
            new PosVel(new Point2(matches.First, matches.Second), new Point2(matches.Third, matches.Fourth)));

    private record PosVel(Point2 Position, Point2 Velocity);
}