using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(7)]
    public class Challenge07
    {
        private readonly IInputReader inputReader;
        private int[] data;

        public Challenge07(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<int>(7, ',').ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int min = data.Min();
            int max = data.Max();

            int lowest = int.MaxValue;
            for (int pos = min; pos < max; pos++)
            {
                int test = data.Select(x => Math.Abs(x - pos)).Sum();
                if (test < lowest)
                    lowest = test;
            }

            return lowest.ToString();
        }

        [Part2]
        public string Part2()
        {
            int min = data.Min();
            int max = data.Max();

            int lowest = int.MaxValue;
            for (int pos = min; pos < max; pos++)
            {
                int test = data.Select(x => MathEx.TriangularNumber(Math.Abs(x - pos))).Sum();
                if (test < lowest)
                    lowest = test;
            }

            return lowest.ToString();
        }
    }
}
