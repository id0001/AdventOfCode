using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2022.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader InputReader)
{
    private static readonly Regex Pattern = new(@"move (\d+) from (\d) to (\d)", RegexOptions.Compiled);

    private readonly Stack<char>[] _stacks =
    {
        new(new[] {'J', 'H', 'P', 'M', 'S', 'F', 'N', 'V'}),
        new(new[] {'S', 'R', 'L', 'M', 'J', 'D', 'Q'}),
        new(new[] {'N', 'Q', 'D', 'H', 'C', 'S', 'W', 'B'}),
        new(new[] {'R', 'S', 'C', 'L'}),
        new(new[] {'M', 'V', 'T', 'P', 'F', 'B'}),
        new(new[] {'T', 'R', 'Q', 'N', 'C'}),
        new(new[] {'G', 'V', 'R'}),
        new(new[] {'C', 'Z', 'S', 'P', 'D', 'L', 'R'}),
        new(new[] {'D', 'S', 'J', 'V', 'G', 'P', 'B', 'F'})
    };

    [Part1]
    public async Task<string> Part1Async()
    {
        await foreach (var (x, from, to) in InputReader.ParseLinesAsync(5, ParseLine))
        {
            if (x == 0)
                continue;

            for (var i = 0; i < x; i++)
                _stacks[to].Push(_stacks[from].Pop());
        }

        return string.Join("", _stacks.Select(x => x.Peek()));
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        await foreach (var (x, from, to) in InputReader.ParseLinesAsync(5, ParseLine))
        {
            if (x == 0)
                continue;

            var temp = new Stack<char>();
            for (var i = 0; i < x; i++)
                temp.Push(_stacks[from].Pop());

            while (temp.Count > 0)
                _stacks[to].Push(temp.Pop());
        }

        return string.Join("", _stacks.Select(x => x.Peek()));
    }

    private static (int, int, int) ParseLine(string line)
    {
        if (!line.StartsWith("move"))
            return (0, 0, 0);

        var match = Pattern.Match(line);
        return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value) - 1,
            int.Parse(match.Groups[3].Value) - 1);
    }
}