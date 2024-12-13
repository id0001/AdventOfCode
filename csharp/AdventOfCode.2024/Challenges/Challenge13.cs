using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    private const long Part2 = 10_000_000_000_000;

    [Part1]
    public async Task<string> Part1Async()
    {
        var machines = await inputReader.ParseTextAsync(13, ParseInput);
        return machines
            .Select(m => CramersRule(m.ButtonA.X, m.ButtonA.Y, m.ButtonB.X, m.ButtonB.Y, m.Price.X, m.Price.Y))
            .Sum()
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var machines = await inputReader.ParseTextAsync(13, ParseInput);
        return machines
            .Select(m => CramersRule(m.ButtonA.X, m.ButtonA.Y, m.ButtonB.X, m.ButtonB.Y, m.Price.X + Part2, m.Price.Y + Part2))
            .Sum()
            .ToString();
    }

    private static IList<Machine> ParseInput(string input)
    {
        return input
            .SelectParagraphs()
            .Select(p => p.SelectLines().Into(lines => new Machine(
                lines[0].Extract<int, int>(@"Button A: X\+(\d+), Y\+(\d+)").Into(x => new Point2(x.First, x.Second)),
                lines[1].Extract<int, int>(@"Button B: X\+(\d+), Y\+(\d+)").Into(x => new Point2(x.First, x.Second)),
                lines[2].Extract<int, int>(@"Prize: X=(-?\d+), Y=(-?\d+)").Into(x => new Point2(x.First, x.Second)))))
            .ToList();
    }

    private static long CramersRule(long ax, long ay, long bx, long by, long px, long py)
    {
        var d = ax * by - bx * ay;
        var dx = px * by - py * bx;
        var dy = py * ax - px * ay;

        if (d == 0)
            return 0L;

        var x = dx / d;
        var y = dy / d;

        if (dx == x * d && dy == y * d)
            return x * 3L + y;

        return 0L;
    }

    private record Machine(Point2 ButtonA, Point2 ButtonB, Point2 Price);
}
