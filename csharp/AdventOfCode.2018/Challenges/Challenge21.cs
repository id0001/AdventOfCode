using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2018.Challenges;

[Challenge(21)]
public class Challenge21(IInputReader inputReader)
{
    private static readonly Action<Argument, Memory>[] Operations
        = [Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr];

    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await inputReader.ReadLinesAsync(21).ToListAsync();
        var program = CreateProgram(lines, out var ipRegister);

        var memory = new Memory(ipRegister);
        var cpu = new Cpu<Memory, string, Argument>(memory, program);
        LoadInstructions(cpu);

        memory.Clear();
        cpu.RunTillHalted();
        return memory.Get(4).ToString();
    }

    [Part2]
    public string Part2()
    {
        var seen = new HashSet<int>();
        var lastSeen = 0;

        var r4 = 123;
        while (true)
        {
            r4 &= 456;
            if (r4 == 72)
                break;
        }

        r4 = 0;
        while (true)
        {
            var r5 = r4 | 65536;
            r4 = 10704114;

            while (true)
            {
                var r2 = r5 & 255;
                r4 += r2;
                r4 &= 16777215;
                r4 *= 65899;
                r4 &= 16777215;

                if (256 > r5)
                {
                    if (seen.Contains(r4))
                        return lastSeen.ToString();

                    seen.Add(r4);
                    lastSeen = r4;
                    break;
                }

                r2 = 0;
                while (true)
                {
                    var r3 = r2 + 1;
                    r3 *= 256;
                    if (r3 > r5)
                    {
                        r5 = r2;
                        break;
                    }

                    r2++;
                }
            }
        }
    }

    private static IList<Instruction<string, Argument>> CreateProgram(IList<string> lines, out int ipRegister)
    {
        ipRegister = lines[0].Extract<int>(@"#ip (\d)").First();
        return lines
            .Skip(1)
            .Select(line => line
                .Extract<string, int, int, int>(@"(.+) (\d+) (\d+) (\d+)")
                .Into(parts =>
                    new Instruction<string, Argument>(parts.First,
                        new Argument(parts.Second, parts.Third, parts.Fourth)))
            )
            .ToList();
    }

    private static void LoadInstructions(Cpu<Memory, string, Argument> cpu)
    {
        foreach (var operation in Operations)
        {
            var opcode = operation.Method.Name.ToLower();
            cpu.AddInstruction(opcode, (a, m) => WrapInstruction(operation, a, m));
        }
    }

    private static void WrapInstruction(Action<Argument, Memory> action, Argument args, Memory memory)
    {
        if (memory.Ip == 28)
        {
            memory.Ip = -1; // halt
            return;
        }

        memory.Set(memory.IpRegister, memory.Ip);
        action(args, memory);
        memory.Ip = memory.Get(memory.IpRegister) + 1;
    }

    private static void Addr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) + memory.Get(args.B));

    private static void Addi(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) + args.B);

    private static void Mulr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) * memory.Get(args.B));

    private static void Muli(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) * args.B);

    private static void Banr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) & memory.Get(args.B));

    private static void Bani(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) & args.B);

    private static void Borr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) | memory.Get(args.B));

    private static void Bori(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) | args.B);
    private static void Setr(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A));
    private static void Seti(Argument args, Memory memory) => memory.Set(args.C, args.A);
    private static void Gtir(Argument args, Memory memory) => memory.Set(args.C, args.A > memory.Get(args.B) ? 1 : 0);
    private static void Gtri(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) > args.B ? 1 : 0);

    private static void Gtrr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) > memory.Get(args.B) ? 1 : 0);

    private static void Eqir(Argument args, Memory memory) => memory.Set(args.C, args.A == memory.Get(args.B) ? 1 : 0);
    private static void Eqri(Argument args, Memory memory) => memory.Set(args.C, memory.Get(args.A) == args.B ? 1 : 0);

    private static void Eqrr(Argument args, Memory memory) =>
        memory.Set(args.C, memory.Get(args.A) == memory.Get(args.B) ? 1 : 0);

    private record Argument(int A, int B, int C);

    private class Memory(int ipRegister) : RegisterMemory<int, int>
    {
        public int IpRegister { get; } = ipRegister;
    }
}