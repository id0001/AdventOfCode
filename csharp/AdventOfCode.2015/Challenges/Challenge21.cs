using AdventOfCode.Core;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(21)]
public class Challenge21
{
    private static readonly Item[] Weapons =
    [
        new Item(8, 4, 0),
        new Item(10, 5, 0),
        new Item(25, 6, 0),
        new Item(40, 7, 0),
        new Item(74, 8, 0)
    ];

    private static readonly Item[] Armor =
    [
        new Item(0, 0, 0),
        new Item(13, 0, 1),
        new Item(31, 0, 2),
        new Item(53, 0, 3),
        new Item(75, 0, 4),
        new Item(102, 0, 5)
    ];

    private static readonly List<Item> Rings =
    [
        new Item(0, 0, 0),
        new Item(25, 1, 0),
        new Item(50, 2, 0),
        new Item(100, 3, 0),
        new Item(20, 0, 1),
        new Item(40, 0, 2),
        new Item(80, 0, 3)
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