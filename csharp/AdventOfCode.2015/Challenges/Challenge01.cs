using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(1)]
public class Challenge01
{
    private readonly IInputReader _inputReader;

    public Challenge01(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async() =>
        (await _inputReader.ReadLineAsync(1)
            .SumAsync(b => b == '(' ? 1 : -1))
        .ToString();

    [Part2]
    public async Task<string?> Part2Async()
    {
        var floor = 0;
        var position = 0;
        await foreach (var c in _inputReader.ReadLineAsync(1))
        {
            position++;
            floor += c == '(' ? 1 : -1;

            if (floor == -1)
                return position.ToString();
        }

        return "No result";
    }
}