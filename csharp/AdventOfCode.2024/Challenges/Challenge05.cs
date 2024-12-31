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
        return lines
            .Where(l => IsCorrect(rules, l))
            .Sum(l => l[l.Length / 2])
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (rules, lines) = await inputReader.ParseTextAsync(5, ParseInput);

        var comparer = new InputComparer(rules);

        return lines
            .Where(l => !IsCorrect(rules, l))
            .Select(l => l.Order(comparer).ToArray())
            .Sum(l => l[l.Length / 2])
            .ToString();
    }

    private static bool IsCorrect(SortRule[] rules, int[] items)
    {
        for (var i = 0; i < items.Length; i++)
        {
            var i1 = i;
            if (rules.Where(r => r.After == items[i1]).IntersectBy(items[(i + 1)..], r => r.Before).Any())
                return false;
        }

        return true;
    }

    private static (SortRule[], List<int[]>) ParseInput(string input) => input.SelectParagraphs()
        .Into(parts => (
            parts[0].SelectLines()
                .Select(x => x.SplitBy<int, int>("|")
                    .Into(n => new SortRule(n.First, n.Second))).ToArray(),
            parts[1].SelectLines().Select(l => l.SplitBy(",").As<int>().ToArray()).ToList()
        ));

    private record SortRule(int Before, int After);

    private class InputComparer(IList<SortRule> sortRules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == y)
                return 0;

            return sortRules.Any(rule => rule.Before == y && rule.After == x) ? 1 : -1;
        }
    }
}