using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader inputReader)
{
    private readonly Regex _pattern = new(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)");

    [Part1]
    public async Task<string?> Part1Async()
    {
        var grid = new bool[1000 * 1000];

        await foreach (var action in inputReader.ParseLinesAsync(6, ParseLine))
        {
            var rect = new Rectangle(action.From, action.To - action.From);

            foreach (var p in rect.AsGridPoints())
            {
                var index = p.Y * 1000 + p.X;
                grid[index] = action.Type switch
                {
                    "turn off" => false,
                    "turn on" => true,
                    "toggle" => !grid[index],
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        return grid.Count(x => x).ToString();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var grid = new int[1000 * 1000];

        await foreach (var action in inputReader.ParseLinesAsync(6, ParseLine))
        {
            var rect = new Rectangle(action.From, action.To - action.From);

            foreach (var p in rect.AsGridPoints())
            {
                var index = p.Y * 1000 + p.X;
                grid[index] = Math.Max(0, grid[index] + action.Type switch
                {
                    "turn off" => -1,
                    "turn on" => 1,
                    "toggle" => 2,
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        }

        return grid.Sum().ToString();
    }

    private LightAction ParseLine(string line)
    {
        var match = _pattern.Match(line);
        var from = new Point2(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
        var to = new Point2(int.Parse(match.Groups[4].Value) + 1, int.Parse(match.Groups[5].Value) + 1);

        return new LightAction(match.Groups[1].Value, from, to);
    }

    private record LightAction(string Type, Point2 From, Point2 To);
}