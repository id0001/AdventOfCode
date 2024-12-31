using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(2)]
public class Challenge02(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
        => (await inputReader
                .ParseLinesAsync(2, ParseLine)
                .Where(x => IsSafe(x))
                .CountAsync())
            .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader
            .ParseLinesAsync(2, ParseLine)
            .Where(x => IsSafe(x, true))
            .CountAsync())
        .ToString();

    private static bool IsSafe(int[] reports, bool tolerance = false)
    {
        var isUp = reports[1] > reports[0];
        for (var i = 1; i < reports.Length; i++)
        {
            var diff = Math.Abs(reports[i] - reports[i - 1]);
            if (diff is < 1 or > 3)
                return tolerance && IsAnySafe(reports);

            if ((isUp && reports[i] < reports[i - 1]) || (!isUp && reports[i] > reports[i - 1]))
                return tolerance && IsAnySafe(reports);
        }

        return true;
    }

    private static bool IsAnySafe(int[] reports)
        => reports.Select((_, i) => RemoveAt(i, reports)).Any(x => IsSafe(x));

    private static int[] RemoveAt(int idx, int[] reports) => reports.Where((_, i) => i != idx).ToArray();

    private static int[] ParseLine(string line) => line.SplitBy(" ").As<int>().ToArray();
}