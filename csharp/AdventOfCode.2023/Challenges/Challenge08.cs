using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2023.Challenges;

[Challenge(8)]
public class Challenge08
{
    private readonly IInputReader _inputReader;

    public Challenge08(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await ParseInputAsync(8);
        return CalculateSteps("AAA", input, n => n == "ZZZ").ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await ParseInputAsync(8);
        return input.Nodes.Keys
            .Where(n => n.EndsWith("A"))
            .Aggregate(1L,
                (total, next) => Euclid.LeastCommonMultiple(total, CalculateSteps(next, input, n => n.EndsWith("Z"))))
            .ToString();
    }

    private static int CalculateSteps(string currentNode, Input input, Func<string, bool> isFinished)
    {
        var index = 0;
        var steps = 0;
        while (!isFinished(currentNode))
        {
            currentNode = input.Instructions[index] switch
            {
                'L' => input.Nodes[currentNode][0],
                'R' => input.Nodes[currentNode][1],
                _ => throw new ArgumentOutOfRangeException()
            };

            steps++;
            index = (index + 1) % input.Instructions.Length;
        }

        return steps;
    }

    private async Task<Input> ParseInputAsync(int day)
    {
        var nl = Environment.NewLine;
        var text = await _inputReader.ReadAllTextAsync(day);
        return text
            .SplitBy($"{nl}{nl}")
            .Into(parts =>
            {
                return new Input(parts.First(),
                    parts.Second()
                        .SplitBy(nl)
                        .Select(line => Regex.Match(line, @"^(\w{3}) = \((\w{3}), (\w{3})\)$"))
                        .ToDictionary(kv => kv.Groups[1].Value, kv => new[] {kv.Groups[2].Value, kv.Groups[3].Value}));
            });
    }

    private record Input(string Instructions, Dictionary<string, string[]> Nodes);
}