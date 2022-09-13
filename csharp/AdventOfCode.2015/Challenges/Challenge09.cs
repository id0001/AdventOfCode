using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(9)]
public class Challenge09
{
    private readonly IInputReader _inputReader;

    public Challenge09(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var lookup = new Dictionary<(string, string), int>();
        
        await foreach(var (a, b, distance) in _inputReader.ParseLinesAsync(9, ParseLine))
        {
            lookup.Add((a,b), distance);
            lookup.Add((b,a), distance);
        }

        var cities = lookup.Keys.Select(ab => ab.Item1).Distinct().ToArray();
        var min = cities.QuickPerm().Min(p => CalcDistance(lookup, p));

        return min.ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var lookup = new Dictionary<(string, string), int>();
        
        await foreach(var (a, b, distance) in _inputReader.ParseLinesAsync(9, ParseLine))
        {
            lookup.Add((a,b), distance);
            lookup.Add((b,a), distance);
        }

        var cities = lookup.Keys.Select(ab => ab.Item1).Distinct().ToArray();
        var max = cities.QuickPerm().Max(p => CalcDistance(lookup, p));

        return max.ToString();
    }

    private static int CalcDistance(IDictionary<(string, string), int> routes, IEnumerable<string> cities)
    {
        var dist = 0;
        foreach (var (current, next) in cities.CurrentAndNext())
        {
            dist += routes[(current, next)];
        }

        return dist;
    }
    
    private static (string, string, int) ParseLine(string line)
    {
        var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var a = split[0];
        var b = split[2];
        var distance = int.Parse(split[4]);
        return (a, b, distance);
    }
}