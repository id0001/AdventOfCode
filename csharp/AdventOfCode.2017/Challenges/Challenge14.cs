using System.Numerics;
using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2017.Challenges;

[Challenge(14)]
public class Challenge14
{
    private const string Input = "xlqgujun";
    private static readonly Rectangle Bounds = new(0, 0, 128, 128);

    [Part1]
    public string Part1() => Enumerable.Range(0, 128)
        .Select(i => KnotHash.Generate($"{Input}-{i}").Sum(b => BitOperations.PopCount(b)))
        .Sum()
        .ToString();

    [Part2]
    public string Part2()
    {
        var bits = Enumerable.Range(0, 128)
            .SelectMany(i => KnotHash.Generate($"{Input}-{i}").ToBits())
            .Select(b => b ? 1 : 0)
            .ToArray();

        var regionCount = 0;

        var bfs = new BreadthFirstSearch<int>(n => GetAdjacent(bits, n));

        while (true)
        {
            var unvisited = Enumerable.Range(0, bits.Length).FirstOrDefault(i => bits[i] == 1, -1);
            if (unvisited == -1)
                return regionCount.ToString();

            regionCount++;
            foreach (var (i, _) in bfs.FloodFill(unvisited))
                bits[i] = 100 + regionCount;
        }
    }

    private static IEnumerable<int> GetAdjacent(int[] source, int i)
    {
        var p = i.ToPoint2(128);
        foreach (var n in p.GetNeighbors().Where(Bounds.Contains))
        {
            var index = n.ToIndex(128);
            if (source[index] == 1)
                yield return index;
        }
    }
}