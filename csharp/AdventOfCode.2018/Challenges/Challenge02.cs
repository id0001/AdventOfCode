using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var c2 = 0;
        var c3 = 0;
        await foreach (var groups in inputReader.ReadLinesAsync(2).Select(line =>
                           line.ToCharArray().GroupBy(c => c).Select(g => new {Character = g.Key, Count = g.Count()})
                               .ToArray()))
        {
            if (groups.Any(g => g.Count == 2))
                c2++;

            if (groups.Any(g => g.Count == 3))
                c3++;
        }

        return (c2 * c3).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await inputReader.ReadLinesAsync(2).ToListAsync();
        var correct = lines.Combinations(2).First(c => c.First().HammingDistance(c.Second()) == 1);

        return ExtractCommon(correct.First(), correct.Second());
    }

    private static string ExtractCommon(string a, string b) =>
        a.Zip(b).Where(pair => pair.First == pair.Second).Select(pair => pair.First).AsString();
}