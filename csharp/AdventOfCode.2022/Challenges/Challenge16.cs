using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.Math;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Challenges
{
    [Challenge(16)]
    public class Challenge16
    {
        private static Regex Pattern = new Regex(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ?(.+)");

        private readonly IInputReader _inputReader;

        public Challenge16(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var valves = await _inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);

            var edges = valves.SelectMany(kv => kv.Value.LeadsTo.SelectMany(t => new[] { (kv.Key, t), (t, kv.Key) })).ToList();
            var vertices = valves.Keys.ToList();

            var travelTimeLookup = FloydWarshall.Calculate(vertices, edges, (_, _) => 1);
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0 && kv.Key != "AA").Select(kv => kv.Key).ToHashSet();

            int max = 0;
            Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 30, 0, (m, _) => max = Math.Max(max, m));

            return max.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var valves = await _inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);
            var sortedValves = valves.Keys.OrderBy(v => v).ToArray();

            var edges = valves.SelectMany(kv => kv.Value.LeadsTo.SelectMany(t => new[] { (kv.Key, t), (t, kv.Key) })).ToList();
            var vertices = valves.Keys.ToList();

            var travelTimeLookup = FloydWarshall.Calculate(vertices, edges, (_, _) => 1);
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0 && kv.Key != "AA").ToDictionary(kv => kv.Key, kv => kv.Value);

            // Generate combinations for half of the unopened valves
            var splitCount = (int)Math.Ceiling(unopenedWithFlowRate.Count / 2d);
            var combinations = Combinatorics.SelectAllCombinations(unopenedWithFlowRate, splitCount).Select(x => x.ToDictionary(kv => kv.Key, kv => kv.Value)).ToList();

            int combinedMax = 0;
            foreach (var combination in combinations)
            {
                var remainingValves = unopenedWithFlowRate.Except(combination).ToDictionary(kv => kv.Key, kv => kv.Value);

                var max1 = 0;
                Search(combination, travelTimeLookup, combination.Select(kv => kv.Key).ToHashSet(), "AA", 26, 0, (m, _) => max1 = Math.Max(max1, m));
                var max2 = 0;
                Search(remainingValves, travelTimeLookup, remainingValves.Select(kv => kv.Key).ToHashSet(), "AA", 26, 0, (m, _) => max2 = Math.Max(max2, m));
                combinedMax = Math.Max(combinedMax, max1 + max2);
            }

            return combinedMax.ToString();
        }

        private void Search(IDictionary<string, Valve> valves, IDictionary<(string, string), int> travelTimeLookup, ISet<string> unvisited, string current, int timeRemaining, int pressure, Action<int, ISet<string>> reportPressure)
        {
            reportPressure(pressure, valves.Keys.Except(unvisited).ToHashSet());

            foreach (var other in unvisited)
            {
                var travelAndOpenTime = travelTimeLookup[(current, other)] + 1;
                int timeAtNext = timeRemaining - travelAndOpenTime;

                if (travelAndOpenTime >= timeRemaining)
                    continue;

                Search(valves, travelTimeLookup, unvisited.Except(new[] { other }).ToHashSet(), other, timeAtNext, pressure + (timeAtNext * valves[other].FlowRate), reportPressure);
            }
        }

        private static Valve ParseLine(string line)
        {
            var match = Pattern.Match(line);

            string name = match.Groups[1].Value;
            int flowRate = int.Parse(match.Groups[2].Value);
            string[] leadsTo = match.Groups[3].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return new Valve(name, flowRate, leadsTo);
        }

        private record Valve(string Name, int FlowRate, string[] LeadsTo);

        private record TravelState(int RemainingTime, string CurrentValve, IEnumerable<string> UnopenedValves, int TotalPressure);
    }
}
