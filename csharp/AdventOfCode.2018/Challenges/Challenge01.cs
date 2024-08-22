using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => await inputReader.ReadLinesAsync<int>(1).SumAsync().ToStringAsync();

    [Part2]
    public async Task<string> Part2Async()
    {
        var visited = new HashSet<int>([0]);
        var f = 0;
        await foreach(var change in inputReader.ReadLinesAsync<int>(1).Cycle())
        {
            f += change;
            if (visited.Contains(f))
                return f.ToString();

            visited.Add(f);
        }

        return "err";
    }
}
