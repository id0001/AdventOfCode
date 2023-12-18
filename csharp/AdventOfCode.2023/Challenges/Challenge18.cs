using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

/*
 * Shoelace + Picks theorem
 * https://en.wikipedia.org/wiki/Pick's_theorem
 * https://www.101computing.net/the-shoelace-algorithm/
 * 
 * Picks (Area of a polygon with only integer points):
 * i = number of contained integers
 * b = number of boundary points
 * Area (A) = i + (b/2) - 1
 * 
 * Shoelace gives the area of the polygon, but we want to include the border.
 * So determine i by plugging the calculated shoelace area into picks algorithm:
 * i = A - (b/2) = 1
 * 
 * And add the border:
 * i + b
 * 
 * Combined:
 * Ar = As + (b/2) + 1
 */

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
        var vertices = new List<LongPoint>();
        var current = new LongPoint(0, 0);
        var circumference = 0L;
        await foreach (var instruction in _inputReader.ParseLinesAsync(18, ParseLine))
        {
            circumference += instruction.Amount;
            var face = GetDirection(instruction.Direction);
            current = new LongPoint(current.X + (face.X * instruction.Amount), current.Y + (face.Y * instruction.Amount));
            vertices.Add(current);
        }
        
        return (GetArea(vertices) + circumference / 2 + 1).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var vertices = new List<LongPoint>();
        var current = new LongPoint(0, 0);
        var circumference = 0L;
        await foreach (var instruction in _inputReader.ParseLinesAsync(18, ParseLine2))
        {
            circumference += instruction.Amount;
            var face = GetDirection(instruction.Direction);
            current = new LongPoint(current.X + (face.X * instruction.Amount), current.Y + (face.Y * instruction.Amount));
            vertices.Add(current);
        }

        return (GetArea(vertices) + circumference / 2 + 1).ToString();
    }

    private static Point2 GetDirection(char c) => c switch
    {
        'U' => Point2.Up,
        'R' => Point2.Right,
        'D' => Point2.Down,
        'L' => Point2.Left,
        _ => throw new ArgumentOutOfRangeException()
    };

    private double GetArea(IList<LongPoint> vertices)
    {
        var vcount = vertices.Count;
        var sum1 = 0L;
        var sum2 = 0L;

        for (var i = 0; i < vcount - 1; i++)
        {
            sum1 += vertices[i].X * vertices[i + 1].Y;
            sum2 += vertices[i].Y * vertices[i + 1].X;
        }

        sum1 += vertices[vcount - 1].X * vertices[0].Y;
        sum2 += vertices[vcount - 1].Y * vertices[0].X;

        return Math.Abs(sum1 - sum2) / 2d;
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
        var dirs = new[] { 'R', 'D', 'L', 'U' };

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
}