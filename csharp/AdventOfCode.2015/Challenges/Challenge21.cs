using AdventOfCode.Core;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(21)]
public class Challenge21
{
    private static readonly Item[] Weapons =
    [
        new(8, 4, 0),
        new(10, 5, 0),
        new(25, 6, 0),
        new(40, 7, 0),
        new(74, 8, 0)
    ];

    private static readonly Item[] Armor =
    [
        new(0, 0, 0),
        new(13, 0, 1),
        new(31, 0, 2),
        new(53, 0, 3),
        new(75, 0, 4),
        new(102, 0, 5)
    ];

    private static readonly List<Item> Rings =
    [
        new(0, 0, 0),
        new(25, 1, 0),
        new(50, 2, 0),
        new(100, 3, 0),
        new(20, 0, 1),
        new(40, 0, 2),
        new(80, 0, 3)
    ];

    [Part1]
    public string Part1()
    {
        var boss = new Stats(109, 8, 2);
        return Weapons
            .SelectMany(_ => Armor, (a, b) => new[] {a, b})
            .SelectMany(_ => Rings.Combinations(2), (a, b) => new
            {
                Cost = a.Sum(x => x.Cost) + b.Sum(x => x.Cost),
                PlayerWins =
                    PlayerWins(new Stats(100, a[0].Damage + b.Sum(x => x.Damage), a[1].Armor + b.Sum(x => x.Armor)),
                        boss)
            })
            .MinBy(x => x.Cost + (x.PlayerWins ? 0 : 1000))!
            .Cost
            .ToString();
    }

    [Part2]
    public string Part2()
    {
        var boss = new Stats(109, 8, 2);
        return Weapons
            .SelectMany(_ => Armor, (a, b) => new[] {a, b})
            .SelectMany(_ => Rings.Combinations(2), (a, b) => new
            {
                Cost = a.Sum(x => x.Cost) + b.Sum(x => x.Cost),
                PlayerWins =
                    PlayerWins(new Stats(100, a[0].Damage + b.Sum(x => x.Damage), a[1].Armor + b.Sum(x => x.Armor)),
                        boss)
            })
            .MaxBy(x => x.Cost * (!x.PlayerWins ? 1 : -1))!
            .Cost
            .ToString();
    }

    private bool PlayerWins(Stats player, Stats enemy) => player.TurnsToKill(enemy) < enemy.TurnsToKill(player);

    private record Item(int Cost, int Damage, int Armor);

    private record Stats(int Hp, int Damage, int Armor)
    {
        public int TurnsToKill(Stats defender) =>
            (int) Math.Ceiling(defender.Hp / (double) Math.Max(1, Damage - defender.Armor));
    }
}