using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(9)]
internal class Challenge09
{
    private static readonly Dictionary<char, Point2> Directions = new()
    {
        {'U', new Point2(0, -1)},
        {'R', new Point2(1, 0)},
        {'D', new Point2(0, 1)},
        {'L', new Point2(-1, 0)}
    };

    private readonly IInputReader _inputReader;

    public Challenge09(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var headPos = Point2.Zero;
        var tailPos = Point2.Zero;
        var visited = new HashSet<Point2>
        {
            tailPos
        };

        await foreach (var (direction, amount) in _inputReader.ParseLinesAsync(9, ParseLine))
            for (var i = 0; i < amount; i++)
            {
                headPos += Directions[direction];

                var diff = headPos - tailPos;
                var move = Point2.Zero;

                if (Math.Abs(diff.X) == 2)
                {
                    move += new Point2(Math.Sign(diff.X), 0);
                    if (diff.Y != 0)
                        move += new Point2(0, Math.Sign(diff.Y));
                }
                else if (Math.Abs(diff.Y) == 2)
                {
                    move += new Point2(0, Math.Sign(diff.Y));
                    if (diff.X != 0)
                        move += new Point2(Math.Sign(diff.X), 0);
                }

                tailPos += move;
                visited.Add(tailPos);
            }

        return visited.Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var knots = new Point2[10];
        var visited = new HashSet<Point2>
        {
            knots[9]
        };

        await foreach (var (direction, amount) in _inputReader.ParseLinesAsync(9, ParseLine))
            for (var i = 0; i < amount; i++)
            {
                knots[0] += Directions[direction];

                for (var j = 1; j < knots.Length; j++)
                {
                    var diff = knots[j - 1] - knots[j];
                    var move = Point2.Zero;

                    if (Math.Abs(diff.X) == 2)
                    {
                        move += new Point2(Math.Sign(diff.X), 0);
                        if (diff.Y != 0)
                            move += new Point2(0, Math.Sign(diff.Y));
                    }
                    else if (Math.Abs(diff.Y) == 2)
                    {
                        move += new Point2(0, Math.Sign(diff.Y));
                        if (diff.X != 0)
                            move += new Point2(Math.Sign(diff.X), 0);
                    }

                    knots[j] += move;
                }

                visited.Add(knots[9]);
            }

        return visited.Count.ToString();
    }

    private (char, int) ParseLine(string line)
    {
        var split = line.Split(' ');
        return (split[0][0], int.Parse(split[1]));
    }
}