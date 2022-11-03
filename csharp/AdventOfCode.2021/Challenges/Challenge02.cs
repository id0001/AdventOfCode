using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(2)]
public class Challenge02
{
    private readonly IInputReader _inputReader;
    private Movement[] _movements;

    public Challenge02(IInputReader inputReader)
    {
        _inputReader = inputReader;
        _movements = Array.Empty<Movement>();
    }

    [Setup]
    public async Task SetupAsync()
    {
        _movements = await _inputReader.ReadLinesAsync(2)
            .Select(l =>
            {
                var s = l.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return new Movement(s[0], int.Parse(s[1]));
            }).ToArrayAsync();
    }

    [Part1]
    public string Part1()
    {
        var x = 0;
        var y = 0;

        foreach (var movement in _movements)
        {
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
        }

        return (x * y).ToString();
    }

    [Part2]
    public string Part2()
    {
        var x = 0;
        var y = 0;
        var aim = 0;

        foreach (var movement in _movements)
        {
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
        }

        return (x * y).ToString();
    }

    private record Movement(string Direction, int Amount);
}