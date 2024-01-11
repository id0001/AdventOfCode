using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2022.Challenges;

[Challenge(14)]
public class Challenge14(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var map = new SparseSpatialMap<Point2, int, char>();

        await foreach (var points in InputReader.ParseLinesAsync(14, ParseLine))
        foreach (var (current, next) in points.CurrentAndNext())
        foreach (var p in Point2.BresenhamLine(current, next))
            map.Set(p, '#');

        var down = new Point2(0, 1);
        var downLeft = new Point2(-1, 1);
        var downRight = new Point2(1, 1);
        var start = new Point2(500, 0);

        var s = 0;
        var bottomBound = map.Bounds.GetMax(1) + 1;
        var curr = start;

        while (curr.Y < bottomBound)
        {
            if (map.Get(curr + down, '.') == '.')
            {
                curr += down;
                continue;
            }

            if (map.Get(curr + downLeft, '.') == '.')
            {
                curr += downLeft;
                continue;
            }

            if (map.Get(curr + downRight, '.') == '.')
            {
                curr += downRight;
                continue;
            }

            s++;
            map.Set(curr, 'o');
            curr = start;
        }

        return s.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var map = new SparseSpatialMap<Point2, int, char>();

        await foreach (var points in InputReader.ParseLinesAsync(14, ParseLine))
        foreach (var (current, next) in points.CurrentAndNext())
        foreach (var p in Point2.BresenhamLine(current, next))
            map.Set(p, '#');

        var down = new Point2(0, 1);
        var downLeft = new Point2(-1, 1);
        var downRight = new Point2(1, 1);
        var start = new Point2(500, 0);

        var s = 0;
        var bottomBound = map.Bounds.GetMax(1) + 2;
        var curr = start;

        while (true)
        {
            if ((curr + down).Y != bottomBound)
            {
                if (map.Get(curr + down, '.') == '.')
                {
                    curr += down;
                    continue;
                }

                if (map.Get(curr + downLeft, '.') == '.')
                {
                    curr += downLeft;
                    continue;
                }

                if (map.Get(curr + downRight, '.') == '.')
                {
                    curr += downRight;
                    continue;
                }
            }

            // At rest
            s++;
            map.Set(curr, 'o');

            if (curr == start)
                break;

            curr = start;
        }

        return s.ToString();
    }

    private static IEnumerable<Point2> ParseLine(string line)
    {
        var split = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in split)
        {
            var arr = part.Split(',').Select(int.Parse).ToArray();
            yield return new Point2(arr[0], arr[1]);
        }
    }

    //private static void PrintMap(SparseSpatialMap<Point2, char> map)
    //{
    //    var sb = new StringBuilder(); ;

    //    for (int y = map.Bounds.GetMin(1); y <= map.Bounds.GetMax(1); y++)
    //    {
    //        for (int x = map.Bounds.GetMin(0); x <= map.Bounds.GetMax(0); x++)
    //        {
    //            var p = new Point2(x, y);
    //            if (map.ContainsKey(p))
    //                sb.Append(map[p]);
    //            else
    //                sb.Append('.');
    //        }

    //        sb.AppendLine();
    //    }

    //    File.WriteAllText(@"C:\Temp\output.txt", sb.ToString());
    //}
}