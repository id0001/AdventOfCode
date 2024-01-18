using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2022.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var points = await inputReader.ParseLinesAsync(18, ParseLine).ToHashSetAsync();

        var surfaceCount = points.Sum(p => p.GetNeighbors().Count(n => !points.Contains(n)));
        return surfaceCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var points = await inputReader.ParseLinesAsync(18, ParseLine).ToHashSetAsync();

        var cloud = new PointCloud<Point3, int>(points);
        var outsideAir = new HashSet<Point3>();
        FloodFill(new Point3(cloud.Bounds.GetMin(0) - 1, cloud.Bounds.GetMin(1) - 1, cloud.Bounds.GetMin(2) - 1), cloud,
            outsideAir);

        var surfaceCount = points.Sum(p => p.GetNeighbors().Count(n => outsideAir.Contains(n)));
        return surfaceCount.ToString();
    }

    private static void FloodFill(Point3 start, PointCloud<Point3, int> cloud, ISet<Point3> points)
    {
        var stack = new Stack<Point3>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var p = stack.Pop();
            points.Add(p);

            foreach (var neighbor in p.GetNeighbors().Where(n =>
                         !points.Contains(n) && !cloud.Contains(n) && cloud.Bounds.Contains(n, 1)))
                stack.Push(neighbor);
        }
    }

    private static Point3 ParseLine(string line)
    {
        var parsed = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        return new Point3(parsed[0], parsed[1], parsed[2]);
    }
}