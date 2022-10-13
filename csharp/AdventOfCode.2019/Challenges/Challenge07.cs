using AdventOfCode.Lib;
using AdventOfCode2019.IntCode.Core;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2019.Challenges
{
    [Challenge(7)]
    public class Challenge07
    {
        private readonly IInputReader _inputReader;

        public Challenge07(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var program = await _inputReader.ReadLineAsync<long>(7, ',').ToArrayAsync();
            var perms = Combinatorics.GenerateAllPermutations(5);

            var highest = int.MinValue;
            foreach (var permutation in perms)
            {
                var signal = 0;
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
            var program = await _inputReader.ReadLineAsync<long>(7, ',').ToArrayAsync();
            var perms = Enumerable.Range(5, 5).Permutations();

            var highest = int.MinValue;
            foreach (var permutation in perms)
            {
                var ampA = new Amp(program, permutation[0]);
                var ampB = new Amp(program, permutation[1]);
                var ampC = new Amp(program, permutation[2]);
                var ampD = new Amp(program, permutation[3]);
                var ampE = new Amp(program, permutation[4]);

                ampA.PipeTo(ampB);
                ampB.PipeTo(ampC);
                ampC.PipeTo(ampD);
                ampD.PipeTo(ampE);
                ampE.PipeTo(ampA);

                await Task.WhenAll(ampA.RunAsync(0), ampB.RunAsync(), ampC.RunAsync(), ampD.RunAsync(), ampE.RunAsync());

                var result = ampE.LastSignal;
                
                if (result > highest)
                    highest = result;
            }

            return highest.ToString();
        }

        private class Amp
        {
            private readonly Cpu _cpu;
            private Amp? _pipeTo;

            public Amp(long[] input, int phase)
            {
                _cpu = new Cpu();
                _cpu.SetProgram(input);
                _cpu.WriteInput(phase);
            }
            
            public int LastSignal { get; private set; }

            public void PipeTo(Amp amp) => _pipeTo = amp;

            public Task RunAsync(params long[] input)
            {
                _cpu.RegisterOutput(o =>
                {
                    LastSignal = (int)o;
                    _pipeTo?._cpu.WriteInput(o);
                });

                return _cpu.StartAsync(input);
            }
        }
    }
}
