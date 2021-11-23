using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(25)]
    public class Challenge25
    {
        private readonly IInputReader inputReader;
        private long[] data;

        public Challenge25(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<long>(25, ',').ToArrayAsync();
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var cpu = new Cpu();
            cpu.SetProgram(data);

            // F this, I bruteforced it by hand.
            string buffer = string.Empty;
            cpu.RegisterOutput(o =>
            {
                buffer += (char)o;
                Console.Write((char)o);

                if (buffer.EndsWith("Command?"))
                {
                    Console.WriteLine();
                    Console.Write("> ");
                    string cmd = Console.ReadLine();
                    foreach (var c in cmd)
                        cpu.WriteInput(c);

                    cpu.WriteInput('\n');
                    Console.Clear();
                    buffer = string.Empty;
                }
            });

            await cpu.StartAsync();

            return null;
        }

        [Part2]
        public string Part2()
        {
            return null;
        }
    }
}
