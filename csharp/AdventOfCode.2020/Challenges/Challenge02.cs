using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(2)]
public class Challenge02
{
    private readonly IInputReader _inputReader;
    private InputLine[]? _input;

    public Challenge02(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        _input = await _inputReader.ReadLinesAsync(2).Select(line =>
        {
            var m = Regex.Match(line, @"^(\d+)-(\d+) (\w): (\w+)$");
            if (!m.Success)
                throw new InvalidOperationException("Line did not match pattern: " + line);

            var min = int.Parse(m.Groups[1].Value);
            var max = int.Parse(m.Groups[2].Value);
            var c = char.Parse(m.Groups[3].Value);
            var pass = m.Groups[4].Value;

            return new InputLine(min, max, c, pass);
        }).ToArrayAsync();
    }

    [Part1]
    public string Part1()
    {
        var result = (from line in _input
            let count = Regex.Matches(line.Password, line.Character.ToString()).Count
            where count >= line.Min && count <= line.Max
            select line.Password).ToArray();

        return result.Length.ToString();
    }

    [Part2]
    public string Part2()
    {
        var result = (from line in _input
            where (line.Password[line.Min - 1] == line.Character) ^ (line.Password[line.Max - 1] == line.Character)
            select line.Password).ToArray();

        return result.Length.ToString();
    }

    private record InputLine(int Min, int Max, char Character, string Password);
}