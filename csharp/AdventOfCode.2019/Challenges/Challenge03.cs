using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2019.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await InputReader.ReadLinesAsync(3).ToArrayAsync();

        var wire1 = GetWire(lines[0].Split(','));
        var wire2 = GetWire(lines[1].Split(','));

        var intersection = wire1.Keys.Intersect(wire2.Keys);

        return intersection.Select(ManhattanDistance).Min().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await InputReader.ReadLinesAsync(3).ToArrayAsync();

        var wire1 = GetWire(lines[0].Split(','));
        var wire2 = GetWire(lines[1].Split(','));

        var intersections = wire1.Keys.Intersect(wire2.Keys);

        return intersections.Select(p => wire1[p] + wire2[p]).Min().ToString();
    }

    private static int ManhattanDistance(Point2 p) => Math.Abs(p.X) + Math.Abs(p.Y);

    private IDictionary<Point2, int> GetWire(string[] moves)
    {
        var dict = new Dictionary<Point2, int>();

        var current = Point2.Zero;
        var steps = 0;
        foreach (var move in moves)
        {
            var dir = move[0];
            var amount = int.Parse(move[1..]);

            for (var i = 0; i < amount; i++)
            {
                current += dir switch
                {
                    'U' => new Point2(0, -1),
                    'R' => new Point2(1, 0),
                    'D' => new Point2(0, 1),
                    'L' => new Point2(-1, 0),
                    _ => throw new NotImplementedException()
                };

                steps++;
                dict.TryAdd(current, steps);
            }
        }

        return dict;
    }
}