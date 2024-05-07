using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(23)]
public class Challenge23(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var registers = new Dictionary<string, int>
        {
            {"a", 7},
            {"b", 0},
            {"c", 0},
            {"d", 0}
        };

        var program = await inputReader.ParseLinesAsync(23, line => line.SplitBy(" ")).ToArrayAsync();

        RunProgram(program, registers);

        return registers["a"].ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var registers = new Dictionary<string, int>
        {
            {"a", 12},
            {"b", 0},
            {"c", 0},
            {"d", 0}
        };

        var program = await inputReader.ParseLinesAsync(23, line => line.SplitBy(" ")).ToArrayAsync();

        RunProgram(program, registers);

        return registers["a"].ToString();
    }

    private static void RunProgram(string[][] program, Dictionary<string, int> registers)
    {
        var ip = 0;
        while (ip < program.Length)
        {
            if (ip + 5 < program.Length && program[ip + 5][0] == "jnz" && program[ip + 5][2] == "-5" && !int.TryParse(program[ip][1], out _))
            {
                Multiply(program, registers, ip);
                ip += 6;
                continue;
            }

            ip = program[ip] switch
            {
            ["cpy", var x, var y] when int.TryParse(x, out _) && int.TryParse(y, out _) => ip + 1, // invalid
            ["cpy", var x, var y] when int.TryParse(x, out var ix) => (registers[y] = ix, ip + 1).Item2, // cpy 0 a
            ["cpy", var x, var y] => (registers[y] = registers[x], ip + 1).Item2,

            ["jnz", var x, var y] when int.TryParse(x, out var ix) && int.TryParse(y, out var iy) => ix != 0 ? ip + iy : ip + 1, // jnz 1 3
            ["jnz", var x, var y] when int.TryParse(x, out var ix) => ix != 0 ? ip + registers[y] : ip + 1, // jnz 3 a
            ["jnz", var x, var y] when int.TryParse(y, out var iy) => registers[x] != 0 ? ip + iy : ip + 1, // jnz a 2
            ["jnz", var x, var y] => registers[x] != 0 ? ip + registers[y] : ip + 1, // jnz a b

            ["inc", var x] => (registers[x] += 1, ip + 1).Item2, // inc a

            ["dec", var x] => (registers[x] -= 1, ip + 1).Item2, // dec a

            ["tgl", var x] when int.TryParse(x, out var ix) => Toggle(program, ip, ix), // tgl 3
            ["tgl", var x] => Toggle(program, ip, registers[x]), // tgl a

                _ => throw new NotImplementedException()
            };
        }
    }

    private static int Toggle(string[][] program, int ip, int x)
    {
        var loc = ip + x;

        if (loc < 0 || loc >= program.Length)
            return ip + 1;

        _ = program[loc][0] switch
        {
            "inc" => program[loc][0] = "dec",
            "jnz" => program[loc][0] = "cpy",
            _ when (program[loc].Length == 2) => program[loc][0] = "inc",
            _ when (program[loc].Length == 3) => program[loc][0] = "jnz",
            _ => throw new NotImplementedException()
        };

        return ip + 1;
    }

    private static void Multiply(string[][] program, Dictionary<string, int> registers, int ip)
    {
        /*
            cpy b c // (b == value)
            inc a // a == result (not b or d or c)
            dec c
            jnz c -2
            dec d
            jnz d -5 -- d == multiplier (b * d)
        */

        var valueReg = program[ip][1];
        var multiplierReg = program[ip + 5][1];

        var between = program[ip + 3][1];

        var resultReg = program[(ip+1)..(ip + 6)].Single(inst => inst[0] != "jnz" && inst[1] != valueReg && inst[1] != multiplierReg && inst[1] != between)[1];

        registers[resultReg] = registers[valueReg] * registers[multiplierReg];
        registers[multiplierReg] = 0;
    }
}