using System.Diagnostics;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2024.Challenges;

[Challenge(17)]
public class Challenge17(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        Test(0,0,9,[2,6],  (a,b,c,r) => b == 1);
        Test(10, 0, 0, [5, 0, 5, 1,5,4], (a,b,c,r) => r == "0,1,2");
        Test(2024, 0, 0, [0,1,5,4,3,0], (a,b,c,r) => r == "4,2,5,6,7,7,7,7,3,1,0" && a == 0);
        Test(0,29,0, [1,7],(a,b,c,r) => b == 26);
        Test(0,2024,43690, [4,0], (a,b,c,r) => b == 44354);
        
        var (cpu, _) = await inputReader.ParseTextAsync(17, ParseInput);
        
        cpu.AddInstruction(0, Adv);
        cpu.AddInstruction(1, Bxl);
        cpu.AddInstruction(2, Bst);
        cpu.AddInstruction(3, Jnz);
        cpu.AddInstruction(4, Bxc);
        cpu.AddInstruction(5, Out);
        cpu.AddInstruction(6, Bdv);
        cpu.AddInstruction(7, Cdv);

        cpu.RunTillHalted();
        return string.Join(",", cpu.Memory.Output);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (cpu, program) = await inputReader.ParseTextAsync(17, ParseInput);
        
        cpu.AddInstruction(0, Adv);
        cpu.AddInstruction(1, Bxl);
        cpu.AddInstruction(2, Bst);
        cpu.AddInstruction(3, Jnz);
        cpu.AddInstruction(4, Bxc);
        cpu.AddInstruction(5, Out);
        cpu.AddInstruction(6, Bdv);
        cpu.AddInstruction(7, Cdv);

        for (var a = 0; a <= 1000; a++)
        {
            cpu.Memory.Clear();
            cpu.Memory.Set('A', a);
            cpu.RunTillHalted();
        
            var result = string.Join(",", cpu.Memory.Output);
            if (result == program)
                return a.ToString();
            Console.WriteLine($"{a}: {result}");
        }

        return string.Empty;
    }

    private static void Test(int a, int b, int c, int[] program, Func<long,long,long,string, bool> result)
    {
        var memory = new Memory();
        memory.Set('A',a);
        memory.Set('B',b);
        memory.Set('C',c);
        
        var cpu = new Cpu<Memory,int,int>(memory, program.Windowed(2).Select(i => new Instruction<int,int>(i[0],i[1])).ToList());
        cpu.AddInstruction(0, Adv);
        cpu.AddInstruction(1, Bxl);
        cpu.AddInstruction(2, Bst);
        cpu.AddInstruction(3, Jnz);
        cpu.AddInstruction(4, Bxc);
        cpu.AddInstruction(5, Out);
        cpu.AddInstruction(6, Bdv);
        cpu.AddInstruction(7, Cdv);
        
        cpu.RunTillHalted();

        Debug.Assert(result(
            memory.Get('A'),
            memory.Get('B'),
            memory.Get('C'),
            string.Join(",", memory.Output)
        ));
    }

    private static void Adv(int arg, Memory memory)
    {
        memory.Set('A', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip+=2;
    }

    private static void Bxl(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('B') ^ arg);
        memory.Ip+=2;
    }

    private static void Bst(int arg, Memory memory)
    {
        memory.Set('B', GetValue(memory, arg) % 8);
        memory.Ip+=2;
    }

    private static void Jnz(int arg, Memory memory)
    {
        if (memory.Get('A') == 0)
        {
            memory.Ip+=2;
            return;
        }

        memory.Ip = arg;
    }

    private static void Bxc(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('B') ^ memory.Get('C'));
        memory.Ip+=2;
    }

    private static void Out(int arg, Memory memory)
    {
        memory.Output.Add(GetValue(memory, arg)  % 8);
        memory.Ip+=2;
    }

    private static void Bdv(int arg, Memory memory)
    {
        memory.Set('B', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip+=2;
    }
    
    private static void Cdv(int arg, Memory memory)
    {
        memory.Set('C', memory.Get('A') / (long)Math.Pow(2, GetValue(memory, arg)));
        memory.Ip+=2;
    }

    private static long GetValue(Memory memory, int operant) => operant switch
    {
        4 => memory.Get('A'),
        5 => memory.Get('B'),
        6 => memory.Get('C'),
        _ => operant
    };
    
    private static (Cpu<Memory, int, int>, string) ParseInput(string input)
    {
        var paragraphs = input.SelectParagraphs();

        var memory = new Memory();
        var abc = paragraphs[0]
            .SelectLines()
            .Select(line => line.Extract<int>(@"(\d+)")[0]).ToArray();
        memory.Set('A', abc[0]);
        memory.Set('B', abc[1]);
        memory.Set('C', abc[2]);

        var program = paragraphs[1]
            .Substring("Program: ".Length)
            .SplitBy(",")
            .As<int>()
            .Windowed(2)
            .Select(arr => new Instruction<int,int>(arr[0], arr[1]))
            .ToList();
        
        var cpu = new Cpu<Memory, int, int>(memory, program);
        return (cpu, paragraphs[1]);
    }

    private class Memory : RegisterMemory<char, long>
    {
        public List<long> Output { get; } = [];

        public override void Clear()
        {
            base.Clear();
            Output.Clear();
        }
    }
}
