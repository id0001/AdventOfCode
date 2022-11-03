using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(1)]
public class Challenge01
{
    private readonly IInputReader _inputReader;

    public Challenge01(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await _inputReader.ReadLinesAsync<int>(1).ToArrayAsync();

        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input.Length; x++)
        {
            if (x == y)
                continue;

            if (input[x] + input[y] == 2020)
                return (input[x] * input[y]).ToString();
        }

        return "-1";
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await _inputReader.ReadLinesAsync<int>(1).ToArrayAsync();

        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input.Length; x++)
        for (var z = 0; z < input.Length; z++)
        {
            if (x == y || x == z || y == z)
                continue;

            if (input[x] + input[y] + input[z] == 2020)
                return (input[x] * input[y] * input[z]).ToString();
        }

        return "-1";
    }
}