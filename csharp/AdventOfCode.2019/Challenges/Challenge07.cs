using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(7)]
    public class Challenge07
    {
        private readonly IInputReader inputReader;
        private long[] program;

        public Challenge07(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            program = await inputReader.ReadLineAsync<long>(7, ',').ToArrayAsync();
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var perms = Enumerable.Range(0, 5).Permutations(0, 4);

            int highest = int.MinValue;
            foreach (var permutation in perms)
            {
                int signal = 0;
                foreach (var phase in permutation)
                {
                    var cpu = new Cpu();
                    cpu.SetProgram(program);
                    cpu.RegisterOutput(o => signal = (int)o);
                    await cpu.StartAsync(phase, signal);
                }

                if (signal > highest)
                    highest = signal;
            }

            return highest.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var perms = Enumerable.Range(5, 9).Permutations(0, 4);

            int highest = int.MinValue;
            foreach (var permutation in perms)
            {
                var ampA = new Amp("A", program, permutation[0]);
                var ampB = new Amp("B", program, permutation[1]);
                var ampC = new Amp("C", program, permutation[2]);
                var ampD = new Amp("D", program, permutation[3]);
                var ampE = new Amp("E", program, permutation[4]);

                ampA.PipeTo(ampB);
                ampB.PipeTo(ampC);
                ampC.PipeTo(ampD);
                ampD.PipeTo(ampE);
                ampE.PipeTo(ampA);

                await Task.WhenAll(ampA.RunAsync(0), ampB.RunAsync(), ampC.RunAsync(), ampD.RunAsync(), ampE.RunAsync());

                int result = ampE.LastSignal;
                
                if (result > highest)
                    highest = result;
            }

            return highest.ToString();
        }

        private class Amp
        {
            private Amp pipeTo;

            public Amp(string id, long[] input, int phase)
            {
                Id = id;
                Cpu = new Cpu();
                Cpu.SetProgram(input);
                Cpu.WriteInput(phase);
            }

            public string Id { get; set; }

            public Cpu Cpu { get; }

            public int LastSignal { get; private set; }

            public void PipeTo(Amp amp) => pipeTo = amp;

            public Task RunAsync(params long[] input)
            {
                Cpu.RegisterOutput(o =>
                {
                    LastSignal = (int)o;
                    pipeTo.Cpu.WriteInput(o);
                });

                return Cpu.StartAsync(input);
            }
        }
    }
}
