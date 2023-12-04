using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(1)]
public class Challenge01
{
    private static readonly string[] Numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    private readonly IInputReader _inputReader;

    public Challenge01(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        return await _inputReader
            .ReadLinesAsync(1)
            .SumAsync(line => line
                .MatchBy(@"(\d)", m => int.Parse($"{m[0].Groups[1].Value}{m[^1].Groups[1].Value}")))
            .ToStringAsync();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var pattern = new Regex(@"(?=(one|two|three|four|five|six|seven|eight|nine|\d))", RegexOptions.Compiled);
        return await _inputReader
            .ReadLinesAsync(1)
            .SumAsync(line => line
                .MatchBy(pattern, m => int.Parse($"{ToDigit(m.First.Groups[1].Value)}{ToDigit(m[^1].Groups[1].Value)}")))
            .ToStringAsync();
    }

    private char ToDigit(string s) => char.IsDigit(s[0]) ? s[0] : (char)('0' | Array.IndexOf(Numbers, s));
}