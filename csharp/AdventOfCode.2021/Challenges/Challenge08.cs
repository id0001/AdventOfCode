using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(8)]
    public class Challenge08
    {
        private readonly IInputReader inputReader;
        private IOPair[] data;

        public Challenge08(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLinesAsync(8).Select(line =>
            {
                string[] ioSplit = line.Split(new[] { '|' }, StringSplitOptions.TrimEntries);
                return new IOPair(new Input(ioSplit[0].Split(' ')), new Output(ioSplit[1].Split(' ')));
            }).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int[] lengths = new[] { 2, 3, 4, 7 };
            int count = data.SelectMany(x => x.Output.Values).Where(x => lengths.Contains(x.Length)).Count();

            return count.ToString();
        }

        [Part2]
        public string Part2()
        {
            return data.Select(FindNumberForSignal).Sum().ToString();
        }

        private int FindNumberForSignal(IOPair io)
        {
            var mapping = MapSignalsToNumbers(io.Input);
            int value = 0;
            for (int i = 0; i < io.Output.Values.Length; i++)
            {
                var chars = mapping.SingleOrDefault(x => x.SetEquals(io.Output.Values[i]));

                int num = Array.IndexOf(mapping, mapping.Single(x => x.SetEquals(io.Output.Values[i])));
                value += num * (int)Math.Pow(10, io.Output.Values.Length - i - 1);
            }

            return value;
        }

        private ISet<char>[] MapSignalsToNumbers(Input input)
        {
            ISet<char>[] result = new HashSet<char>[10];

            var values = input.Values.OrderBy(x => x.Length).ToArray();
            result[1] = new HashSet<char>(values[0]);
            result[7] = new HashSet<char>(values[1]);
            result[4] = new HashSet<char>(values[2]);
            result[8] = new HashSet<char>(values[^1]);

            ISet<char> ab = result[1];
            ISet<char> d = result[7].Except(result[1]).ToHashSet();
            ISet<char> ef = result[4].Except(result[1]).ToHashSet();
            ISet<char> gc = result[8].Except(result[4].Union(d)).ToHashSet();

            ISet<char> c = values.Where(x => x.Length == 6).Select(x => x.Except(result[4].Union(d))).Single(x => x.Count() == 1).ToHashSet();
            ISet<char> g = gc.Except(c).ToHashSet();
            ISet<char> e = values.Where(x => x.Length == 6).Select(x => x.Except(result[7].Union(gc))).Single(x => x.Count() == 1).ToHashSet();
            ISet<char> f = ef.Except(e).ToHashSet();

            ISet<char> a = values.Where(x => x.Length == 5).Select(x => x.Except(gc.Union(f).Union(d))).Single(x => x.Count() == 1).ToHashSet();
            ISet<char> b = ab.Except(a).ToHashSet();

            result[0] = result[8].Except(f).ToHashSet();
            result[2] = result[8].Except(e).Except(b).ToHashSet();
            result[3] = result[8].Except(e).Except(g).ToHashSet();
            result[5] = result[8].Except(a).Except(g).ToHashSet();
            result[6] = result[8].Except(a).ToHashSet();
            result[9] = result[8].Except(g).ToHashSet();

            return result;
        }

        private record Input(string[] Values);
        private record Output(string[] Values);
        private record IOPair(Input Input, Output Output);
    }
}
