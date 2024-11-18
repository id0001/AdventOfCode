using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (state, groups) = ParseInput(await inputReader.ReadLinesAsync(12).ToListAsync());
        for (var i = 0; i < 20; i++)
        {
            var newState = new PointCloud<Point1, int>();
            for (var x = state.Bounds.GetMin(0) - 2; x <= state.Bounds.GetMax(0) + 2; x++)
            {
                var key = GetKey(state, x);
                if (groups.ContainsKey(key) && groups[key] == '#')
                    newState.Set(x);
            }

            state = newState;
        }

        var (min, max) = (state.Bounds.GetMin(0), state.Bounds.GetMax(0));
        return Enumerable.Range(min, max - min + 1).Where(i => state.Contains(i)).Sum().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (state, groups) = ParseInput(await inputReader.ReadLinesAsync(12).ToListAsync());
        for (long i = 0;; i++)
        {
            var newState = new PointCloud<Point1, int>();
            for (var x = state.Bounds.GetMin(0) - 2; x <= state.Bounds.GetMax(0) + 2; x++)
            {
                var key = GetKey(state, x);
                if (groups.ContainsKey(key) && groups[key] == '#')
                    newState.Set(x);
            }

            if (StateToString(state) == StateToString(newState)) // Found cycle
            {
                var add = 50_000_000_000 - i;
                var (min, max) = (state.Bounds.GetMin(0), state.Bounds.GetMax(0));
                return Enumerable.Range(min, max - min + 1).Where(ix => state.Contains(ix)).Select(ix => ix + add).Sum()
                    .ToString();
            }

            state = newState;
        }
    }

    private static string GetKey(PointCloud<Point1, int> cloud, int index)
    {
        var sb = new StringBuilder();
        for (var i = index - 2; i <= index + 2; i++)
            sb.Append(cloud.Contains(new Point1(i)) ? '#' : '.');

        return sb.ToString();
    }

    private static string StateToString(PointCloud<Point1, int> cloud)
    {
        var sb = new StringBuilder();
        for (var i = cloud.Bounds.GetMin(0) - 2; i <= cloud.Bounds.GetMax(0) + 2; i++)
            sb.Append(cloud.Contains(new Point1(i)) ? '#' : '.');

        return sb.ToString();
    }

    private static (PointCloud<Point1, int>, IDictionary<string, char>) ParseInput(List<string> input)
    {
        var initialState = ParseInitialState(input[0]);
        var groups = ParseConversionGroups(input.Skip(2));
        return (new PointCloud<Point1, int>(initialState), groups);
    }

    private static IEnumerable<Point1> ParseInitialState(string input)
    {
        var state = input.Extract(@"initial state: (.+)").First();
        for (var i = 0; i < state.Length; i++)
            if (state[i] == '#')
                yield return i;
    }

    private static IDictionary<string, char> ParseConversionGroups(IEnumerable<string> lines)
        => lines.Select(line => line.SplitBy<string, char>("=>")).ToDictionary(kv => kv.First, kv => kv.Second);
}