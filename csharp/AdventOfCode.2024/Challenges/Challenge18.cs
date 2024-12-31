using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var bytes = await inputReader.ParseLinesAsync(18, ParseLine).ToListAsync();
        var grid = bytes.Take(1024).ToHashSet();

        return grid
            .Path(Point2.Zero, GetAdjacent)
            .FindShortest(p => p == new Point2(70, 70))
            .Distance
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var bytes = await inputReader.ParseLinesAsync(18, ParseLine).ToListAsync();
        for (var i = bytes.Count - 1; i > 0; i--)
        {
            var grid = bytes.Take(i).ToHashSet();
            var result = grid.Path(Point2.Zero, GetAdjacent)
                .FindShortest(p => p == new Point2(70, 70));

            if (result.Success)
                return bytes[i + 1].ToString();
        }

        return string.Empty;
    }

    private static IEnumerable<Point2> GetAdjacent(ISet<Point2> bytes, Point2 point)
        => point
            .GetNeighbors()
            .Where(neighbor => neighbor.X is >= 0 and <= 70 && neighbor.Y is >= 0 and <= 70)
            .Where(neighbor => !bytes.Contains(neighbor));

    private static Point2 ParseLine(string line)
        => line.SplitBy(",").As<int>().Into(p => new Point2(p[0], p[1]));
}