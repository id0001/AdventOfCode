using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader InputReader)
{
    [Part1]
    public async Task<string?> Part1Async() => await InputReader.ParseLinesAsync(7, ParseLine)
        .OrderBy(x => Strength(x.Cards[4], false))
        .OrderBy(x => Strength(x.Cards[3], false))
        .OrderBy(x => Strength(x.Cards[2], false))
        .OrderBy(x => Strength(x.Cards[1], false))
        .OrderBy(x => Strength(x.Cards[0], false))
        .OrderBy(x => Score(x.Cards, false))
        .Select((x, i) => (i + 1) * x.Bid)
        .SumAsync()
        .ToStringAsync();

    [Part2]
    public async Task<string?> Part2Async() => await InputReader.ParseLinesAsync(7, ParseLine)
        .OrderBy(x => Strength(x.Cards[4], true))
        .OrderBy(x => Strength(x.Cards[3], true))
        .OrderBy(x => Strength(x.Cards[2], true))
        .OrderBy(x => Strength(x.Cards[1], true))
        .OrderBy(x => Strength(x.Cards[0], true))
        .OrderBy(x => Score(x.Cards, true))
        .Select((x, i) => (i + 1) * x.Bid)
        .SumAsync()
        .ToStringAsync();

    private Hand ParseLine(string line) =>
        line.SplitBy(" ")
            .Into(parts => new Hand(
                parts.First(),
                parts.Second().As<int>()
            ));

    private static int Strength(char c, bool useJokers)
    {
        if (useJokers && c == 'J')
            return 0;

        return c switch
        {
            '2' => 1,
            '3' => 2,
            '4' => 3,
            '5' => 4,
            '6' => 5,
            '7' => 6,
            '8' => 7,
            '9' => 8,
            'T' => 9,
            'J' => 10,
            'Q' => 11,
            'K' => 12,
            'A' => 13,
            _ => throw new NotImplementedException()
        };
    }

    private static int Score(string cards, bool useJokers)
    {
        var values = cards.GroupBy(c => c).ToDictionary(kv => kv.Key, kv => kv.Count());

        var jokerCount = 0;
        if (useJokers)
            values.TryGetValue('J', out jokerCount);

        if (values.Count == 1)
            return 6;

        if (values.ContainsValue(4))
            return jokerCount > 0 ? 6 : 5;

        if (values.ContainsValue(3) && values.ContainsValue(2))
            return jokerCount > 0 ? 6 : 4;

        if (values.ContainsValue(3))
            return jokerCount > 0 ? 5 : 3;

        if (values.Values.Count(x => x == 2) == 2)
            return jokerCount switch
            {
                2 => 5,
                1 => 4,
                _ => 2
            };

        if (values.ContainsValue(2))
            return jokerCount > 0 ? 3 : 1;

        return jokerCount > 0 ? 1 : 0;
    }

    private record Hand(string Cards, int Bid);
}