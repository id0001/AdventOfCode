using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(2)]
public class Challenge02
{
    private readonly IInputReader _inputReader;

    public Challenge02(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var sum = 0;
        await foreach (var game in _inputReader.ParseLinesAsync(2, ParseLine))
            if (IsPossible(game))
                sum += game.Number;

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var sum = 0;
        await foreach (var game in _inputReader.ParseLinesAsync(2, ParseLine))
            sum += GetPowerOfMinimumNumberOfCubes(game);

        return sum.ToString();
    }

    private bool IsPossible(Game game)
    {
        int[] max = { 12, 13, 14 };
        foreach (var set in game.Sets)
            if (set.Zip(max).Any(x => x.First > x.Second))
                return false;

        return true;
    }

    private int GetPowerOfMinimumNumberOfCubes(Game game)
    {
        return game.Sets.Aggregate(new int[3],
                (a, b) => new[] { Math.Max(a[0], b[0]), Math.Max(a[1], b[1]), Math.Max(a[2], b[2]) })
            .Product();
    }

    private Game ParseLine(string line)
    {
        // Game 1: 3 blue, 2 green, 6 red; 17 green, 4 red, 8 blue; 2 red, 1 green, 10 blue; 1 blue, 5 green

        return line.SplitBy(":")
            .Into(parts => new Game
            {
                Number = parts.First().SplitBy(" ").Second().As<int>(),
                Sets = parts
                    .Second()
                    .SplitBy(";")
                    .Select(x => x
                        .SplitBy(",")
                        .Into(set => Increment(new int[3], set)))
                    .ToList()
            });
    }

    private int[] Increment(int[] set, IList<string> value)
    {
        foreach (var item in value.Select(x => x.SplitBy(" ")))
            set[RgbToIndex(item.Second())] += item.First().As<int>();

        return set;
    }

    private static int RgbToIndex(string color) => color switch
    {
        "red" => 0,
        "green" => 1,
        "blue" => 2,
        _ => throw new NotImplementedException()
    };

    private record Game
    {
        public required int Number { get; init; }
        public required List<int[]> Sets { get; init; }
    }
}