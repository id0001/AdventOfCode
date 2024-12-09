using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(8)]
public class Challenge08(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(8);
        var antennaLocations = GetAntennaLocations(grid);

        var antinodes = new HashSet<Point2>();
        var bounds = grid.Bounds();
        foreach (var kv in antennaLocations)
        {
            foreach (var window in kv.Value.Combinations(2))
            {
                var sorted = window.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();

                var p1 = GetLeftAntinode(sorted[0], sorted[1]);
                if (bounds.Contains(p1))
                    antinodes.Add(p1);

                var p2 = GetRightAntinode(sorted[0], sorted[1]);
                if (bounds.Contains(p2))
                    antinodes.Add(p2);
            }
        }

        return antinodes.Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(8);
        var antennaLocations = GetAntennaLocations(grid);

        var antinodes = new HashSet<Point2>();
        var bounds = grid.Bounds();
        foreach (var kv in antennaLocations)
        {
            foreach (var window in kv.Value.Combinations(2))
            {
                var sorted = window.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();

                var left = sorted[0];
                var right = sorted[1];

                antinodes.Add(left);
                antinodes.Add(right);

                var antinode = GetLeftAntinode(left, right);
                while (bounds.Contains(antinode))
                {
                    antinodes.Add(antinode);
                    right = left;
                    left = antinode;
                    antinode = GetLeftAntinode(left, right);
                }

                left = sorted[0];
                right = sorted[1];

                antinode = GetRightAntinode(left, right);
                while (bounds.Contains(antinode))
                {
                    antinodes.Add(antinode);
                    left = right;
                    right = antinode;
                    antinode = GetRightAntinode(left, right);
                }
            }
        }

        return antinodes.Count.ToString();
    }

    private static Dictionary<char, List<Point2>> GetAntennaLocations(char[,] grid)
    {
        var beaconLocations = new Dictionary<char, List<Point2>>();
        foreach (var (p, c) in grid.AsEnumerable())
        {
            if (c == '.')
                continue;

            if (!beaconLocations.TryAdd(c, [p]))
                beaconLocations[c].Add(p);
        }

        return beaconLocations;
    }

    private Point2 GetLeftAntinode(Point2 a, Point2 b) => new Point2(a.X + (a.X - b.X), a.Y + (a.Y - b.Y));
    private Point2 GetRightAntinode(Point2 a, Point2 b) => new Point2(b.X + (b.X - a.X), b.Y + (b.Y - a.Y));
}
