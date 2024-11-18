using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2018.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    private static readonly Action<int[], RegisterMemory<int, int>>[] Operations
        = [Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr];

    [Part1]
    public async Task<string> Part1Async()
    {
        var (tests, _) = ParseText(await inputReader.ReadAllTextAsync(16));
        return tests.Where(t => TestOpcodes(t.Before, t.Input, t.After).Count(r => r) >= 3).Count().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (tests, prog) = ParseText(await inputReader.ReadAllTextAsync(16));

        var opCodes = DetermineOperations(tests.ToList());

        var memory = new RegisterMemory<int, int>();
        var cpu = new Cpu<RegisterMemory<int, int>, int, int[]>(memory, prog);
        for (var i = 0; i < opCodes.Length; i++)
        {
            var opcode = opCodes[i];
            cpu.AddInstruction(i, (a, r) => ExecAndIncIp(opcode, a, r));
        }

        cpu.RunTillHalted();
        return memory.Get(0).ToString();
    }

    private Action<int[], RegisterMemory<int, int>>[] DetermineOperations(IList<Sample> tests)
    {
        var opCodes = new Action<int[], RegisterMemory<int, int>>[16];
        var known = new List<Action<int[], RegisterMemory<int, int>>>();
        var operations = new List<Action<int[], RegisterMemory<int, int>>>(Operations);

        // Until all operations are known
        while (known.Count < operations.Count)
        {
            var candidates = new List<Action<int[], RegisterMemory<int, int>>>[16];
            for (var i = 0; i < candidates.Length; i++)
                candidates[i] =
                    operations.Except(known)
                        .ToList(); // Set all possible candidates for each number (operations - known)

            foreach (var test in tests)
            {
                // Run every operation for each sample
                var res = TestOpcodes(test.Before, test.Input, test.After);
                for (var i = 0; i < res.Length; i++)
                    // For each failed operation remove the operation from the candidates for the test number
                    if (!res[i])
                        candidates[test.Input[0]].Remove(Operations[i]);
            }

            for (var i = 0; i < 16; i++)
                // When only 1 candidate remains it must be the correct opcode
                if (candidates[i].Count == 1)
                {
                    opCodes[i] = candidates[i][0];
                    known.Add(opCodes[i]);
                }
        }

        return opCodes;
    }

    private bool[] TestOpcodes(int[] template, int[] arguments, int[] expected) =>
        Operations.Select(op => Test(template, arguments, expected, op)).ToArray();

    private static void ExecAndIncIp(Action<int[], RegisterMemory<int, int>> action, int[] args,
        RegisterMemory<int, int> registers)
    {
        action(args, registers);
        registers.Ip++;
    }

    private static void Addr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) + registers.Get(args[2]));

    private static void Addi(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) + args[2]);

    private static void Mulr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) * registers.Get(args[2]));

    private static void Muli(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) * args[2]);

    private static void Banr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) & registers.Get(args[2]));

    private static void Bani(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) & args[2]);

    private static void Borr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) | registers.Get(args[2]));

    private static void Bori(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) | args[2]);

    private static void Setr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]));

    private static void Seti(int[] args, RegisterMemory<int, int> registers) => registers.Set(args[3], args[1]);

    private static void Gtir(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], args[1] > registers.Get(args[2]) ? 1 : 0);

    private static void Gtri(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) > args[2] ? 1 : 0);

    private static void Gtrr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) > registers.Get(args[2]) ? 1 : 0);

    private static void Eqir(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], args[1] == registers.Get(args[2]) ? 1 : 0);

    private static void Eqri(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) == args[2] ? 1 : 0);

    private static void Eqrr(int[] args, RegisterMemory<int, int> registers) =>
        registers.Set(args[3], registers.Get(args[1]) == registers.Get(args[2]) ? 1 : 0);

    private bool Test(int[] startingRegisters, int[] arguments, int[] expectedOutput,
        Action<int[], RegisterMemory<int, int>> action)
    {
        var registers = new RegisterMemory<int, int>();
        for (var i = 0; i < startingRegisters.Length; i++)
            registers.Set(i, startingRegisters[i]);
        action(arguments, registers);
        return registers.Registers.All(kv => expectedOutput[kv.Key] == kv.Value);
    }

    private static (IEnumerable<Sample> TestOutput, IList<Instruction<int, int[]>> Program) ParseText(string input)
    {
        var nl = Environment.NewLine;
        return input
            .SplitBy($"{nl}{nl}{nl}")
            .Into(split => (split.First().SplitBy($"{nl}{nl}").Select(ExtractTest), ExtractProgram(split.Second())));
    }

    private static Sample ExtractTest(string input) => input
        .SplitBy(Environment.NewLine)
        .Into(parts => new Sample(
            parts.First().Extract<int>(@"Before: \[(\d), (\d), (\d), (\d)\]"),
            parts.Second().SplitBy(" ").As<int>().ToArray(),
            parts.Third().Extract<int>(@"After:  \[(\d), (\d), (\d), (\d)\]")
        ));

    private static IList<Instruction<int, int[]>> ExtractProgram(string input) => input
        .SplitBy(Environment.NewLine)
        .Select(line => line.SplitBy(" ").As<int>().Into(args => new Instruction<int, int[]>(args[0], args.ToArray())))
        .ToList();

    private record Sample(int[] Before, int[] Input, int[] After);
}