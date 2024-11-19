using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    private static readonly string DoubleNewLine = $"{Environment.NewLine}{Environment.NewLine}";

    [Part1]
    public async Task<string> Part1Async()
    {
        var (immuneSystem, infected) = ParseInput(await inputReader.ReadAllTextAsync(24));
        Army winner = Fight(immuneSystem, infected);

        return winner.Groups.Sum(g => g.Units).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {

        int boost = 1570;
        Army winner = null!;
        for (; ; boost++)
        {
            var (immuneSystem, infected) = ParseInput(await inputReader.ReadAllTextAsync(0));
            foreach (var group in immuneSystem.Groups)
                group.Boost = boost;

            winner = Fight(immuneSystem, infected);
            if (winner.Id == "Immune System")
                break;
        }

        //SpecialFunctions.BinarySearch(1, 1_000_000, )
        return winner.Groups.Sum(g => g.Units).ToString();
    }

    private static Army Fight(Army immuneSystem, Army infected)
    {
        while (!immuneSystem.Groups.All(g => g.Units == 0) && !infected.Groups.All(g => g.Units == 0))
        {
            var battles = ChooseTargets(immuneSystem, infected).Concat(ChooseTargets(infected, immuneSystem)).ToList();

            foreach (var battle in battles.OrderByDescending(t => t.Attacker.Initiative))
            {
                var dmg = CalculateDamage(battle.Attacker, battle.Defender);
                var losses = Math.Min(battle.Defender.Units, dmg / battle.Defender.Hp);
                battle.Defender.Units -= losses;
            }
        }

        var winner = immuneSystem.Groups.All(g => g.Units == 0) ? infected : immuneSystem;
        return winner;
    }

    private static IEnumerable<Target> ChooseTargets(Army attackers, Army defenders)
    {
        var chosen = new List<Group>();
        foreach (var attacker in attackers.Groups.Where(g => g.Units > 0).OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
        {
            var choices = defenders.Groups
                .Where(defender => defender.Units > 0 && CalculateDamage(attacker, defender) > 0)
                .Except(chosen)
                .OrderByDescending(defender => CalculateDamage(attacker, defender))
                .ThenByDescending(defender => defender.EffectivePower)
                .ThenByDescending(defender => defender.Initiative)
                .ToList();

            if (choices.Count == 0)
                continue;

            chosen.Add(choices.First());
            yield return new Target(attacker, choices.First());
        }
    }

    private static int CalculateDamage(Group attacker, Group defender)
    {
        if (attacker.Units == 0)
            return 0;

        if (defender.Immunity.Contains(attacker.AttackType))
            return 0;

        int dmg = attacker.Units * attacker.EffectivePower;
        if (defender.Weakness.Contains(attacker.AttackType))
            dmg *= 2;

        return dmg;
    }

    private static (Army, Army) ParseInput(string input)
        => input.SplitBy(DoubleNewLine).Select(ParseArmy).ToList().Into(armies => (armies.First(), armies.Second()));

    private static Army ParseArmy(string input)
        => input.SplitBy(Environment.NewLine).Into(lines => new Army(lines.First().TrimEnd(':'), lines.Skip(1).Select((line, i) => ParseGroup(line, lines.First().TrimEnd(':'), i + 1)).ToArray()));

    private static Group ParseGroup(string input, string army, int id)
    {
        var result = input.Extract(
            @"(\d+) units each with (\d+) hit points (?:\(([^)]+)\) )?with an attack that does (\d+) ([^ ]+) damage at initiative (\d+)");

        var units = result[0].As<int>();
        var hp = result[1].As<int>();
        var weakAndImmune = result[2];
        var ad = result[3].As<int>();
        var attackType = result[4];
        var initiative = result[5].As<int>();

        var (weak, immune) = ParseWeaknessesAndImmunities(weakAndImmune);
        return new Group
        {
            Army = army,
            Id = id,
            Units = units,
            Hp = hp,
            AttackDamage = ad,
            Initiative = initiative,
            AttackType = attackType,
            Weakness = weak,
            Immunity = immune
        };
    }

    private static (string[] Weak, string[] Immune) ParseWeaknessesAndImmunities(string input)
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

    private record Target(Group Attacker, Group Defender);

    private record Army(string Id, Group[] Groups);

    private class Group
    {
        public int EffectivePower => Units * (AttackDamage + Boost);
        public required string Army { get; init; }
        public required int Id { get; init; }
        public required int Units { get; set; }
        public required int Hp { get; init; }
        public required int AttackDamage { get; init; }
        public required int Initiative { get; init; }
        public required string AttackType { get; init; }
        public required string[] Weakness { get; init; }
        public required string[] Immunity { get; init; }
        public int Boost = 0;
    }
}