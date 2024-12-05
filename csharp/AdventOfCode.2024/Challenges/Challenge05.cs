using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (rules, lines) = await inputReader.ParseTextAsync(5, ParseInput);

        var correct = lines.Where(l => IsCorrect(rules, l)).ToArray();

        return lines.Where(l => IsCorrect(rules, l)).Select(l => l[l.Length/2]).Sum().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (rules, lines) = await inputReader.ParseTextAsync(5, ParseInput);

        var incorrect = lines.Where(l => !IsCorrect(rules, l)).Select(l => l.ToList()).ToList();

        for (var i = 0; i < incorrect.Count; i++)
        {
            var j = 0;
            while(j < incorrect[i].Count-1)
            {
                for (var k = j + 1; k < incorrect[i].Count; k++)
                {
                    if (rules.Any(r => r.Before == incorrect[i][k] && r.After == incorrect[i][j]))
                    {
                        var n = incorrect[i][j];
                        incorrect[i].Insert(k + 1, n);
                        incorrect[i].RemoveAt(j);
                        continue;
                    }

                    j++;
                }
            }

            if (!IsCorrect(rules, incorrect[i]))
                i--;
        }
        
        
        return incorrect.Select(l => l[l.Count/2]).Sum().ToString();
    }

    private static bool IsCorrect(SortRule[] rules, IList<int> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (var j = i+1; j < items.Count; j++)
            {
                if (rules.Any(r => r.Before == items[j] && r.After == items[i]))
                    return false;
            }
        }

        return true;
    }

    private static (SortRule[], int[][]) ParseInput(string input) => input.SelectParagraphs()
        .Into(parts => (
            parts[0].SelectLines()
                .Select(x => x.SplitBy<int, int>("|")
                    .Into(n => new SortRule(n.First, n.Second))).ToArray(),
            parts[1].SelectLines().Select(l => l.SplitBy(",").As<int>().ToArray()).ToArray()
        ));

    private record SortRule(int Before, int After);
}
