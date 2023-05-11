using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.PathFinding;
using System.Net.Http.Headers;
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

            var travelTimeLookup = GetTravelTimeLookup(valves);
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0).Select(kv => kv.Key).ToHashSet();

            int max = 0;
            Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 30, 0, (m, _) => max = Math.Max(max, m));

            return max.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var valves = await _inputReader.ParseLinesAsync(16, ParseLine).ToDictionaryAsync(x => x.Name);

            var travelTimeLookup = GetTravelTimeLookup(valves);
            var unopenedWithFlowRate = valves.Where(kv => kv.Value.FlowRate > 0).Select(kv => kv.Key).ToHashSet();

            var moves = new List<(int, ISet<string>)>();

            int max = 0;
            Search(valves, travelTimeLookup, unopenedWithFlowRate, "AA", 26, 0, (m, o) => moves.Add((m, o)));

            foreach (var m1 in moves)
            {
                foreach (var m2 in moves)
                {
                    if (m1.Item1 + m2.Item1 > max && !m1.Item2.Intersect(m2.Item2).Any())
                        max = m1.Item1 + m2.Item1;
                }
            }

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

        private IDictionary<(string, string), int> GetTravelTimeLookup(Dictionary<string, Valve> valves)
        {
            var bfs = new BreadthFirstSearch<string>(n => GetAdjecent(valves, n));
            var all = valves.Values.Select(x => x.Name).ToList();
            var dict = new Dictionary<(string, string), int>();

            for (int y = 0; y < all.Count; y++)
            {
                for (int x = 0; x < all.Count; x++)
                {
                    if (x == y)
                        continue;

                    if (bfs.TryPath(all[y], n => n == all[x], out var path))
                    {
                        int len = path.Count();
                        dict.Add((all[x], all[y]), len - 1);
                    }
                }
            }

            return dict;
        }

        private static IEnumerable<string> GetAdjecent(Dictionary<string, Valve> valves, string currentValve)
        {
            foreach (var next in valves[currentValve].LeadsTo)
                yield return next;
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

        private sealed record State(int RemainingTime, string CurrentValve, HashSet<string> OpenValves, int CurrentRate, int TotalReleased)
        {
            public bool Equals(State? other)
            {
                if (ReferenceEquals(null, other)) return false;

                return RemainingTime == other.RemainingTime
                    && CurrentValve == other.CurrentValve
                    && OpenValves.SetEquals(OpenValves)
                    && other.CurrentRate == CurrentRate
                    && other.TotalReleased == TotalReleased;
            }

            public override int GetHashCode() => HashCode.Combine(RemainingTime, CurrentValve, OpenValves, CurrentRate, TotalReleased);
        }
    }
}
