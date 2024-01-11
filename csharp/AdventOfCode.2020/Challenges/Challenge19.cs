using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader InputReader)
{
    private readonly List<string> _input = new();
    private readonly Dictionary<int, string> _rules = new();


    [Setup]
    public async Task SetupAsync()
    {
        var state = 0; // 0 = rules, 1 = input

        await foreach (var line in InputReader.ReadLinesAsync(19))
        {
            if (string.IsNullOrEmpty(line))
                state++;

            if (state == 0)
            {
                var match = Regex.Match(line, @"^(\d+): (.+)$");
                _rules.Add(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
            }
            else
            {
                _input.Add(line);
            }
        }
    }

    [Part1]
    public string Part1()
    {
        var pattern = BuildPattern(_rules, 0, new Dictionary<int, string>());
        var count = _input.Count(line => Regex.IsMatch(line, pattern));

        return count.ToString();
    }

    [Part2]
    public string Part2()
    {
        _rules[8] = "42 | 42 8";
        _rules[11] = "42 31 | 42 11 31";
        var pattern = BuildPattern(_rules, 0, new Dictionary<int, string>());
        var count = _input.Count(line => Regex.IsMatch(line, pattern));

        return count.ToString();
    }

    private static string BuildPattern(IDictionary<int, string> rules, int key, IDictionary<int, string> cache)
    {
        if (cache.TryGetValue(key, out var pattern))
            return pattern;

        switch (rules[key])
        {
            case "\"a\"":
                return "a";
            case "\"b\"":
                return "b";
        }

        var orGroups = rules[key].Split("|");

        for (var o = 0; o < orGroups.Length; o++)
            switch (key)
            {
                case 8 when orGroups[o].Contains("8"):
                {
                    var p42 = BuildPattern(rules, 42, cache);
                    orGroups[o] = $"{p42}+";
                    break;
                }
                case 11 when orGroups[o].Contains("11"):
                {
                    var p42 = BuildPattern(rules, 42, cache);
                    var p31 = BuildPattern(rules, 31, cache);

                    orGroups[o] = "(?:" +
                                  string.Join("|", Enumerable.Range(1, 10).Select(i => $"{p42}{{{i}}}{p31}{{{i}}}")) +
                                  ")";
                    break;
                }
                default:
                {
                    var parts = orGroups[o].Trim().Split(" ");
                    for (var p = 0; p < parts.Length; p++)
                    {
                        var rule = int.Parse(parts[p].Trim());
                        parts[p] = BuildPattern(rules, rule, cache);
                    }

                    orGroups[o] = string.Concat(parts);
                    break;
                }
            }

        var result = key == 0 ? $"^{string.Join("|", orGroups)}$" : $"(?:{string.Join("|", orGroups)})";

        if (!cache.ContainsKey(key))
            cache.Add(key, result);

        return result;
    }
}