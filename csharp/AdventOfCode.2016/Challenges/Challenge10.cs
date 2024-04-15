using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    private static readonly Regex InputPattern = new(@"value (\d+) goes to bot (\d+)");

    private static readonly Regex ComparePattern =
        new(@"bot (\d+) gives low to (output|bot) (\d+) and high to (output|bot) (\d+)");

    [Part1]
    public async Task<string> Part1Async()
    {
        var nodes = await inputReader.ParseLinesAsync(10, ParseLine).ToListAsync();

        var compareNodes = nodes.OfType<CompareNode>().ToDictionary(kv => kv.Bot);
        var botMemory = compareNodes.ToDictionary(kv => kv.Key, _ => new List<int>());

        foreach (var input in nodes.OfType<InputNode>())
            botMemory[input.Bot].Add(input.Value);

        while (true)
            foreach (var toProcess in botMemory.Where(kv => kv.Value.Count == 2))
            {
                var cb = compareNodes[toProcess.Key];
                var values = toProcess.Value.Order().ToArray();
                toProcess.Value.Clear();

                if (values[0] == 17 && values[1] == 61)
                    return cb.Bot.ToString(); // Done

                if (cb.LowType == "bot")
                    botMemory[cb.Low].Add(values[0]);

                if (cb.HighType == "bot")
                    botMemory[cb.High].Add(values[1]);
            }
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var nodes = await inputReader.ParseLinesAsync(10, ParseLine).ToListAsync();

        var compareNodes = nodes.OfType<CompareNode>().ToDictionary(kv => kv.Bot);
        var botMemory = compareNodes.ToDictionary(kv => kv.Key, _ => new List<int>());
        var outputs = new int[compareNodes.Values
            .SelectMany(n => new[] {(Type: n.LowType, Id: n.Low), (Type: n.HighType, Id: n.High)})
            .Where(n => n.Type == "output")
            .Max(n => n.Id) + 1];

        foreach (var input in nodes.OfType<InputNode>())
            botMemory[input.Bot].Add(input.Value);

        bool nothingProcessed;
        do
        {
            nothingProcessed = true;
            foreach (var toProcess in botMemory.Where(kv => kv.Value.Count == 2))
            {
                nothingProcessed = false;
                var cb = compareNodes[toProcess.Key];
                var values = toProcess.Value.Order().ToArray();
                toProcess.Value.Clear();

                if (cb.LowType == "bot")
                    botMemory[cb.Low].Add(values[0]);
                else
                    outputs[cb.Low] = values[0];

                if (cb.HighType == "bot")
                    botMemory[cb.High].Add(values[1]);
                else
                    outputs[cb.High] = values[1];
            }
        } while (!nothingProcessed);

        return (outputs[0] * outputs[1] * outputs[2]).ToString();
    }

    private static Node ParseLine(string line) => (InputPattern.Match(line), ComparePattern.Match(line)) switch
    {
        var (i, _) when i.Success => new InputNode(i.Groups[2].Value.As<int>(), i.Groups[1].Value.As<int>()),
        var (_, c) when c.Success => new CompareNode(c.Groups[1].Value.As<int>(), c.Groups[2].Value,
            c.Groups[3].Value.As<int>(), c.Groups[4].Value, c.Groups[5].Value.As<int>()),
        _ => throw new NotImplementedException()
    };

    private abstract record Node(int Bot);

    private record CompareNode(int Bot, string LowType, int Low, string HighType, int High) : Node(Bot);

    private record InputNode(int Bot, int Value) : Node(Bot);
}