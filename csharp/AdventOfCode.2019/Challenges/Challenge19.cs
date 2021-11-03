using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(19)]
    public class Challenge19
    {
        private readonly IInputReader inputReader;
        private long[] data;

        public Challenge19(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<long>(19, ',').ToArrayAsync();
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var cpu = new Cpu();

            int beamCount = 0;

            cpu.SetProgram(data);
            cpu.RegisterOutput(x =>
            {
                beamCount += (int)x;
            });

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    await cpu.StartAsync(x, y);
                }
            }

            return beamCount.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var cpu = new Cpu();

            int x = 0;
            int y = 0;

            int output = -1;
            cpu.SetProgram(data);
            cpu.RegisterOutput(p =>
            {
                output = (int)p;
            });

            while (true)
            {
                await cpu.StartAsync(x, y + 99); // Sample bottom left.
                while (output != 1)
                {
                    x++;
                    await cpu.StartAsync(x, y + 99); // Sample bottom left.
                }

                await cpu.StartAsync(x + 99, y); // Sample top right.
                if (output == 1)
                {
                    return ((x * 10000) + y).ToString();
                }

                y++;
            }
        }
    }
}
