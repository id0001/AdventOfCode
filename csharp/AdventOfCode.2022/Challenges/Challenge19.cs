using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var blueprints = await InputReader.ParseLinesAsync(19, ParseLine).ToListAsync();
        var result = await Task.WhenAll(blueprints.Select(async bp => await FindHighestAmountOfGeodes(bp, 24) * bp.Id));
        return result.Sum().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var blueprints = await InputReader.ParseLinesAsync(19, ParseLine).ToListAsync();
        var result = await Task.WhenAll(blueprints.Take(3).Select(async bp => await FindHighestAmountOfGeodes(bp, 32)));
        return result.Product().ToString();
    }

    private Task<int> FindHighestAmountOfGeodes(Blueprint blueprint, int minutes)
    {
        var state = new State(minutes, 0, 0, 0, 0, 0, 0, 0, 1);

        var highestAmount = 0;
        var cache = new HashSet<State>();

        var stack = new Stack<State>(new[] {state});
        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current.TimeLeft == 0)
            {
                if (current.Geodes > highestAmount) highestAmount = current.Geodes;

                continue;
            }

            foreach (var neighbor in GetNeighbors(blueprint, current))
            {
                if (neighbor.Geodes + neighbor.GeodeBots * neighbor.TimeLeft +
                    Euclid.TriangularNumber(neighbor.TimeLeft) < highestAmount)
                    continue;

                if (!cache.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    cache.Add(neighbor);
                }
            }
        }

        return Task.FromResult(highestAmount);
    }

    private IEnumerable<State> GetNeighbors(Blueprint bp, State state)
    {
        var highestOreCost = Math.Max(bp.OreCost, Math.Max(bp.ClayCost, Math.Max(bp.ObsidianOreCost, bp.GeodeOreCost)));

        if (state.Obsidian >= bp.GeodeObsidianCost && state.Ore >= bp.GeodeOreCost)
        {
            yield return new State(state.TimeLeft - 1, state.Geodes + state.GeodeBots,
                state.Obsidian + state.ObsidianBots - bp.GeodeObsidianCost, state.Clay + state.ClayBots,
                state.Ore + state.OreBots - bp.GeodeOreCost, state.GeodeBots + 1, state.ObsidianBots, state.ClayBots,
                state.OreBots);
        }
        else
        {
            if (state.Ore >= bp.ObsidianOreCost && state.Clay >= bp.ObsidianClayCost &&
                state.ObsidianBots < bp.GeodeObsidianCost)
                yield return new State(state.TimeLeft - 1, state.Geodes + state.GeodeBots,
                    state.Obsidian + state.ObsidianBots, state.Clay + state.ClayBots - bp.ObsidianClayCost,
                    state.Ore + state.OreBots - bp.ObsidianOreCost, state.GeodeBots, state.ObsidianBots + 1,
                    state.ClayBots, state.OreBots);
            else if (state.Ore >= bp.ClayCost && state.ClayBots < bp.ObsidianClayCost)
                yield return new State(state.TimeLeft - 1, state.Geodes + state.GeodeBots,
                    state.Obsidian + state.ObsidianBots, state.Clay + state.ClayBots,
                    state.Ore + state.OreBots - bp.ClayCost, state.GeodeBots, state.ObsidianBots, state.ClayBots + 1,
                    state.OreBots);

            if (state.Ore >= bp.OreCost && state.OreBots < highestOreCost)
                yield return new State(state.TimeLeft - 1, state.Geodes + state.GeodeBots,
                    state.Obsidian + state.ObsidianBots, state.Clay + state.ClayBots,
                    state.Ore + state.OreBots - bp.OreCost, state.GeodeBots, state.ObsidianBots, state.ClayBots,
                    state.OreBots + 1);

            yield return new State(state.TimeLeft - 1, state.Geodes + state.GeodeBots,
                state.Obsidian + state.ObsidianBots, state.Clay + state.ClayBots, state.Ore + state.OreBots,
                state.GeodeBots, state.ObsidianBots, state.ClayBots, state.OreBots);
        }
    }

    private static Blueprint ParseLine(string line)
    {
        var match = Regex.Match(line,
            @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");
        return new Blueprint(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value),
            int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value),
            int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value));
    }

    private record Blueprint(
        int Id,
        int OreCost,
        int ClayCost,
        int ObsidianOreCost,
        int ObsidianClayCost,
        int GeodeOreCost,
        int GeodeObsidianCost);

    private record State(
        int TimeLeft,
        int Geodes,
        int Obsidian,
        int Clay,
        int Ore,
        int GeodeBots,
        int ObsidianBots,
        int ClayBots,
        int OreBots);
}