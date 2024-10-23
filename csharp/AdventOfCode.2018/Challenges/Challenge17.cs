using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(17)]
public class Challenge17(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var sand = CreateGrid(await inputReader.ParseLinesAsync(17, ParseLine).SelectMany(x => x.ToAsyncEnumerable()).ToListAsync());

        var water = new SparseSpatialMap<Point2, int, Water>();
        Flow(new Point2(500, 0), sand, water);

        return water
            .Where(p => p.Key.Y >= sand.Bounds.GetMin(1) && p.Key.Y <= sand.Bounds.GetMax(1))
            .Count()
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var sand = CreateGrid(await inputReader.ParseLinesAsync(17, ParseLine).SelectMany(x => x.ToAsyncEnumerable()).ToListAsync());

        var water = new SparseSpatialMap<Point2, int, Water>();
        Flow(new Point2(500, 0), sand, water);

        return water
            .Where(p => p.Key.Y >= sand.Bounds.GetMin(1) && p.Key.Y <= sand.Bounds.GetMax(1) && p.Value == Water.Settled)
            .Count()
            .ToString();
    }

    private bool Flow(Point2 current, PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water)
    {
        if (current.Y > sand.Bounds.GetMax(1))
            return true; // hit the void

        water.Set(current, Water.Flowing);

        bool hitVoid = false;
        if (CanFlowDown(sand, water, current))
            hitVoid = Flow(current.Down, sand, water);

        if (CanFlowSideways(sand, water, current, Face.Left))
            hitVoid = Flow(current.Left, sand, water);

        if (CanFlowSideways(sand, water, current, Face.Right))
            hitVoid = Flow(current.Right, sand, water);

        if (!hitVoid && water.Get(current) == Water.Flowing)
            Settle(current, sand, water);

        return hitVoid;
    }

    private static void Settle(Point2 current, PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water)
    {
        var toSettle = new List<Point2>();
        for (int i = current.X; ; i--)
        {
            var p = new Point2(i, current.Y);
            if (sand.Contains(p))
                break; // hit wall

            if (!sand.Contains(p) && !water.ContainsKey(p))
                return; // hit empty air. Cannot settle

            toSettle.Add(p);
        }

        for (int i = current.X; ; i++)
        {
            var p = new Point2(i, current.Y);
            if (sand.Contains(p))
                break; // hit wall

            if (!sand.Contains(p) && !water.ContainsKey(p))
                return; // hit empty air. Cannot settle

            toSettle.Add(p);
        }

        foreach (var p in toSettle)
            water.Set(p, Water.Settled);
    }

    private static bool CanFlowDown(PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water, Point2 current)
        => IsEmpty(sand, water, current.Down);

    private static bool CanFlowSideways(PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water, Point2 current, Point2 direction)
        => IsEmpty(sand, water, current + direction) && IsSolidOrSettled(sand, water,current.Down);

    private static bool IsEmpty(PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water, Point2 p)
        => !sand.Contains(p) && water.Get(p) == Water.None;

    private static bool IsSolidOrSettled(PointCloud<Point2, int> sand, SparseSpatialMap<Point2, int, Water> water, Point2 p)
        => sand.Contains(p) || water.Get(p) == Water.Settled;

    private static PointCloud<Point2, int> CreateGrid(IEnumerable<Point2> points) => new(points);

    private static IEnumerable<Point2> ParseLine(string line) => line
        .SplitBy(",")
        .Into(parts =>
        {
            var (c, a) = parts.First().Extract<char, int>(@"(x|y)=(\d+)");
            var (bl, br) = parts.Second().Extract<int, int>(@"(\d+)..(\d+)");
            return ParsePoints(a, Enumerable.Range(bl, br - bl + 1), c == 'y');
        });

    private static IEnumerable<Point2> ParsePoints(int a, IEnumerable<int> range, bool flipped) => range.Select(b => flipped ? new Point2(b, a) : new Point2(a, b));

    private enum Water
    {
        None = 0,
        Flowing = 1,
        Settled = 2
    }
}