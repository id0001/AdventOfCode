using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (a, b) = ParseInput(await inputReader.ReadAllTextAsync(24));
        return string.Empty;
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
    }

    private (Legion[] ImmuneSystem, Legion[] Infection) ParseInput(string input)
    {
        var nl = Environment.NewLine;
        var (a,b,_) = input.SplitBy($"{nl}{nl}").Select(ParseArmy);
        return (a, b)!;
    }

    private static Legion[] ParseArmy(string input) => input.SplitBy(Environment.NewLine).Skip(1).Select(ParseLegion).ToArray();

    private static Legion ParseLegion(string text)
    {
        var result = text.Extract(
            @"(\d+) units each with (\d+) hit points (?:\(([^)]+)\) )?with an attack that does (\d+) ([^ ]+) damage at initiative (\d+)");

        var units = result[0].As<int>();
        var hp = result[1].As<int>();
        var weakImmune = result[2];
        var ad = result[3].As<int>();
        var attackType = result[4];
        var initiative = result[5].As<int>();
        
        var (weak, immune) = ParseWeaknessAndImmunities(weakImmune);
        return new Legion(units, hp, ad, initiative, attackType, weak, immune);
    }

    private static (string[] Weak, string[] Immune) ParseWeaknessAndImmunities(string input)
    {
        var items = input.SplitBy(";");
        if (items.Length == 0)
            return ([], []);

        return items.Length switch
        {
            1 when items[0].StartsWith("weak to") => (ParseWeaknesses(items[0]), []),
            1 => ([], ParseImmunities(items[0])),
            _ => items[0].StartsWith("weak to")
                ? (ParseWeaknesses(items[0]), ParseImmunities(items[1]))
                : (ParseWeaknesses(items[1]), ParseImmunities(items[0]))
        };
    }

    private static string[] ParseWeaknesses(string input) => input["weak to".Length..].SplitBy(",").ToArray();

    private static string[] ParseImmunities(string input) => input["immune to".Length..].SplitBy(",").ToArray();

    private record Legion(
        int Units,
        int Hp,
        int Ad,
        int Initiative,
        string AttackType,
        string[] Weakness,
        string[] Immunity);
}