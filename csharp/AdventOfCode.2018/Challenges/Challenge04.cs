using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(4)]
public class Challenge04(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var logs = ParseLogs(await inputReader.ParseLinesAsync(4, PreParse).OrderBy(log => log.Time).ToListAsync());
        var guard = logs.OrderByDescending(x => x.Value.Sum(a => a.TotalMinutes)).First();
        var minute = ExtractAsleepCountPerMinute(guard.Value).Select((v, i) => new {Minute = i, Value = v})
            .OrderByDescending(x => x.Value).First().Minute;

        return (guard.Key * minute).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var logs = ParseLogs(await inputReader.ParseLinesAsync(4, PreParse).OrderBy(log => log.Time).ToListAsync());
        var res = logs.Select(kv =>
            {
                var asleep = ExtractAsleepCountPerMinute(kv.Value).Select((v, i) => new {Minute = i, Value = v})
                    .OrderByDescending(x => x.Value).First();
                return (Id: kv.Key, asleep.Minute, asleep.Value);
            })
            .OrderByDescending(x => x.Value)
            .First();

        return (res.Id * res.Minute).ToString();
    }

    private static int[] ExtractAsleepCountPerMinute(IEnumerable<Asleep> list) => Enumerable
        .Range(0, 60)
        .Select(minute => list.Count(asleep => asleep.From.Minute <= minute && asleep.To.Minute > minute))
        .ToArray();

    private static (DateTime Time, string Action) PreParse(string line) =>
        line.Extract<DateTime, string>(@"\[(.+)] (.+)");

    private static IDictionary<int, List<Asleep>> ParseLogs(IEnumerable<(DateTime Time, string Action)> logs)
    {
        var pattern = @"Guard #(\d+) begins shift";

        var dict = new Dictionary<int, List<Asleep>>();
        foreach (var chunk in logs.ChunkBy(log => Regex.IsMatch(log.Action, pattern)).Select(c => c.ToArray()))
        {
            var id = chunk.First().Action.Extract<int>(pattern)[0];
            var asleep = chunk.Skip(1).Chunk(2).Select(parts => new Asleep(parts.First().Time, parts.Second().Time))
                .ToList();
            if (!dict.TryAdd(id, asleep))
                dict[id].AddRange(asleep);
        }

        return dict;
    }

    private record Asleep(DateTime From, DateTime To)
    {
        public readonly double TotalMinutes = (To - From).TotalMinutes;
    }
}