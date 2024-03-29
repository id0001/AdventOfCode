﻿using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode2019.IntCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ReadLineAsync<long>(5, ',').ToArrayAsync();

        long result = -1;
        var cpu = new Cpu();
        cpu.RegisterOutput(o => result = o);
        cpu.SetProgram(program);
        await cpu.StartAsync(1);

        return result.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ReadLineAsync<long>(5, ',').ToArrayAsync();

        long result = -1;
        var cpu = new Cpu();
        cpu.RegisterOutput(o => result = o);
        cpu.SetProgram(program);
        await cpu.StartAsync(5);

        return result.ToString();
    }
}