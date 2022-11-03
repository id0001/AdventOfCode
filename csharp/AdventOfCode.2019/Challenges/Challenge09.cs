using AdventOfCode2019.IntCode.Core;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(9)]
public class Challenge09
{
    private readonly IInputReader _inputReader;

    public Challenge09(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(9, ',').ToArrayAsync();
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
        var program = await _inputReader.ReadLineAsync<long>(9, ',').ToArrayAsync();
        long output = 0;
        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o => output = o);
        await cpu.StartAsync(2);
        return output.ToString();
    }
}