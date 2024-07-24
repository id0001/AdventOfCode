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

        var cpu = new Cpu<MemoryEx, Arguments>(new(), program);
        AddBaseInstructions(cpu);
        cpu.AddInstruction("snd", (args, ctx) => { ctx.Rcv = ctx.Get(args.X); ctx.Ip++; });
        cpu.AddInstruction("rcv", (args, ctx) => ctx.Ip = ctx.Get(args.X) > 0 ? -1 : ctx.Ip + 1); // Halt

        cpu.RunTillHalted();

        return cpu.Memory.Rcv.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ParseLinesAsync(18, ParseInput).ToListAsync();

        var cpu0 = new Cpu<MemoryEx, Arguments>(new(), program);
        var cpu1 = new Cpu<MemoryEx, Arguments>(new(), program);

        AddBaseInstructions(cpu0);
        AddBaseInstructions(cpu1);

        AddPart2Instructions(cpu0, cpu1);
        AddPart2Instructions(cpu1, cpu0);

        cpu0.Memory.Set("p", 0);
        cpu1.Memory.Set("p", 1);

        while (!(cpu0.Memory.IsWaiting && cpu1.Memory.IsWaiting))
        {
            cpu0.Next();
            cpu1.Next();
        }

        return cpu1.Memory.SendCounter.ToString();
    }

    private static Instruction<Arguments> ParseInput(string line) => line
        .SplitBy(" ")
        .Into(args => new Instruction<Arguments>(
            args.First(),
            new Arguments(args.Second(), args.ThirdOrDefault(string.Empty))
            )
        );

    private static void AddBaseInstructions(Cpu<MemoryEx, Arguments> cpu)
    {
        cpu.AddInstruction("set", (args, ctx) => { ctx.Set(args.X, ctx.Get(args.Y)); ctx.Ip++; });
        cpu.AddInstruction("add", (args, ctx) => { ctx.Set(args.X, ctx.Get(args.X) + ctx.Get(args.Y)); ctx.Ip++; });
        cpu.AddInstruction("mul", (args, ctx) => { ctx.Set(args.X, ctx.Get(args.X) * ctx.Get(args.Y)); ctx.Ip++; });
        cpu.AddInstruction("mod", (args, ctx) => { ctx.Set(args.X, ctx.Get(args.X) % ctx.Get(args.Y)); ctx.Ip++; });
        cpu.AddInstruction("jgz", (args, ctx) => ctx.Ip = ctx.Get(args.X) > 0 ? ctx.Ip + (int)ctx.Get(args.Y) : ctx.Ip + 1);
    }

    private static void AddPart2Instructions(Cpu<MemoryEx, Arguments> cpu, Cpu<MemoryEx, Arguments> other)
    {
        cpu.AddInstruction("snd", (args, ctx) => { other.Memory.MessageQueue.Enqueue(ctx.Get(args.X)); ctx.SendCounter++; ctx.Ip++; });
        cpu.AddInstruction("rcv", (args, ctx) =>
        {
            if (ctx.MessageQueue.Count == 0)
                ctx.IsWaiting = true;
            else
            {
                ctx.IsWaiting = false;
                ctx.Set(args.X, ctx.MessageQueue.Dequeue());
                ctx.Ip++;
            }
        });
    }

    private class MemoryEx() : RegisterMemory<long>
    {
        public long Rcv { get; set; }

        public int SendCounter { get; set; }

        public Queue<long> MessageQueue { get; set; } = new();

        public bool IsWaiting { get; set; }
    }

    private record Arguments(string X, string Y);
}