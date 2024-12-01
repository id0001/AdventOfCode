using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (a, b) = await inputReader.ParseTextAsync(1, ParseText);
        return a.Zip(b).Select(x => Math.Abs(x.First - x.Second)).Sum().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (a, b) = await inputReader.ParseTextAsync(1, ParseText);
        var lookup = b.GroupBy(x => x).ToDictionary(kv => kv.Key, kv => kv.Count());
        return a.Select(x => lookup.GetValueOrDefault(x, 0) * x).Sum().ToString();
    }

    private static (List<int>, List<int>) ParseText(string input)
    {
        var (a, b) = input
            .SelectLines()
            .Select(line => line.SplitBy<int, int>(" "))
            .Unzip();

        return ([.. a.Order()], [.. b.Order()]);
    }
}
