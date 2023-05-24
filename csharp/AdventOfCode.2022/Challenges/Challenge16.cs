using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.PathFinding;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0).Select(kv => kv.Key).ToHashSet();

            int max = 0;
            Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 30, 0, (m, _) => max = Math.Max(max, m));

            return max.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var valves = await _inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);

            var edges = valves.SelectMany(kv => kv.Value.LeadsTo.SelectMany(t => new[] { (kv.Key, t), (t, kv.Key) })).ToList();
            var vertices = valves.Keys.ToList();

            var travelTimeLookup = FloydWarshall.Calculate(vertices, edges, (_, _) => 1);
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0).Select(kv => kv.Key).ToHashSet();

            var map = new Dictionary<ISet<string>, int>();
            Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 26, 0, (p, s) =>
            {
                if (map.ContainsKey(s) && p > map[s])
                    map[s] = p;
                else if (!map.ContainsKey(s))
                    map.Add(s, p);
            });

            int max = 0;
            var keys = map.Keys.ToArray();
            for (int y = 0; y < keys.Length; y++)
                for (int x = y + 1; x < keys.Length; x++)
                    if (keys[x].Intersect(keys[y]).Count() == 0)
                        max = Math.Max(max, map[keys[x]] + map[keys[y]]);

            return max.ToString();
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
