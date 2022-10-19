using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(8)]
public class Challenge08
{
    private readonly IInputReader _inputReader;

    public Challenge08(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var input = await _inputReader.ReadLinesAsync(8)
            .Select(line => new Instruction(line[..3], int.Parse(line[4..]))).ToListAsync();

        ISet<int> history = new HashSet<int>();

        var acc = 0;
        for (var ip = 0; ip < input.Count; ip++)
        {
            if (history.Contains(ip))
                return acc.ToString();

            history.Add(ip);

            var instruction = input[ip];

            switch (instruction.Opcode)
            {
                case "acc":
                    acc += instruction.Value;
                    break;
                case "jmp":
                    ip += instruction.Value - 1; // -1 because the for loop always increases the ip by 1
                    break;
                case "nop":
                    break;
            }
        }

        return null;
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await _inputReader.ReadLinesAsync(8)
            .Select(line => new Instruction(line[..3], int.Parse(line[4..]))).ToListAsync();

        var closedList = new HashSet<int>();

        bool hasError;
        int acc;
        do
        {
            // Reset the computer
            hasError = false;
            var history = new HashSet<int>();
            var changedInstruction = false;
            acc = 0;

            for (var ip = 0; ip < input.Count; ip++)
            {
                if (history.Contains(ip))
                {
                    hasError = true;
                    break;
                }

                history.Add(ip);
                var instruction = input[ip];
                if (!changedInstruction)
                {
                    switch (instruction.Opcode)
                    {
                        case "nop" when instruction.Value != 0 && !closedList.Contains(ip) &&
                                        ip - instruction.Value > 0 && ip + instruction.Value <= input.Count:
                            // change to jmp if instruction falls within boundaries and not on trylist or causes infinite loop
                            instruction = new Instruction("jmp", instruction.Value);
                            closedList.Add(ip);
                            changedInstruction = true;
                            break;
                        case "jmp" when !closedList.Contains(ip):
                            // change to nop if not in trylist
                            instruction = new Instruction("nop", instruction.Value);
                            closedList.Add(ip);
                            changedInstruction = true;
                            break;
                    }
                }

                switch (instruction.Opcode)
                {
                    case "acc":
                        acc += instruction.Value;
                        break;
                    case "jmp":
                        ip += instruction.Value - 1; // -1 because the for loop always increases the ip by 1
                        break;
                    case "nop":
                        break;
                }
            }
        } while (hasError);

        return acc.ToString();
    }

    private record Instruction(string Opcode, int Value);
}