using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.Math;
// ReSharper disable UsageOfDefaultStructEquality

namespace AdventOfCode2022.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    private static readonly Regex Pattern = new(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ?(.+)");

    [Part1]
    public async Task<string> Part1Async()
    {
        var valves = await inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);

        var edges = valves.SelectMany(kv => kv.Value.LeadsTo.SelectMany(t => new[] {(kv.Key, t), (t, kv.Key)}))
            .ToList();
        var vertices = valves.Keys.ToList();

        var travelTimeLookup = FloydWarshall.Calculate(vertices, edges, (_, _) => 1);
        var unopenedWithFlowRate =
            valves.Where(kv => kv.Value.FlowRate > 0 && kv.Key != "AA").Select(kv => kv.Key).ToHashSet();

        var max = 0;
        Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 30, 0, (m, _) => max = Math.Max(max, m));

        return max.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var valves = await inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);

        var edges = valves.SelectMany(kv => kv.Value.LeadsTo.SelectMany(t => new[] {(kv.Key, t), (t, kv.Key)}))
            .ToList();
        var vertices = valves.Keys.ToList();

        var travelTimeLookup = FloydWarshall.Calculate(vertices, edges, (_, _) => 1);
        var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0 && kv.Key != "AA")
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        // Generate combinations for half of the unopened valves
        var splitCount = (int) Math.Ceiling(unopenedWithFlowRate.Count / 2d);
        var combinations = Combinatorics.SelectAllCombinations(unopenedWithFlowRate, splitCount)
            .Select(x => x.ToDictionary(kv => kv.Key, kv => kv.Value)).ToList();

        var combinedMax = 0;
        foreach (var combination in combinations)
        {
            var remainingValves = unopenedWithFlowRate.Except(combination).ToDictionary(kv => kv.Key, kv => kv.Value);

            var max1 = 0;
            Search(combination, travelTimeLookup, combination.Select(kv => kv.Key).ToHashSet(), "AA", 26, 0,
                (m, _) => max1 = Math.Max(max1, m));
            var max2 = 0;
            Search(remainingValves, travelTimeLookup, remainingValves.Select(kv => kv.Key).ToHashSet(), "AA", 26, 0,
                (m, _) => max2 = Math.Max(max2, m));
            combinedMax = Math.Max(combinedMax, max1 + max2);
        }

        return combinedMax.ToString();
    }

    private void Search(IDictionary<string, Valve> valves, IDictionary<(string, string), int> travelTimeLookup,
        ISet<string> unvisited, string current, int timeRemaining, int pressure,
        Action<int, ISet<string>> reportPressure)
    {
        reportPressure(pressure, valves.Keys.Except(unvisited).ToHashSet());

        foreach (var other in unvisited)
        {
            var travelAndOpenTime = travelTimeLookup[(current, other)] + 1;
            var timeAtNext = timeRemaining - travelAndOpenTime;

            if (travelAndOpenTime >= timeRemaining)
                continue;

            Search(valves, travelTimeLookup, unvisited.Except(new[] {other}).ToHashSet(), other, timeAtNext,
                pressure + timeAtNext * valves[other].FlowRate, reportPressure);
        }
    }

    private static Valve ParseLine(string line)
    {
        var match = Pattern.Match(line);

        var name = match.Groups[1].Value;
        var flowRate = int.Parse(match.Groups[2].Value);
        var leadsTo = match.Groups[3].Value
            .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new Valve(name, flowRate, leadsTo);
    }

    private record Valve(string Name, int FlowRate, string[] LeadsTo);
}