using AdventOfCode.Lib;
using AdventOfCode.Lib.Comparers;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(10)]
public class Challenge10
{
    private readonly IInputReader _inputReader;
    private readonly List<Point2> _asteroids = new List<Point2>();

    public Challenge10(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var y = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(10))
        {
            for (var x = 0; x < line.Length; x++)
                if (line[x] == '#')
                    _asteroids.Add(new Point2(x, y));

            y++;
        }
    }

    [Part1]
    public string Part1()
    {
        var visibleMap = _asteroids.ToDictionary(kv => kv, _ => 0);

        for (var i = 0; i < _asteroids.Count; i++)
        {
            var origin = _asteroids[i];
            for (var j = i + 1; j < _asteroids.Count; j++)
            {
                var target = _asteroids[j];

                var d0 = Distance(origin, target);
                var a0 = Angle(origin, target);

                var blocked = false;
                for (var k = 0; k < _asteroids.Count; k++)
                {
                    if (k == i || k == j)
                        continue;

                    var test = _asteroids[k];
                    var a1 = Angle(origin, test);

                    if (!a0.MarginalEquals(a1) || !(Distance(origin, test) < d0)) continue;

                    // Equal angle and distance is lower.
                    // Target is blocked.
                    blocked = true;
                    break;
                }

                if (blocked) continue;

                visibleMap[origin]++;
                visibleMap[target]++;
            }
        }

        var best = visibleMap.MaxBy(kv => kv.Value);

        return best.Value.ToString();
    }

    [Part2]
    public string Part2()
    {
        var center = new Point2(20, 18);
        _asteroids.Remove(center);
        var comparer = new DoubleEqualityComparer();

        var list = _asteroids.Select(p => new { Point = p, Angle = Angle(center, p), Distance = Distance(center, p) })
            .GroupBy(t => t.Angle, comparer)
            .OrderBy(g => g.Key)
            .Select(g => g.OrderBy(x => x.Distance).Select(x => x.Point).ToList())
            .ToList();

        var p = EnumerateSortedAsteroids(list)
            .Skip(199)
            .First();

        return (p.X * 100 + p.Y).ToString();
    }

    private static IEnumerable<Point2> EnumerateSortedAsteroids(IReadOnlyList<List<Point2>> list)
    {
        var length = list.Sum(x => x.Count);
        var indices = new int[list.Count];
        var i = 0;
        for (var k = 0; k < length; k++)
        {
            if (indices[i] < list[i].Count)
            {
                var p = list[i][indices[i]];
                yield return p;
                indices[i]++;
            }

            i = (i + 1) % list.Count;
        }
    }

    private static double Distance(Point2 origin, Point2 target) => Point2.DistanceSquared(origin, target);

    private static double Angle(Point2 origin, Point2 target)
    {
        var angle = Vector2.AngleOnCircle(target, origin);
        angle += Math.PI / 2d;
        return angle % (Math.PI * 2);
    }
}