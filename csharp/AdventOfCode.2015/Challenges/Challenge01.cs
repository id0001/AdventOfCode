using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string?> Part1Async() =>
        (await inputReader.ReadLineAsync(1)
            .SumAsync(b => b == '(' ? 1 : -1))
        .ToString();

    [Part2]
    public async Task<string?> Part2Async()
    {
        var floor = 0;
        var position = 0;
        await foreach (var c in inputReader.ReadLineAsync(1))
        {
            position++;
            floor += c == '(' ? 1 : -1;

            if (floor == -1)
                return position.ToString();
        }

        return "No result";
    }
}