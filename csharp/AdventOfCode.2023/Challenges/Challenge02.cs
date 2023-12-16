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
        int[] max = {12, 13, 14};
        foreach (var set in game.Sets)
            if (set.Zip(max).Any(x => x.First > x.Second))
                return false;

        return true;
    }

    private int GetPowerOfMinimumNumberOfCubes(Game game)
    {
        return game.Sets.Aggregate(new int[3],
                (a, b) => new[] {Math.Max(a[0], b[0]), Math.Max(a[1], b[1]), Math.Max(a[2], b[2])})
            .Product();
    }

    private Game ParseLine(string line)
    {
        // Game 1: 3 blue, 2 green, 6 red; 17 green, 4 red, 8 blue; 2 red, 1 green, 10 blue; 1 blue, 5 green

        return line.SplitBy(":")
            .Transform(parts => new Game
            {
                Number = int.Parse(parts.First().SplitBy(" ").Second()),
                Sets = parts
                    .Second()
                    .SplitBy(";")
                    .Select(x => x
                        .SplitBy(",")
                        .Transform(set => Increment(new int[3], set)))
                    .ToList()
            });
    }

    private int[] Increment(int[] set, IList<string> value)
    {
        foreach (var item in value.Select(x => x.SplitBy(" ")))
            switch (item.Second())
            {
                case "red":
                    set[0] += int.Parse(item.First());
                    break;
                case "green":
                    set[1] += int.Parse(item.First());
                    break;
                case "blue":
                    set[2] += int.Parse(item.First());
                    break;
                default:
                    throw new NotImplementedException();
            }

        return set;
    }

    private record Game
    {
        public required int Number { get; init; }
        public required List<int[]> Sets { get; init; }
    }
}