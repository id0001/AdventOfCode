using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(13)]
public class Challenge13
{
    private static readonly Regex Pattern = new(@"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)\.");
    
    private readonly IInputReader _inputReader;

    public Challenge13(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var map = await _inputReader.ParseLinesAsync(13, ParseLine).ToDictionaryAsync(kv => (kv.From, kv.To), kv => kv.Units);

        var people = map.Select(x => x.Key.From).ToHashSet();

        var mostGained = 0;
        foreach (var permutation in people.QuickPerm())
        {
            var gained = 0;
            foreach (var (curr, next) in permutation.CurrentAndNext(true))
                gained += map[(curr, next)] + map[(next, curr)];
            
            mostGained = Math.Max(mostGained, gained);
        }

        return mostGained.ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var map = await _inputReader.ParseLinesAsync(13, ParseLine).ToDictionaryAsync(kv => (kv.From, kv.To), kv => kv.Units);
        var people = map.Select(x => x.Key.From).ToHashSet();
        foreach (var person in people)
        {
            map.Add(("Me",person), 0);
            map.Add((person,"Me"), 0);
        }

        people.Add("Me");

        var mostGained = 0;
        foreach (var permutation in people.QuickPerm())
        {
            var gained = 0;
            foreach (var (curr, next) in permutation.CurrentAndNext(true))
                gained += map[(curr, next)] + map[(next, curr)];

            mostGained = Math.Max(mostGained, gained);
        }

        return mostGained.ToString();
    }

    private static MatchUp ParseLine(string line)
    {
        var match = Pattern.Match(line);
        
        var from = match.Groups[1].Value;
        var gain = match.Groups[2].Value == "gain";
        var units = int.Parse(match.Groups[3].Value) * (gain ? 1 : -1);
        var to = match.Groups[4].Value;

        return new MatchUp(from, to, units);
    }

    private record MatchUp(string From, string To, int Units);
}