using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2018.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (terrain, units) = Parse(await inputReader.ReadGridAsync(15));

        Run(terrain, units, 3, out long score);
        return score.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (terrain, baseUnits) = Parse(await inputReader.ReadGridAsync(15));

        for (var ap = 4; ap < 200; ap++)
        {
            // Make a copy of baseUnits
            if (Run(terrain, baseUnits.ToDictionary(), ap, out var score))
                return score.ToString();
        }

        return string.Empty;
    }

    private static bool Run(char[,] terrain, IDictionary<Point2, Unit> units, long ap, out long score)
    {
        // set correct AP
        foreach (var key in units.Keys)
        {
            if (units[key] is Elf elf)
                units[key] = elf with { Ap = ap };
        }

        var elfCount = units.Count(u => u.Value.GetType() == typeof(Elf));
        score = 0;
        int round = 0;
        while (true)
        {
            foreach (var key in units.Keys.OrderBy(k => k.Y).ThenBy(k => k.X).ToArray())
            {
                if (!units.ContainsKey(key))
                    continue;

                if (!units.Values.OfType<Elf>().Any() || !units.Values.OfType<Goblin>().Any())
                {
                    score = units.Values.Sum(x => x.Hp) * round;
                    return units.All(x => x.Value.GetType() == typeof(Elf)) && elfCount == units.Values.Count;
                }

                switch (units[key])
                {
                    case Elf:
                        ExecuteTurn(terrain, units, key, GetPositions<Goblin>(units));
                        break;
                    case Goblin:
                        ExecuteTurn(terrain, units, key, GetPositions<Elf>(units));
                        break;
                    default:
                        break;
                }
            }

            round++;
        }
    }

    private static void ExecuteTurn(char[,] terrain, IDictionary<Point2, Unit> units, Point2 currentPosition, ISet<Point2> targets)
    {
        // Move or stay
        var nextPosition = NextPosition(terrain, units, targets, currentPosition);
        if (currentPosition != nextPosition)
        {
            var unit = units[currentPosition];
            units.Remove(currentPosition);
            units.Add(nextPosition, unit);
            currentPosition = nextPosition;
        }

        // Attack
        var targetsInRange = GetTargetsInRange(targets, currentPosition).ToList();
        if (targetsInRange.Count == 0)
            return;

        var target = targetsInRange.MinBy(t => units[t].Hp);
        units[target] = units[target] with { Hp = units[target].Hp - units[currentPosition].Ap };
        if (units[target].Hp <= 0)
            units.Remove(target);
    }

    private static IEnumerable<Point2> GetTargetsInRange(ISet<Point2> targets, Point2 current)
        => current.GetNeighbors().Where(targets.Contains);

    private static Point2 NextPosition(char[,] terrain, IDictionary<Point2, Unit> units, ISet<Point2> targets, Point2 currentPosition)
    {
        if (GetTargetsInRange(targets, currentPosition).Any())
            return currentPosition;

        var occupied = units.Keys.ToHashSet();
        var spacesToCheck = targets.SelectMany(t => GetOpenSpacesAroundTarget(terrain, occupied, t));

        var bfs = new BreadthFirstSearch<Point2>(c => GetAdjacent(terrain, occupied, c));

        var reachable = bfs
            .CalculateDistances(currentPosition)
            .IntersectBy(spacesToCheck, s => s.Key)
            .OrderBy(k => k.Value)
            .ThenBy(kv => kv.Key.Y)
            .ThenBy(kv => kv.Key.X)
            .Select(kv => (Point2?)kv.Key)
            .FirstOrDefault();

        if (!reachable.HasValue)
            return currentPosition;

        bfs.TryPath(currentPosition, p => p == reachable, out var path);
        return path.Second();
    }

    private static ISet<Point2> GetPositions<T>(IDictionary<Point2, Unit> units)
        where T : Unit
        => units.Where(kv => kv.Value.GetType() == typeof(T)).Select(x => x.Key).ToHashSet();

    private static IEnumerable<Point2> GetOpenSpacesAroundTarget(char[,] terrain, ISet<Point2> occupied, Point2 target)
        => target.GetNeighbors().Where(n => !occupied.Contains(n) && terrain[n.Y, n.X] == '.').OrderBy(n => n.Y).ThenBy(n => n.X);

    private static IEnumerable<Point2> GetAdjacent(char[,] terrain, ISet<Point2> units, Point2 current)
        => GetOpenSpacesAroundTarget(terrain, units, current);

    private static (char[,] Terrain, Dictionary<Point2, Unit> Units) Parse(char[,] input)
    {
        var terrain = new char[input.GetLength(0), input.GetLength(1)];
        Dictionary<Point2, Unit> units = new();

        foreach (var (p, c) in input.AsEnumerable())
        {
            if (c == '#' || c == '.')
            {
                terrain[p.Y, p.X] = c;
                continue;
            }

            if (c == 'G')
                units.Add(p, new Goblin());

            if (c == 'E')
                units.Add(p, new Elf());

            terrain[p.Y, p.X] = '.';
        }

        return (terrain, units);
    }

    private record Unit(long Hp, long Ap);
    private record Elf() : Unit(200, 3);
    private record Goblin() : Unit(200, 3);
}