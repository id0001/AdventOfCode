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
        var program = await inputReader.ReadLinesAsync(8).ToArrayAsync();
        var registers = new Dictionary<string, int>();
        
        RunProgram(program, registers, out _);

        return registers.MaxBy(r => r.Value).Value.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ReadLinesAsync(8).ToArrayAsync();
        var registers = new Dictionary<string, int>();
        
        RunProgram(program, registers, out var maxValue);

        return maxValue.ToString();
    }

    private void RunProgram(string[] program, Dictionary<string, int> registers, out int maxValue)
    {
        var ip = 0;
        maxValue = int.MinValue;
        while (ip < program.Length)
        {
            var extract = program[ip].Extract(@"(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)");

            var register = extract[0];
            var op = extract[1];
            var mod = extract[2].As<int>();
            var conditionA = extract[3];
            var compare = extract[4];
            var conditionB = extract[5].As<int>();

            registers[register] = registers.GetValueOrDefault(register, 0) + compare switch
            {
                ">" when registers.GetValueOrDefault(conditionA, 0) > conditionB && op == "inc" => mod,
                ">" when registers.GetValueOrDefault(conditionA, 0) > conditionB && op == "dec" => -mod,
                "<" when registers.GetValueOrDefault(conditionA, 0) < conditionB && op == "inc" => mod,
                "<" when registers.GetValueOrDefault(conditionA, 0) < conditionB && op == "dec" => -mod,
                ">=" when registers.GetValueOrDefault(conditionA, 0) >= conditionB && op == "inc" => mod,
                ">=" when registers.GetValueOrDefault(conditionA, 0) >= conditionB && op == "dec" => -mod,
                "<=" when registers.GetValueOrDefault(conditionA, 0) <= conditionB && op == "inc" => mod,
                "<=" when registers.GetValueOrDefault(conditionA, 0) <= conditionB && op == "dec" => -mod,
                "==" when registers.GetValueOrDefault(conditionA, 0) == conditionB && op == "inc" => mod,
                "==" when registers.GetValueOrDefault(conditionA, 0) == conditionB && op == "dec" => -mod,
                "!=" when registers.GetValueOrDefault(conditionA, 0) != conditionB && op == "inc" => mod,
                "!=" when registers.GetValueOrDefault(conditionA, 0) != conditionB && op == "dec" => -mod,
                _ => 0
            };

            ip++;
            maxValue = Math.Max(maxValue, registers.MaxBy(r => r.Value).Value);
        }
    }
}