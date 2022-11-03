using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(16)]
public class Challenge16
{
    private static readonly (string, int, Func<int, int, bool>)[] Signature =
    {
        ("children", 3, (a, b) => a == b),
        ("cats", 7, (a, b) => a > b),
        ("samoyeds", 2, (a, b) => a == b),
        ("pomeranians", 3, (a, b) => a < b),
        ("akitas", 0, (a, b) => a == b),
        ("vizslas", 0, (a, b) => a == b),
        ("goldfish", 5, (a, b) => a < b),
        ("trees", 3, (a, b) => a > b),
        ("cars", 2, (a, b) => a == b),
        ("perfumes", 1, (a, b) => a == b)
    };

    private static readonly Regex Pattern = new(@"^Sue (\d+):(.+)$");

    private readonly IInputReader _inputReader;

    public Challenge16(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async() =>
        (await _inputReader.ParseLinesAsync(16, ParseLine)
            .OrderByDescending(s => MatchCount(s, Signature.Select(x => x.Item2))).FirstAsync())
        .Number.ToString();

    [Part2]
    public async Task<string?> Part2Async() => (await _inputReader.ParseLinesAsync(16, ParseLine)
            .OrderByDescending(s => MatchCount2(s, Signature)).FirstAsync())
        .Number.ToString();

    private static int MatchCount(Sue sue, IEnumerable<int> expected) =>
        sue.Values.Zip(expected).Count(x => x.First == x.Second);

    private static int MatchCount2(Sue sue,
        IEnumerable<(string Name, int Expected, Func<int, int, bool> Compare)> signature) => sue.Values
        .Zip(signature)
        .Count(x => x.First != -1 && x.Second.Compare(x.First, x.Second.Expected));

    private static Sue ParseLine(string line)
    {
        var match = Pattern.Match(line);

        var number = int.Parse(match.Groups[1].Value);
        var values = new int[10];
        Array.Fill(values, -1);

        var items = match.Groups[2].Value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var item in items)
        {
            var split = item.Split(": ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var i = Array.FindIndex(Signature, ((string Name, int, Func<int, int, bool>) x) => x.Name == split[0]);
            values[i] = int.Parse(split[1]);
        }

        return new Sue(number, values);
    }

    private record Sue(int Number, int[] Values);
}