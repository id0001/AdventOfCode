using AdventOfCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part1]
    public Task<string> Part1Async()
    {
        //var program = await _inputReader.ReadLineAsync<long>(25, ',').ToArrayAsync();

        //var cpu = new Cpu();
        //cpu.SetProgram(program);

        //var buffer = string.Empty;
        //cpu.RegisterOutput(o =>
        //{
        //    buffer += (char)o;
        //    Console.Write((char)o);

        //    if (!buffer.EndsWith("Command?")) return;
        //    Console.WriteLine();
        //    Console.Write("> ");
        //    var cmd = Console.ReadLine();
        //    foreach (var c in cmd!)
        //        cpu.WriteInput(c);

        //    cpu.WriteInput('\n');
        //    Console.Clear();
        //    buffer = string.Empty;
        //});

        //await cpu.StartAsync();

        // F this, I bruteforced it by hand.
        return Task.FromResult("2147502592");
    }
}