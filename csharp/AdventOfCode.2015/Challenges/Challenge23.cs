using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(23)]
public class Challenge23(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var registers = new Dictionary<char, int>
        {
            {'a', 0},
            {'b', 0}
        };

        var program = await inputReader.ParseLinesAsync(23, ParseInstruction).ToArrayAsync();
        ExecuteProgram(registers, program);

        return registers['b'].ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var registers = new Dictionary<char, int>
        {
            {'a', 1},
            {'b', 0}
        };

        var program = await inputReader.ParseLinesAsync(23, ParseInstruction).ToArrayAsync();
        ExecuteProgram(registers, program);

        return registers['b'].ToString();
    }

    private static void ExecuteProgram(Dictionary<char, int> registers, (string, char, int)[] program)
    {
        var ip = 0;

        while (ip < program.Length)
        {
            var (c, r, v) = program[ip];

            switch (c)
            {
                case "hlf":
                    registers[r] /= 2;
                    ip++;
                    break;
                case "tpl":
                    registers[r] *= 3;
                    ip++;
                    break;
                case "inc":
                    registers[r] += 1;
                    ip++;
                    break;
                case "jmp":
                    ip += v;
                    break;
                case "jie":
                    ip += registers[r] % 2 == 0 ? v : 1;
                    break;
                case "jio":
                    ip += registers[r] == 1 ? v : 1;
                    break;
            }
        }
    }

    private (string, char, int) ParseInstruction(string line) => line
        .SplitBy(" ", ",")
        .Into(parts =>
        {
            if (parts.Length == 2)
            {
                if (parts[1].Length == 1)
                    return (parts.First(), parts.Second().As<char>(), 0);
                return (parts.First(), 'x', parts.Second().Replace("+", string.Empty).As<int>());
            }

            return (parts.First(), parts.Second().As<char>(), parts.Third().Replace("+", string.Empty).As<int>());
        });
}