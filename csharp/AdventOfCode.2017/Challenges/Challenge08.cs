using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Assembly;

namespace AdventOfCode2017.Challenges;

[Challenge(8)]
public class Challenge08(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ParseLinesAsync(8, ParseLine).ToArrayAsync();
        var cpu = new Cpu<RegisterMemory<int>, Arguments>(new RegisterMemory<int>(), program);

        var ignore = 0;
        cpu.AddInstruction("inc", (args, mem) => Inc(args, mem, ref ignore));
        cpu.AddInstruction("dec", (args, mem) => Dec(args, mem, ref ignore));

        cpu.RunTillHalted();
        return cpu.Memory.Registers.MaxBy(r => r.Value).Value.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ParseLinesAsync(8, ParseLine).ToArrayAsync();
        var cpu = new Cpu<RegisterMemory<int>, Arguments>(new RegisterMemory<int>(), program);

        var maxValue = 0;
        cpu.AddInstruction("inc", (args, mem) => Inc(args, mem, ref maxValue));
        cpu.AddInstruction("dec", (args, mem) => Dec(args, mem, ref maxValue));

        cpu.RunTillHalted();
        return maxValue.ToString();
    }

    private static void Dec(Arguments args, RegisterMemory<int> mem, ref int maxValue)
    {
        var regVal = mem.Get(args.Register) - args.Compare switch
        {
            ">" when mem.Get(args.CondtionA) > args.ConditionB => args.Mod,
            "<" when mem.Get(args.CondtionA) < args.ConditionB => args.Mod,
            ">=" when mem.Get(args.CondtionA) >= args.ConditionB => args.Mod,
            "<=" when mem.Get(args.CondtionA) <= args.ConditionB => args.Mod,
            "==" when mem.Get(args.CondtionA) == args.ConditionB => args.Mod,
            "!=" when mem.Get(args.CondtionA) != args.ConditionB => args.Mod,
            _ => 0
        };

        mem.Set(args.Register, regVal);
        mem.Ip++;
        maxValue = Math.Max(maxValue, mem.Registers.MaxBy(r => r.Value).Value);
    }

    private static void Inc(Arguments args, RegisterMemory<int> mem, ref int maxValue)
    {
        var regVal = mem.Get(args.Register) + args.Compare switch
        {
            ">" when mem.Get(args.CondtionA) > args.ConditionB => args.Mod,
            "<" when mem.Get(args.CondtionA) < args.ConditionB => args.Mod,
            ">=" when mem.Get(args.CondtionA) >= args.ConditionB => args.Mod,
            "<=" when mem.Get(args.CondtionA) <= args.ConditionB => args.Mod,
            "==" when mem.Get(args.CondtionA) == args.ConditionB => args.Mod,
            "!=" when mem.Get(args.CondtionA) != args.ConditionB => args.Mod,
            _ => 0
        };

        mem.Set(args.Register, regVal);
        mem.Ip++;
        maxValue = Math.Max(maxValue, mem.Registers.MaxBy(r => r.Value).Value);
    }

    private static Instruction<Arguments> ParseLine(string line)
    {
        var extract = line.Extract(@"(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)");
        var register = extract[0];
        var op = extract[1];
        var mod = extract[2].As<int>();
        var conditionA = extract[3];
        var compare = extract[4];
        var conditionB = extract[5].As<int>();

        return new Instruction<Arguments>(op, new Arguments(register, mod, conditionA, compare, conditionB));
    }

    private record Arguments(string Register, int Mod, string CondtionA, string Compare, int ConditionB);
}