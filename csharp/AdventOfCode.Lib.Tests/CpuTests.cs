using AdventOfCode.Lib.Assembly;

namespace AdventOfCode.Lib.Tests;

[TestClass]
public class CpuTests
{
    [TestMethod]
    public async Task Test_2017_Day8_Part2()
    {
        var program = await InputReader.ParseLinesAsync("2017.8.2.txt", ParseLine8).ToListAsync();
        var cpu = new Cpu<RegisterMemory<int>, Args2017Day8>(new RegisterMemory<int>(), program);

        var maxValue = 0;
        cpu.AddInstruction("inc", (args, mem) =>
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
            maxValue = System.Math.Max(maxValue, mem.Registers.MaxBy(r => r.Value).Value);
        });

        cpu.AddInstruction("dec", (args, mem) =>
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
            maxValue = System.Math.Max(maxValue, mem.Registers.MaxBy(r => r.Value).Value);
        });

        cpu.RunTillHalted();

        Assert.AreEqual(7774, maxValue);
    }

    private static Instruction<Args2017Day8> ParseLine8(string line)
    {
        var extract = line.Extract(@"(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)");
        var register = extract[0];
        var op = extract[1];
        var mod = extract[2].As<int>();
        var conditionA = extract[3];
        var compare = extract[4];
        var conditionB = extract[5].As<int>();

        return new Instruction<Args2017Day8>(op, new Args2017Day8(register, mod, conditionA, compare, conditionB));
    }

    private record Args2017Day8(string Register, int Mod, string CondtionA, string Compare, int ConditionB);
}