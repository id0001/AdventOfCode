using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode2019.IntCode.Core;

namespace AdventOfCode2019.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await inputReader.ReadLineAsync<long>(13, ',').ToArrayAsync();

        var screenBuffer = new int[64, 64];
        var outputBuffer = new Queue<int>();

        var ballX = 0;
        var paddleX = 0;

        var cpu = new Cpu();
        cpu.SetProgram(program);

        cpu.RegisterOutput(o =>
        {
            outputBuffer.Enqueue((int) o);

            if (outputBuffer.Count != 3) return;

            var x = outputBuffer.Dequeue();
            var y = outputBuffer.Dequeue();
            var id = outputBuffer.Dequeue();

            if (x == -1 && y == 0)
            {
                // Draw score
                screenBuffer[0, 0] = id;
            }
            else
            {
                // Draw tile
                screenBuffer[y + 2, x] = id;

                switch (id)
                {
                    case 4:
                        ballX = x;
                        break;
                    case 3:
                        paddleX = x;
                        break;
                }
            }
        });

        cpu.RegisterInput(() =>
        {
            if (ballX == paddleX)
                cpu.WriteInput(0);
            else if (ballX < paddleX)
                cpu.WriteInput(-1);
            else
                cpu.WriteInput(1);
        });

        await cpu.StartAsync();

        return screenBuffer.OfType<int>().Count(x => x == 2).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await inputReader.ReadLineAsync<long>(13, ',').ToArrayAsync();

        var screenBuffer = new int[64, 64];
        var outputBuffer = new Queue<int>();

        var ballX = 0;
        var paddleX = 0;

        var cpu = new Cpu();
        program[0] = 2;
        cpu.SetProgram(program);

        cpu.RegisterOutput(o =>
        {
            outputBuffer.Enqueue((int) o);

            if (outputBuffer.Count != 3) return;

            var x = outputBuffer.Dequeue();
            var y = outputBuffer.Dequeue();
            var id = outputBuffer.Dequeue();

            if (x == -1 && y == 0)
            {
                // Draw score
                screenBuffer[0, 0] = id;
            }
            else
            {
                // Draw tile
                screenBuffer[y + 2, x] = id;

                switch (id)
                {
                    case 4:
                        ballX = x;
                        break;
                    case 3:
                        paddleX = x;
                        break;
                }
            }
        });

        cpu.RegisterInput(() =>
        {
            if (ballX == paddleX)
                cpu.WriteInput(0);
            else if (ballX < paddleX)
                cpu.WriteInput(-1);
            else
                cpu.WriteInput(1);
        });

        await cpu.StartAsync();

        return screenBuffer[0, 0].ToString();
    }
}