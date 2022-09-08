using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(7)]
public class Challenge07
{
    private readonly IInputReader _inputReader;

    public Challenge07(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var instructionSet = await CreateInstructionSetAsync();
        var buffer = new Dictionary<string, int>();
        return Resolve(instructionSet, buffer, "a").ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var instructionSet = await CreateInstructionSetAsync();
        var buffer = new Dictionary<string, int>();
        var a = Resolve(instructionSet, buffer, "a");
        buffer.Clear();
        buffer.Add("b", a);
        return Resolve(instructionSet, buffer, "a").ToString();
    }

    private async Task<IDictionary<string, Instruction>> CreateInstructionSetAsync()
    {
        var instructionSet = new Dictionary<string, Instruction>();
        await foreach (var (key, instruction) in _inputReader.ParseLinesAsync(7, ParseInstruction))
        {
            instructionSet.Add(key, instruction);
        }

        return instructionSet;
    }

    private int Input(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wire) =>
        Resolve(instructionSet, buffer, wire);

    private int And(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wireA,
        string wireB)
    {
        var resA = Resolve(instructionSet, buffer, wireA);
        var resB = Resolve(instructionSet, buffer, wireB);
        return resA & resB;
    }

    private int Or(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wireA,
        string wireB)
    {
        var resA = Resolve(instructionSet, buffer, wireA);
        var resB = Resolve(instructionSet, buffer, wireB);
        return resA | resB;
    }

    private int Not(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wire) =>
        ~Resolve(instructionSet, buffer, wire)  & 0xffff;
    
    private int RShift(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wireA,
    string wireB) {
        var resA = Resolve(instructionSet, buffer, wireA);
        var resB = Resolve(instructionSet, buffer, wireB);
        return resA >> resB;
    }

    private int LShift(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer, string wireA,
        string wireB)
    {
        var resA = Resolve(instructionSet, buffer, wireA);
        var resB = Resolve(instructionSet, buffer, wireB);
        return resA << resB;
    }

    private (string, Instruction) ParseInstruction(string line)
    {
        var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (split.Length == 3) // Input
            return (split[2], new Instruction((a, b) => Input(a, b, split[0])));

        if (split.Length == 4) // NOT
            return (split[3], new Instruction((a, b) => Not(a, b, split[1])));

        return split[1] switch
        {
            "AND" => (split[4], new Instruction((a, b) => And(a, b, split[0], split[2]))),
            "OR" => (split[4], new Instruction((a, b) => Or(a, b, split[0], split[2]))),
            "LSHIFT" => (split[4], new Instruction((a, b) => LShift(a, b, split[0], split[2]))),
            "RSHIFT" => (split[4], new Instruction((a, b) => RShift(a, b, split[0], split[2]))),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int Resolve(IDictionary<string, Instruction> instructionSet, IDictionary<string, int> buffer,
        string wire)
    {
        if (int.TryParse(wire, out var value) || buffer.TryGetValue(wire, out value))
            return value;

        var instruction = instructionSet[wire];
        buffer.AddOrUpdate(wire, instruction.Execute(instructionSet, buffer));
        return buffer[wire];
    }

    private record Instruction(Func<IDictionary<string, Instruction>, IDictionary<string, int>, int> Execute);
}