using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

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
    public async Task<string> Part1Async()
    {
        var pattern = new Regex(@"(\d)", RegexOptions.Compiled);
        var sum = 0;

        await foreach (var line in _inputReader.ReadLinesAsync(1))
        {
            var matches = pattern.Matches(line);
            sum += int.Parse($"{matches[0].Groups[1].Value}{matches[^1].Groups[1].Value}");
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var pattern = new Regex(@"(?=(one|two|three|four|five|six|seven|eight|nine|\d))", RegexOptions.Compiled);
        var sum = 0;

        await foreach (var line in _inputReader.ReadLinesAsync(1))
        {
            var matches = pattern.Matches(line);
            sum += int.Parse($"{ToDigit(matches[0].Groups[1].Value)}{ToDigit(matches[^1].Groups[1].Value)}");
        }

        return sum.ToString();
    }

    private char ToDigit(string s) => char.IsDigit(s[0]) ? s[0] : (char)('0' | Array.IndexOf(Numbers, s));
}