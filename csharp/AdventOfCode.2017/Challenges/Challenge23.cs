using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2017.Challenges;

[Challenge(23)]
public class Challenge23(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ParseLinesAsync(23, ParseLine).ToListAsync();

        var cpu = new Cpu<MemoryEx, Arguments>(new MemoryEx(), program);
        cpu.AddInstruction("set", (args, mem) =>
        {
            mem.Set(args.X, args.Y.Value(mem));
            mem.Ip++;
        });
        cpu.AddInstruction("sub", (args, mem) =>
        {
            mem.Set(args.X, args.X.Value(mem) - args.Y.Value(mem));
            mem.Ip++;
        });
        cpu.AddInstruction("mul", (args, mem) =>
        {
            mem.Set(args.X, args.X.Value(mem) * args.Y.Value(mem));
            mem.Ip++;
            mem.MulCalled++;
        });
        cpu.AddInstruction("jnz", (args, mem) => mem.Ip += args.X.Value(mem) != 0 ? args.Y.Value(mem) : 1);

        cpu.RunTillHalted();
        return cpu.Memory.MulCalled.ToString();
    }

    [Part2]
    public Task<string> Part2Async()
    {
        var b = 57 * 100 + 100000;
        var c = b + 17000;
        var h = 0;

        for (var i = b; i <= c; i += 17)
        {
            var d = 2;
            while (i % d != 0)
                d++;

            if (i != d)
                h++;
        }

        return Task.FromResult(h.ToString());
    }

    private static Instruction<Arguments> ParseLine(string line) => line
        .SplitBy(" ")
        .Into(parts => new Instruction<Arguments>(parts.First(), new Arguments(parts.Second(), parts.Third())));

    private record Arguments(RegisterOrValueArgument<int> X, RegisterOrValueArgument<int> Y);

    private class MemoryEx : RegisterMemory<int>
    {
        public int MulCalled { get; set; }
    }
}