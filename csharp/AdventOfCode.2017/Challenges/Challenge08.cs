using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(8)]
public class Challenge08(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ReadLinesAsync(8).Select(line => new KeyValuePair<string, string>(string.Empty, line)).ToListAsync();
        var cpu = new Cpu<int>();
        cpu.InstructionSet.Add(string.Empty, GeneralInstruction);
        cpu.Program = program;

        while (cpu.MoveNext())
        {
        }

        return cpu.Registers.MaxBy(r => r.Value).Value.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ReadLinesAsync(8).Select(line => new KeyValuePair<string, string>(string.Empty, line)).ToListAsync();
        var cpu = new Cpu<int>();
        cpu.InstructionSet.Add(string.Empty, GeneralInstruction);
        cpu.Program = program;

        int maxValue = int.MinValue;
        while (cpu.MoveNext())
        {
            maxValue = Math.Max(maxValue, cpu.Registers.MaxBy(r => r.Value).Value);
        }

        return maxValue.ToString();
    }

    private static int GeneralInstruction(Cpu<int> cpu, string instruction)
    {
        var extract = instruction.Extract(@"(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)");

        var register = extract[0];
        var op = extract[1];
        var mod = extract[2].As<int>();
        var conditionA = extract[3];
        var compare = extract[4];
        var conditionB = extract[5].As<int>();

        cpu.Set(register, cpu.Get(register) + compare switch
        {
            ">" when (cpu.Get(conditionA) > conditionB && op == "inc") => mod,
            ">" when (cpu.Get(conditionA) > conditionB && op == "dec") => -mod,
            "<" when (cpu.Get(conditionA) < conditionB && op == "inc") => mod,
            "<" when (cpu.Get(conditionA) < conditionB && op == "dec") => -mod,
            ">=" when (cpu.Get(conditionA) >= conditionB && op == "inc") => mod,
            ">=" when (cpu.Get(conditionA) >= conditionB && op == "dec") => -mod,
            "<=" when (cpu.Get(conditionA) <= conditionB && op == "inc") => mod,
            "<=" when (cpu.Get(conditionA) <= conditionB && op == "dec") => -mod,
            "==" when (cpu.Get(conditionA) == conditionB && op == "inc") => mod,
            "==" when (cpu.Get(conditionA) == conditionB && op == "dec") => -mod,
            "!=" when (cpu.Get(conditionA) != conditionB && op == "inc") => mod,
            "!=" when (cpu.Get(conditionA) != conditionB && op == "dec") => -mod,
            _ => 0
        });

        return cpu.Ip + 1;
    }


    private class Cpu<TRegister>
    {
        public Dictionary<string, TRegister> Registers { get; } = new();

        public Dictionary<string, Func<Cpu<TRegister>, string, int>> InstructionSet { get; } = new();

        public List<KeyValuePair<string, string>> Program { get; set; }

        public int Ip { get; set; }

        public bool Halt { get; set; }

        public Func<Cpu<TRegister>, bool> HaltCondition { get; set; } = x => x.Program == null || x.Ip >= x.Program.Count;

        public bool MoveNext()
        {
            if (Halt || HaltCondition(this))
                return false;

            var current = Program[Ip];
            var instruction = InstructionSet[current.Key];

            Ip = instruction.Invoke(this, current.Value);
            return true;
        }

        public void Reset()
        {
            Halt = false;
            Ip = 0;
            Registers.Clear();
        }

        public TRegister? Get(string register) => Registers!.GetValueOrDefault(register, default);

        public void Set(string register, TRegister value) => Registers[register] = value;
    }
}