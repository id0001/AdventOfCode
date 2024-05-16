using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var list = await inputReader.ReadLinesAsync(5).Select(x => x.As<int>()).ToArrayAsync();

        var ip = 0;
        var steps = 0;
        while (ip < list.Length)
        {
            ip += list[ip]++;
            steps++;
        }

        return steps.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var list = await inputReader.ReadLinesAsync(5).Select(x => x.As<int>()).ToArrayAsync();

        var ip = 0;
        var steps = 0;
        while (ip < list.Length)
        {
            var v = ip;
            ip += list[ip];
            list[v] += list[v] >= 3 ? -1 : 1;
            steps++;
        }

        return steps.ToString();
    }
}