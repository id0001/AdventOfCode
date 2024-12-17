using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2024.Challenges;

[Challenge(17)]
public class Challenge17(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (cpu, _) = await inputReader.ParseTextAsync(17, ParseInput);
        cpu.RunTillHalted();
        return string.Join(",", cpu.Memory.Output);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (cpu, program) = await inputReader.ParseTextAsync(17, ParseInput);
        return FindValueOfA(cpu, 15, 0, program).ToString();
    }

    private static long FindValueOfA(Assembler<Memory, int, int> cpu, int digitsRemaining, long value, string target)
    {
        for (var i = 0; i < 8; i++)
        {
            var a = value * 8L + i; // Multiply the existing value by 8 and add the current digit
            cpu.Memory.Clear();
            cpu.Memory.Set('A', a);
            cpu.RunTillHalted();

            if (!target.EndsWith(string.Join(",", cpu.Memory.Output)))
                continue; // Not the correct digit

            if (digitsRemaining == 0)
                return a; // The correct value for a is found

            a = FindValueOfA(cpu, digitsRemaining - 1, a, target);
            if (a == -1)
                continue; // Wrong path taken. Different values can lead to the same output but result in errors later down the line

            return a;
        }

        return -1; // Non of the digits result in the correct program
    }

    private static void Adv(int arg, Memory memory)
    {
        memory.Set('A', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip += 2;
    }

    private static void Bxl(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('B') ^ arg);
        memory.Ip += 2;
    }

    private static void Bst(int arg, Memory memory)
    {
        memory.Set('B', GetValue(memory, arg) % 8);
        memory.Ip += 2;
    }

    private static void Jnz(int arg, Memory memory)
    {
        if (memory.Get('A') == 0)
        {
            memory.Ip += 2;
            return;
        }

        memory.Ip = arg;
    }

    private static void Bxc(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('B') ^ memory.Get('C'));
        memory.Ip += 2;
    }

    private static void Out(int arg, Memory memory)
    {
        memory.Output.Add(GetValue(memory, arg) % 8);
        memory.Ip += 2;
    }

    private static void Bdv(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip += 2;
    }

    private static void Cdv(int arg, Memory memory)
    {
        memory.Set('C', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip += 2;
    }

    private static long GetValue(Memory memory, int operant) => operant switch
    {
        4 => memory.Get('A'),
        5 => memory.Get('B'),
        6 => memory.Get('C'),
        _ => operant
    };

    private static (Assembler<Memory, int, int>, string) ParseInput(string input)
    {
        var paragraphs = input.SelectParagraphs();

        var program = paragraphs[1]
            .Substring("Program: ".Length)
            .SplitBy(",")
            .As<int>()
            .Windowed(2)
            .Select(arr => new Instruction<int, int>(arr[0], arr[1]))
            .ToList();

        var memory = new Memory(program);
        var abc = paragraphs[0]
            .SelectLines()
            .Select(line => line.Extract<int>(@"(\d+)")[0]).ToArray();
        memory.Set('A', abc[0]);
        memory.Set('B', abc[1]);
        memory.Set('C', abc[2]);

        var cpu = new Assembler<Memory, int, int>(memory);

        cpu.AddInstruction(0, Adv);
        cpu.AddInstruction(1, Bxl);
        cpu.AddInstruction(2, Bst);
        cpu.AddInstruction(3, Jnz);
        cpu.AddInstruction(4, Bxc);
        cpu.AddInstruction(5, Out);
        cpu.AddInstruction(6, Bdv);
        cpu.AddInstruction(7, Cdv);

        return (cpu, paragraphs[1]);
    }

    private class Memory(IList<Instruction<int, int>> program)
        : RegisterMemory<char, long, Instruction<int, int>>(program)
    {
        public List<long> Output { get; } = [];

        public override void Clear()
        {
            base.Clear();
            Output.Clear();
        }
    }
}
