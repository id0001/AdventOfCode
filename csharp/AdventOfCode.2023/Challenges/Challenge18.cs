using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2023.Challenges;

[Challenge(18)]
public class Challenge18
{
    private readonly IInputReader _inputReader;

    public Challenge18(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var map = new SparseSpatialMap<Point2, char>
        {
            [Point2.Zero] = '.'
        };

        var current = Point2.Zero;
        var prevDir = Point2.Zero;
        var firstDir = Point2.Zero;
        var count = 0L;

        await foreach (var instruction in _inputReader.ParseLinesAsync(18, ParseLine))
        {
            var pose = new Pose2(current, GetDirection(instruction.Direction));

            if (prevDir != Point2.Zero)
                map[pose.Position] = GetCorner(prevDir, pose.Face);

            if (firstDir == Point2.Zero)
                firstDir = pose.Face;

            count += instruction.Amount;
            for (var i = 0; i < instruction.Amount; i++)
            {
                pose = pose.Step();
                map[pose.Position] = pose.Face.X != 0 ? '-' : '|';
            }

            current = pose.Position;
            prevDir = pose.Face;
        }

        map[Point2.Zero] = GetCorner(prevDir, firstDir);

        return (count + CountInside(map)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var map = new Dictionary<LongPoint, char>();
        var left = 0L;
        var right = 0L;
        var top = 0L;
        var bottom = 0L;

        var current = new LongPoint(0,0);
        var prevDir = Point2.Zero;
        var firstDir = Point2.Zero;
        var count = 0L;

        await foreach (var instruction in _inputReader.ParseLinesAsync(0, ParseLine2))
        {
            var dir = GetDirection(instruction.Direction);

            if (prevDir != Point2.Zero)
                map[current] = GetCorner(prevDir, dir);

            if (firstDir == Point2.Zero)
                firstDir = dir;

            count += instruction.Amount;
            for (var i = 0; i < instruction.Amount; i++)
            {
                current = new LongPoint(current.X + dir.X, current.Y + dir.Y);
                map[current] = dir.X != 0 ? '-' : '|';

                left = Math.Min(left, current.X);
                top = Math.Min(top, current.Y);
                right = Math.Max(right, current.X);
                bottom = Math.Max(bottom, current.Y);
            }

            prevDir = dir;
        }

        map[new LongPoint(0,0)] = GetCorner(prevDir, firstDir);

        return (count + CountInside2(map, new Bounds(left, top, right, bottom))).ToString();
    }

    private static char GetCorner(Point2 from, Point2 to)
    {
        return (from, to) switch
        {
            ((1, 0), (0, 1)) => '7',
            ((1, 0), (0, -1)) => 'J',
            ((-1, 0), (0, 1)) => 'F',
            ((-1, 0), (0, -1)) => 'L',
            ((0, 1), (1, 0)) => 'L',
            ((0, 1), (-1, 0)) => 'J',
            ((0, -1), (1, 0)) => 'F',
            ((0, -1), (-1, 0)) => '7',
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Point2 GetDirection(char c) => c switch
    {
        'U' => Point2.Up,
        'R' => Point2.Right,
        'D' => Point2.Down,
        'L' => Point2.Left,
        _ => throw new ArgumentOutOfRangeException()
    };

    private static long CountInside(SparseSpatialMap<Point2, char> map)
    {
        // Loop over every row and column and count how many vertical borders are crossed.
        // A point is inside the polygon when the amount of crossed borders is uneven.
        var inside = 0;
        var prev = '.';
        for (var y = map.Bounds.GetMin(1) - 1; y < map.Bounds.GetMax(1) + 1; y++)
        {
            var bordersCrossed = 0;
            for (var x = map.Bounds.GetMin(0) - 1; x < map.Bounds.GetMax(0) + 1; x++)
            {
                var p = new Point2(x, y);
                if (map.ContainsKey(p))
                {
                    if (map[p] == '7' && prev == 'L') // Special case L7 does not cross borders twice
                    {
                        prev = map[p];
                        continue;
                    }

                    if (map[p] == 'J' && prev == 'F') // Special case FJ does not cross borders twice
                    {
                        prev = map[p];
                        continue;
                    }

                    if (map[p] == '-') // Special case - does not cross any border
                        continue;

                    prev = map[p];
                    bordersCrossed++;
                    continue;
                }

                if (bordersCrossed % 2 == 1)
                    inside++;
            }
        }

        return inside;
    }
    
    private static long CountInside2(Dictionary<LongPoint, char> map, Bounds bounds)
    {
        // Loop over every row and column and count how many vertical borders are crossed.
        // A point is inside the polygon when the amount of crossed borders is uneven.
        var inside = 0;
        var prev = '.';
        for (var y = bounds.Top; y <= bounds.Bottom; y++)
        {
            var bordersCrossed = 0;
            for (var x = bounds.Left; x <= bounds.Right; x++)
            {
                var p = new LongPoint(x, y);
                if (map.ContainsKey(p))
                {
                    if (map[p] == '7' && prev == 'L') // Special case L7 does not cross borders twice
                    {
                        prev = map[p];
                        continue;
                    }

                    if (map[p] == 'J' && prev == 'F') // Special case FJ does not cross borders twice
                    {
                        prev = map[p];
                        continue;
                    }

                    if (map[p] == '-') // Special case - does not cross any border
                        continue;

                    prev = map[p];
                    bordersCrossed++;
                    continue;
                }

                if (bordersCrossed % 2 == 1)
                    inside++;
            }
        }

        return inside;
    }

    private static Instruction ParseLine(string line)
    {
        return line.SplitBy(" ")
            .Transform(parts => new Instruction(
                char.Parse(parts.First()),
                int.Parse(parts.Second()))
            );
    }

    private static Instruction ParseLine2(string line)
    {
        var dirs = new[] {'R', 'D', 'L', 'U'};

        return line.SplitBy(" ")
            .Transform(parts =>
                {
                    var match = Regex.Match(parts.Third(), @"\(#([\da-z]{5})(\d)\)");
                    return new Instruction(
                        dirs[int.Parse(match.Groups[2].Value)],
                        Convert.ToInt64($"0x{match.Groups[1].Value}", 16)
                    );
                }
            );
    }

    private record Instruction(char Direction, long Amount);

    private record LongPoint(long X, long Y);

    private record Bounds(long Left, long Top, long Right, long Bottom);
}