using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadLinesAsync<int>(10).OrderBy(e => e).ToArrayAsync();

        var differences = new[] {0, 0, 1};

        var prev = 0;
        foreach (var adapter in input)
        {
            differences[adapter - prev - 1]++;
            prev = adapter;
        }

        return (differences[0] * differences[2]).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadLinesAsync<int>(10).OrderBy(e => e).ToArrayAsync();

        var max = input[^1] + 3;
        var set = input.Prepend(0).Append(max).ToArray();
        var groupCount = new[] {0, 0, 0};

        var c = 0;
        for (var i = 0; i < set.Length; i++)
        {
            if (i == 0 || i == set.Length - 1 || set[i] - set[i - 1] == 3 || set[i + 1] - set[i - 1] > 3)
            {
                if (c > 0)
                    groupCount[c - 1]++;

                c = 0;
                continue;
            }

            c++;
        }

        if (c > 0)
            groupCount[c - 1]++;

        var count = (long) Math.Pow(2, groupCount[0]) * (long) Math.Pow(4, groupCount[1]) *
                    (long) Math.Pow(7, groupCount[2]);
        return count.ToString();
    }
}