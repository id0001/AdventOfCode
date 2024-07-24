using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2017.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ParseLinesAsync(18, ParseInput).ToListAsync();

        var cpu = new Cpu<long, MemoryEx>();
        AddBaseInstructions(cpu);
        cpu.AddInstruction("snd", (args, ctx) => { ctx.Rcv = args.Value("X"); ctx.Ip++; });
        cpu.AddInstruction("rcv", (args, ctx) => ctx.Ip = args.Value("X") > 0 ? -1 : ctx.Ip + 1); // Halt

        cpu.LoadProgram(program);
        cpu.RunTillHalted();

        return cpu.Memory.Rcv.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ParseLinesAsync(18, ParseInput).ToListAsync();

        var cpu0 = new Cpu<long, MemoryEx>();
        var cpu1 = new Cpu<long, MemoryEx>();

        AddBaseInstructions(cpu0);
        AddBaseInstructions(cpu1);

        AddPart2Instructions(cpu0, cpu1);
        AddPart2Instructions(cpu1, cpu0);

        cpu0.LoadProgram(program);
        cpu0.Memory.Set("p", 0);

        cpu1.LoadProgram(program);
        cpu1.Memory.Set("p", 1);

        while (!(cpu0.Memory.IsWaiting && cpu1.Memory.IsWaiting))
        {
            cpu0.Next();
            cpu1.Next();
        }

        return cpu1.Memory.SendCounter.ToString();
    }

    private static Instruction<long> ParseInput(string line) => line
        .SplitBy(" ")
        .Into(args => new Instruction<long>(
            args.First(),
            new[] { "X", "Y" }
                .Zip(args.Skip(1))
                .ToDictionary(kv => kv.First, kv => kv.Second)
            )
        );

    private static void AddBaseInstructions(Cpu<long, MemoryEx> cpu)
    {
        cpu.AddInstruction("set", (args, ctx) => { ctx.Set(args.Register("X"), args.Value("Y")); ctx.Ip++; });
        cpu.AddInstruction("add", (args, ctx) => { ctx.Set(args.Register("X"), args.Value("X") + args.Value("Y")); ctx.Ip++; });
        cpu.AddInstruction("mul", (args, ctx) => { ctx.Set(args.Register("X"), args.Value("X") * args.Value("Y")); ctx.Ip++; });
        cpu.AddInstruction("mod", (args, ctx) => { ctx.Set(args.Register("X"), args.Value("X") % args.Value("Y")); ctx.Ip++; });
        cpu.AddInstruction("jgz", (args, ctx) => ctx.Ip = args.Value("X") > 0 ? ctx.Ip + (int)args.Value("Y") : ctx.Ip + 1);
    }

    private static void AddPart2Instructions(Cpu<long, MemoryEx> cpu, Cpu<long, MemoryEx> other)
    {
        cpu.AddInstruction("snd", (args, ctx) => { other.Memory.MessageQueue.Enqueue(args.Value("X")); ctx.SendCounter++; ctx.Ip++; });
        cpu.AddInstruction("rcv", (args, ctx) =>
        {
            if (ctx.MessageQueue.Count == 0)
                ctx.IsWaiting = true;
            else
            {
                ctx.IsWaiting = false;
                ctx.Set(args.Register("X"), ctx.MessageQueue.Dequeue());
                ctx.Ip++;
            }
        });
    }

    private class MemoryEx() : DefaultMemory<long>
    {
        public long Rcv { get; set; }

        public int SendCounter { get; set; }

        public Queue<long> MessageQueue { get; set; } = new();

        public bool IsWaiting { get; set; }
    }
}