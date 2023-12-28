using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(21)]
public class Challenge21(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await inputReader.ReadLinesAsync(21)
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToDictionaryAsync(kv => kv[0], kv => kv[1]);

        return BuildExpressionTree(new Dictionary<string, Func<long>>(), lines, "root")().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await inputReader.ReadLinesAsync(21)
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToDictionaryAsync(kv => kv[0], kv => kv[1]);

        lines.Remove("root"); // Don't need root

        var cache = new Dictionary<string, Func<long>>();
        var calcFflg = BuildExpressionTree(cache, lines, "fflg");
        var calcQwqj = BuildExpressionTree(cache, lines, "qwqj");

        var v = SpecialFunctions.BinarySearch(0, 10_000_000_000_000L, m =>
        {
            cache["humn"] = () => m;
            return calcQwqj() - calcFflg();
        });

        return v!.Value.ToString();
    }

    private static Func<long> BuildExpressionTree(IDictionary<string, Func<long>> cache,
        IDictionary<string, string> lines, string key)
    {
        if (!cache.ContainsKey(key))
        {
            var line = lines[key];
            if (long.TryParse(line, out var num))
            {
                cache.Add(key, () => num);
            }
            else
            {
                var split = line.Split(' ');
                cache.Add(key, split[1] switch
                {
                    "+" => () =>
                        BuildExpressionTree(cache, lines, split[0])() + BuildExpressionTree(cache, lines, split[2])(),
                    "-" => () =>
                        BuildExpressionTree(cache, lines, split[0])() - BuildExpressionTree(cache, lines, split[2])(),
                    "*" => () =>
                        BuildExpressionTree(cache, lines, split[0])() * BuildExpressionTree(cache, lines, split[2])(),
                    "/" => () =>
                        BuildExpressionTree(cache, lines, split[0])() / BuildExpressionTree(cache, lines, split[2])(),
                    _ => throw new NotImplementedException()
                });
            }
        }

        return cache[key];
    }
}