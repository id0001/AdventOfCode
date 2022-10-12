using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2015.Challenges;

[Challenge(17)]
public class Challenge17
{
    private readonly IInputReader _inputReader;

    public Challenge17(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var items = await _inputReader.ReadLinesAsync<int>(17).ToArrayAsync();

        var combinations = Enumerable
            .Range(1, items.Length+1)
            .SelectMany(k => Combinatorics.SelectAllCombinations(items, k))
            .Count(c => c.Sum() == 150);

        return combinations.ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var items = await _inputReader.ReadLinesAsync<int>(17).ToArrayAsync();

        for (var i = 1; i <= items.Length; i++)
        {
            var combinations = Combinatorics
                .SelectAllCombinations(items, i)
                .Count(c => c.Sum() == 150);

            if (combinations > 0)
                return combinations.ToString();
        }

        return "No combinations found";
    }
}