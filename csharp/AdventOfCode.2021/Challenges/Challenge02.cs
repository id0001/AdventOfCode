using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var x = 0;
        var y = 0;

        await foreach (var movement in inputReader.ParseLinesAsync(2, ParseLine))
            switch (movement.Direction)
            {
                case "forward":
                    x += movement.Amount;
                    break;
                case "down":
                    y += movement.Amount;
                    break;
                case "up":
                    y -= movement.Amount;
                    break;
            }

        return (x * y).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var x = 0;
        var y = 0;
        var aim = 0;

        await foreach (var movement in inputReader.ParseLinesAsync(2, ParseLine))
            switch (movement.Direction)
            {
                case "forward":
                    x += movement.Amount;
                    y += movement.Amount * aim;
                    break;
                case "down":
                    aim += movement.Amount;
                    break;
                case "up":
                    aim -= movement.Amount;
                    break;
            }

        return (x * y).ToString();
    }

    private Movement ParseLine(string line) =>
        line.SplitBy(" ").Transform(parts => new Movement(parts.First(), int.Parse(parts.Second())));

    private record Movement(string Direction, int Amount);
}