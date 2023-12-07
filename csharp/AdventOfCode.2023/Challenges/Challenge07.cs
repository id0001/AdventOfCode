using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(7)]
public class Challenge07
{
    private static readonly Dictionary<char, int> CardStrength = new()
    {
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'J', 11},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    private static readonly Dictionary<char, int> CardStrength2 = new()
    {
        {'J', 1},
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    private readonly IInputReader _inputReader;

    public Challenge07(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        return await _inputReader.ParseLinesAsync(7, ParseLine)
            .OrderBy(x => CardStrength[x.Cards[4]])
            .OrderBy(x => CardStrength[x.Cards[3]])
            .OrderBy(x => CardStrength[x.Cards[2]])
            .OrderBy(x => CardStrength[x.Cards[1]])
            .OrderBy(x => CardStrength[x.Cards[0]])
            .OrderBy(x => Score(x.Cards))
            .Select((x, i) => (i + 1) * x.Bid)
            .SumAsync()
            .ToStringAsync();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var rank = 1;
        await foreach (var hand in _inputReader.ParseLinesAsync(7, ParseLine)
                           .OrderBy(x => CardStrength2[x.Cards[4]])
                           .OrderBy(x => CardStrength2[x.Cards[3]])
                           .OrderBy(x => CardStrength2[x.Cards[2]])
                           .OrderBy(x => CardStrength2[x.Cards[1]])
                           .OrderBy(x => CardStrength2[x.Cards[0]])
                           .OrderBy(x => Score2(x.Cards)))
        {
            Console.WriteLine($"{hand}: {rank * hand.Bid}");
            rank++;
        }

        return await _inputReader.ParseLinesAsync(7, ParseLine)
            .OrderBy(x => CardStrength2[x.Cards[4]])
            .OrderBy(x => CardStrength2[x.Cards[3]])
            .OrderBy(x => CardStrength2[x.Cards[2]])
            .OrderBy(x => CardStrength2[x.Cards[1]])
            .OrderBy(x => CardStrength2[x.Cards[0]])
            .OrderBy(x => Score2(x.Cards))
            .Select((x, i) => (i + 1) * x.Bid)
            .SumAsync()
            .ToStringAsync();
    }

    private Hand ParseLine(string line)
    {
        return line.SplitBy(" ", parts => new Hand(
            parts.First,
            int.Parse(parts.Second)
        ));
    }

    private static int Score(string cards)
    {
        var values = cards.GroupBy(c => c).ToDictionary(kv => kv.Key, kv => kv.Count());
        if (values.Count == 1)
            return 6;

        if (values.ContainsValue(4))
            return 5;

        if (values.ContainsValue(3) && values.ContainsValue(2))
            return 4;

        if (values.ContainsValue(3))
            return 3;

        if (values.Values.Count(x => x == 2) == 2)
            return 2;

        if (values.ContainsValue(2))
            return 1;

        return 0;
    }

    private static int Score2(string cards)
    {
        var values = cards.GroupBy(c => c).ToDictionary(kv => kv.Key, kv => kv.Count());

        values.TryGetValue('J', out var jokerCount);

        if (values.Count == 1)
            return 6;

        if (values.ContainsValue(4))
        {
            if (jokerCount > 0)
                return 6;

            return 5;
        }

        if (values.ContainsValue(3) && values.ContainsValue(2))
        {
            if (jokerCount > 0)
                return 6;

            return 4;
        }

        if (values.ContainsValue(3))
        {
            if (jokerCount > 0)
                return 5;

            return 3;
        }

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