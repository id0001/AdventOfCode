using AdventOfCode2019.IntCode.Core;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(25)]
public class Challenge25
{
    private readonly IInputReader _inputReader;

    public Challenge25(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(25, ',').ToArrayAsync();

        var cpu = new Cpu();
        cpu.SetProgram(program);

        // F this, I bruteforced it by hand.
        var buffer = string.Empty;
        cpu.RegisterOutput(o =>
        {
            buffer += (char)o;
            Console.Write((char)o);

            if (!buffer.EndsWith("Command?")) return;
            Console.WriteLine();
            Console.Write("> ");
            var cmd = Console.ReadLine();
            foreach (var c in cmd!)
                cpu.WriteInput(c);

            cpu.WriteInput('\n');
            Console.Clear();
            buffer = string.Empty;
        });

        await cpu.StartAsync();

        return null;
    }
}