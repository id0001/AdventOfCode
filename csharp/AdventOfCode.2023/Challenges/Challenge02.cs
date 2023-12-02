using System.Text.RegularExpressions;
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
        int sum = 0;
        await foreach (var game in _inputReader.ParseLinesAsync(2, ParseLine))
        {
            if (IsPossible(game))
                sum += game.Number;
        }
        
        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        int sum = 0;
        await foreach (var game in _inputReader.ParseLinesAsync(2, ParseLine))
        {
            sum += GetPowerOfMinimumNumberOfCubes(game);
        }
        
        return sum.ToString();
    }

    private bool IsPossible(Game game)
    {
        int[] max = {12, 13, 14};
        foreach (var set in game.Sets)
        {
            if (set.Zip(max).Any(x => x.First > x.Second))
                return false;
        }

        return true;
    }

    private int GetPowerOfMinimumNumberOfCubes(Game game) => game.Sets.Aggregate(new[] {0, 0, 0},
            (a, b) => new[] {Math.Max(a[0], b[0]), Math.Max(a[1], b[1]), Math.Max(a[2], b[2])})
        .Product();

    private Game ParseLine(string line)
    {
        // Game 1: 3 blue, 2 green, 6 red; 17 green, 4 red, 8 blue; 2 red, 1 green, 10 blue; 1 blue, 5 green

        var split = line.Split(new[] {":"}, StringSplitOptions.TrimEntries);
        var gamePattern = new Regex(@"Game (\d+)", RegexOptions.Compiled);
        var match = gamePattern.Match(split[0]);
        int game = int.Parse(match.Groups[1].Value);

        var sets = ParseSets(split[1]);
        return new Game(game, sets);
    }

    private List<int[]> ParseSets(string line)
    {
        var split = line.Split(new[] {";"}, StringSplitOptions.TrimEntries);
        return split.Select(ParseSet).ToList();
    }

    private int[] ParseSet(string line)
    {
        var split = line.Split(new[] {","}, StringSplitOptions.TrimEntries);
        var set = new int[3];

        foreach (var item in split)
        {
            var itemSplit = item.Split(' ');
            switch (itemSplit[1])
            {
                case "red":
                    set[0] += int.Parse(itemSplit[0]);
                    break;
                case "green":
                    set[1] += int.Parse(itemSplit[0]);
                    break;
                case "blue":
                    set[2] += int.Parse(itemSplit[0]);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return set;
    }

    private record Game(int Number, List<int[]> Sets);
}
