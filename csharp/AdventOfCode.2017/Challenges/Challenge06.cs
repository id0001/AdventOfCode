using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var banks = (await inputReader.ReadAllTextAsync(6)).SplitBy("\t").As<int>().ToArray();

        var visited = new HashSet<string> {string.Join(",", banks)};
        var cycle = 0;

        while (true)
        {
            var (i, blocks) = Enumerable.Range(0, 16).Zip(banks).MaxBy(pair => pair.Second);
            banks[i] = 0;

            while (blocks > 0)
            {
                banks[(i + 1).Mod(16)]++;
                blocks--;
                i++;
            }

            cycle++;

            var text = string.Join(",", banks);
            if (visited.Contains(text))
                return cycle.ToString();

            visited.Add(text);
        }
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var banks = (await inputReader.ReadAllTextAsync(6)).SplitBy("\t").As<int>().ToArray();

        var visited = new OrderedDictionary<string, int>
        {
            {string.Join(",", banks), 0}
        };

        var cycle = 0;

        while (true)
        {
            var (i, blocks) = Enumerable.Range(0, 16).Zip(banks).MaxBy(pair => pair.Second);
            banks[i] = 0;

            while (blocks > 0)
            {
                banks[(i + 1).Mod(16)]++;
                blocks--;
                i++;
            }

            cycle++;

            var text = string.Join(",", banks);
            if (visited.ContainsKey(text))
                return (cycle - visited[text]).ToString();

            visited.Add(text, cycle);
        }
    }
}