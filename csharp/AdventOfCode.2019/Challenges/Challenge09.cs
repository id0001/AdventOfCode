using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode2019.IntCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await InputReader.ReadLineAsync<long>(9, ',').ToArrayAsync();
        long output = 0;
        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o => output = o);
        await cpu.StartAsync(1);
        return output.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await InputReader.ReadLineAsync<long>(9, ',').ToArrayAsync();
        long output = 0;
        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o => output = o);
        await cpu.StartAsync(2);
        return output.ToString();
    }
}