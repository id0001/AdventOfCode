using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var registers = new Dictionary<string, int>
        {
            {"a", 0},
            {"b", 0},
            {"c", 0},
            {"d", 0}
        };

        var program = await inputReader.ParseLinesAsync(12, line => line.SplitBy(" ")).ToArrayAsync();

        RunProgram(program, registers);

        return registers["a"].ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var registers = new Dictionary<string, int>
        {
            {"a", 0},
            {"b", 0},
            {"c", 1},
            {"d", 0}
        };

        var program = await inputReader.ParseLinesAsync(12, line => line.SplitBy(" ")).ToArrayAsync();

        RunProgram(program, registers);

        return registers["a"].ToString();
    }

    private static void RunProgram(string[][] program, Dictionary<string, int> registers)
    {
        var ip = 0;
        while (ip < program.Length)
        {
            _ = program[ip] switch
            {
                ["cpy", var x, var y] when int.TryParse(x, out var ix) => registers[y] = ix,
                ["cpy", var x, var y] => registers[y] = registers[x],
                ["jnz", var x, var y] when int.TryParse(x, out var ix) => ip = ix != 0 ? ip + int.Parse(y) - 1 : ip,
                ["jnz", var x, var y] => ip = registers[x] != 0 ? ip + int.Parse(y) - 1 : ip,
                ["inc", var x] => registers[x] = registers[x] + 1,
                ["dec", var x] => registers[x] = registers[x] - 1,
                _ => throw new NotImplementedException()
            };

            ip++;
        }
    }
}