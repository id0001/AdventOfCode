using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode2019.IntCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await InputReader.ReadLineAsync<long>(19, ',').ToArrayAsync();
        var cpu = new Cpu();

        var beamCount = 0;

        cpu.SetProgram(program);
        cpu.RegisterOutput(x => { beamCount += (int) x; });

        for (var y = 0; y < 50; y++)
        for (var x = 0; x < 50; x++)
            await cpu.StartAsync(x, y);

        return beamCount.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await InputReader.ReadLineAsync<long>(19, ',').ToArrayAsync();
        var cpu = new Cpu();

        var x = 0;
        var y = 0;

        var output = -1;
        cpu.SetProgram(program);
        cpu.RegisterOutput(p => { output = (int) p; });

        while (true)
        {
            await cpu.StartAsync(x, y + 99); // Sample bottom left.
            while (output != 1)
            {
                x++;
                await cpu.StartAsync(x, y + 99); // Sample bottom left.
            }

            await cpu.StartAsync(x + 99, y); // Sample top right.
            if (output == 1) return (x * 10000 + y).ToString();

            y++;
        }
    }
}