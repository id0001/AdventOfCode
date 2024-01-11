using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

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
public class Challenge18(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var vertices = new List<Point2L>();
        var current = new Point2L(0, 0);
        var pointsInBorder = 0L;
        await foreach (var instruction in InputReader.ParseLinesAsync(18, ParseLine))
        {
            pointsInBorder += instruction.Amount;
            var face = GetDirection(instruction.Direction);
            current = new Point2L(current.X + face.X * instruction.Amount, current.Y + face.Y * instruction.Amount);
            vertices.Add(current);
        }

        return (Polygon.CountInteriorPoints((long) Polygon.ShoelaceArea(vertices), pointsInBorder) + pointsInBorder)
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var vertices = new List<Point2L>();
        var current = new Point2L(0, 0);
        var pointsInBorder = 0L;
        await foreach (var instruction in InputReader.ParseLinesAsync(18, ParseLine2))
        {
            pointsInBorder += instruction.Amount;
            var face = GetDirection(instruction.Direction);
            current = new Point2L(current.X + face.X * instruction.Amount, current.Y + face.Y * instruction.Amount);
            vertices.Add(current);
        }

        return (Polygon.CountInteriorPoints((long) Polygon.ShoelaceArea(vertices), pointsInBorder) + pointsInBorder)
            .ToString();
    }

    private static Point2 GetDirection(char c) => c switch
    {
        'U' => Face.Up,
        'R' => Face.Right,
        'D' => Face.Down,
        'L' => Face.Left,
        _ => throw new ArgumentOutOfRangeException()
    };

    private static Instruction ParseLine(string line)
    {
        return line.SplitBy(" ")
            .Into(parts => new Instruction(
                parts.First().As<char>(),
                parts.Second().As<int>())
            );
    }

    private static Instruction ParseLine2(string line)
    {
        var dirs = new[] {'R', 'D', 'L', 'U'};

        return line.SplitBy(" ")
            .Into(parts =>
                {
                    var match = Regex.Match(parts.Third(), @"\(#([\da-z]{5})(\d)\)");
                    return new Instruction(
                        dirs[match.Groups[2].Value.As<int>()],
                        Convert.ToInt64($"0x{match.Groups[1].Value}", 16)
                    );
                }
            );
    }

    private record Instruction(char Direction, long Amount);

    private record Point2L(long X, long Y) : IPoint<long>
    {
        long IPoint<long>.this[int index] => index switch
        {
            0 => X,
            1 => Y,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };

        int IPoint<long>.Dimensions => 2;

        bool IEquatable<IPoint<long>>.Equals(IPoint<long>? other)
        {
            if (other is null)
                return false;

            var instance = (IPoint<long>) this;

            if (other.Dimensions != instance.Dimensions)
                return false;

            for (var d = 0; d < instance.Dimensions; d++)
                if (instance[d] != other[d])
                    return false;

            return true;
        }
    }
}