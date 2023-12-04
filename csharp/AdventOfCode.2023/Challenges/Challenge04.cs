using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(4)]
public class Challenge04
{
    private readonly IInputReader _inputReader;

    public Challenge04(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        return await _inputReader.ParseLinesAsync(4, ParseLine)
            .Select(card => card.Score)
            .SumAsync()
            .ToStringAsync();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lookup = await _inputReader.ParseLinesAsync(4, ParseLine).ToDictionaryAsync(kv => kv.Id, kv => kv.AmountOfWinningNumbers);
        return lookup.Keys.Sum(c => CountCards(c, lookup)).ToString();
    }

    private int CountCards(int id, Dictionary<int, int> lookup)
    {
        return 1 + Enumerable.Range(id + 1, lookup[id])
            .Where(lookup.ContainsKey)
            .Sum(c => CountCards(c, lookup));
    }

    private ScratchCard ParseLine(string line)
    {
        return line
            .SplitBy(":", x => new ScratchCard
            {
                Id = int.Parse(x.First.SplitBy(" ").Second),
                WinningNumbers = x.Second.SplitBy("|").First().SplitBy(" ").Select(int.Parse).ToArray(),
                Numbers = x.Second.SplitBy("|").Second.SplitBy(" ").Select(int.Parse).ToArray(),
            });
    }

    private record ScratchCard
    {
        private int amountOfWinningNumbers = -1;

        public required int Id { get; init; }
        public required int[] WinningNumbers { get; init; }
        public required int[] Numbers { get; init; }

        public int AmountOfWinningNumbers => amountOfWinningNumbers < 0 ? amountOfWinningNumbers = Numbers.Intersect(WinningNumbers).Count() : amountOfWinningNumbers;

        public int Score
        {
            get
            {
                if (AmountOfWinningNumbers == 0)
                    return 0;

                return (int)Math.Pow(2, AmountOfWinningNumbers - 1);
            }
        }
    }
}
