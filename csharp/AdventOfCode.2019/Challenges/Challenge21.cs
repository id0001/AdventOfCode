using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(21)]
    public class Challenge21
    {
        private readonly IInputReader inputReader;
        private long[] data;

        public Challenge21(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<long>(21, ',').ToArrayAsync();
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            long result = -1;

            int ip = 0;
            string[] instructions = new[]
            {
                Not('C','J'),
                And('D','J'),
                Not('B','T'),
                And('C','T'),
                Or('T','J'),
                Not('A','T'),
                And('B','T'),
                Or('T','J'),
                Walk()
            };

            var cpu = new Cpu();
            cpu.SetProgram(data);
            cpu.RegisterInput(() =>
            {
                WriteInstruction(cpu, instructions[ip++]);
            });

            cpu.RegisterOutput(o =>
            {
                if (o > 255)
                {
                    result = o;
                    return;
                }

                Console.Write((char)o);
            });

            await cpu.StartAsync();

            return result.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            long result = -1;

            int ip = 0;
            string[] instructions = new[]
            {
                Not('C','J'),
                And('D','J'),
                And('H','J'),
                Not('B','T'),
                And('C','T'),
                And('D','T'),
                Or('T','J'),
                Not('A','T'),
                And('B','T'),
                Or('T','J'),
                Not('A','T'),
                Or('T','J'),
                Run()
            };

            var cpu = new Cpu();
            cpu.SetProgram(data);
            cpu.RegisterInput(() =>
            {
                WriteInstruction(cpu, instructions[ip++]);
            });

            cpu.RegisterOutput(o =>
            {
                if (o > 255)
                {
                    result = o;
                    return;
                }

                Console.Write((char)o);
            });

            await cpu.StartAsync();

            return result.ToString();
        }

        private static string And(char x, char y) => $"AND {x} {y}\n";

        private static string Or(char x, char y) => $"OR {x} {y}\n";

        private static string Not(char x, char y) => $"NOT {x} {y}\n";

        private static string Walk() => "WALK\n";

        private static string Run() => "RUN\n";

        private void WriteInstruction(Cpu cpu, string instruction)
        {
            foreach (var c in instruction)
            {
                cpu.WriteInput(c);
            }
        }
    }
}
