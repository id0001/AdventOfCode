using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2015.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader inputReader)
{
    private static readonly Regex Pattern =
        new(@"\w+: capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)");

    [Part1]
    public async Task<string?> Part1Async()
    {
        var list = await inputReader.ParseLinesAsync(15, ParseLine).ToListAsync();

        var score = 0;
        foreach (var distribution in Combinatorics.GenerateAllPartitions(100, list.Count))
        {
            var zip = list.Zip(distribution).ToList();
            var cap = Math.Max(0, zip.Sum(x => x.First.Capacity * x.Second));
            var dur = Math.Max(0, zip.Sum(x => x.First.Durability * x.Second));
            var fla = Math.Max(0, zip.Sum(x => x.First.Flavor * x.Second));
            var tex = Math.Max(0, zip.Sum(x => x.First.Texture * x.Second));
            score = Math.Max(score, cap * dur * fla * tex);
        }

        return score.ToString();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var list = await inputReader.ParseLinesAsync(15, ParseLine).ToListAsync();

        var score = 0;
        foreach (var distribution in Combinatorics.GenerateAllPartitions(100, list.Count))
        {
            var zip = list.Zip(distribution).ToList();
            var cap = Math.Max(0, zip.Sum(x => x.First.Capacity * x.Second));
            var dur = Math.Max(0, zip.Sum(x => x.First.Durability * x.Second));
            var fla = Math.Max(0, zip.Sum(x => x.First.Flavor * x.Second));
            var tex = Math.Max(0, zip.Sum(x => x.First.Texture * x.Second));
            var cal = Math.Max(0, zip.Sum(x => x.First.Calories * x.Second));

            if (cal == 500)
                score = Math.Max(score, cap * dur * fla * tex);
        }

        return score.ToString();
    }

    private static Ingredient ParseLine(string line)
    {
        var match = Pattern.Match(line);

        var cap = int.Parse(match.Groups[1].Value);
        var dur = int.Parse(match.Groups[2].Value);
        var fla = int.Parse(match.Groups[3].Value);
        var tex = int.Parse(match.Groups[4].Value);
        var cal = int.Parse(match.Groups[5].Value);
        return new Ingredient(cap, dur, fla, tex, cal);
    }

    private record Ingredient(int Capacity, int Durability, int Flavor, int Texture, int Calories);
}