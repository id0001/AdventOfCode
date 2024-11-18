using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2018.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader inputReader)
{
    private static readonly Action<Argument, Memory>[] Operations
        = [Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr];

    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await inputReader.ReadLinesAsync(19).ToListAsync();
        var program = CreateProgram(lines, out var ipRegister);

        var memory = new Memory {IpRegister = ipRegister};
        var cpu = new Cpu<Memory, string, Argument>(memory, program);
        LoadInstructions(cpu);
        cpu.RunTillHalted();

        return memory.Get(0).ToString();
    }

    [Part2]
    public string Part2()
    {
        var sum = 0;
        for (var i = 1; i <= 10551319; i++)
            if (10551319 % i == 0)
                sum += i;

        return sum.ToString();
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
        memory.Set(memory.IpRegister, memory.Ip);
        action(args, memory);
        memory.Ip = memory.Get(memory.IpRegister) + 1;
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

    private class Memory : RegisterMemory<int, int>
    {
        public int IpRegister { get; set; }

        public override void Clear()
        {
            base.Clear();
            IpRegister = 0;
        }
    }
}