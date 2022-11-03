using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(14)]
public class Challenge14
{
    private readonly IInputReader _inputReader;
    private readonly Dictionary<string, char> _insertionMap = new();
    private readonly Dictionary<char, int> _indexes = new();
    private string _template = string.Empty;

    public Challenge14(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await _inputReader.ReadLinesAsync(14).ToArrayAsync();

        _template = lines[0];

        for (var i = 2; i < lines.Length; i++)
        {
            var split = lines[i].Split(" -> ");
            _insertionMap.Add(split[0], Convert.ToChar(split[1]));
        }

        var uniqueChars = _insertionMap.Values.Distinct().ToArray();
        for (var i = 0; i < uniqueChars.Length; i++) _indexes.Add(uniqueChars[i], i);
    }

    [Part1]
    public string Part1()
    {
        return CalculateResult(10).ToString();
    }

    [Part2]
    public string Part2()
    {
        return CalculateResult(40).ToString();
    }

    private long CalculateResult(int steps)
    {
        var counts = new long[_indexes.Count];
        var cache = new Dictionary<StateKey, long[]>();
        for (var i = 0; i < _template.Length - 1; i++)
        {
            counts[GetCharIndex(_template[i])]++;
            counts = counts.Zip(CountInserted(_template.Substring(i, 2), 0, steps - 1, cache), (a, b) => a + b)
                .ToArray();
        }

        counts[GetCharIndex(_template[^1])]++;
        return counts.Max() - counts.Min();
    }

    private long[] CountInserted(string pair, int depth, int maxDepth, Dictionary<StateKey, long[]> cache)
    {
        if (cache.TryGetValue(new StateKey(pair, depth), out var counts)) return counts;

        var inserted = _insertionMap[pair];
        counts = new long[_indexes.Count];

        if (depth == maxDepth)
        {
            counts[GetCharIndex(inserted)]++;
            return counts;
        }

        var pair1 = new string(new[] { pair[0], inserted });
        var pair2 = new string(new[] { inserted, pair[1] });
        var c0 = CountInserted(pair1, depth + 1, maxDepth, cache);
        var c1 = CountInserted(pair2, depth + 1, maxDepth, cache);

        counts = c0.Zip(c1, (a, b) => a + b).ToArray();
        counts[GetCharIndex(inserted)]++;

        var key = new StateKey(pair, depth);
        if (!cache.TryAdd(key, counts))
            cache[key] = counts;

        return counts;
    }

    private int GetCharIndex(char c) => _indexes[c];

    // ReSharper disable NotAccessedPositionalProperty.Local
    private record StateKey(string Pair, int Depth);
}