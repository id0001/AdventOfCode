using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadLinesAsync<int>(1).ToArrayAsync();
        return Enumerable.Range(1, input.Length - 1).Aggregate(0, (a, i) => a + (input[i] > input[i - 1] ? 1 : 0))
            .ToString();
    }


    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadLinesAsync<int>(1).ToArrayAsync();
        return Enumerable.Range(1, input.Length - 3).Aggregate(0,
            (a, i) => a + (input[i] + input[i + 1] + input[i + 2] > input[i - 1] + input[i] + input[i + 1]
                ? 1
                : 0)).ToString();
    }
}