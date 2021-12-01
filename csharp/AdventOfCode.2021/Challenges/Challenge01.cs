using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(1)]
    public class Challenge01
    {
        private readonly IInputReader inputReader;
        private int[] input;

        public Challenge01(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            input = await inputReader.ReadLinesAsync<int>(1).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int count = 0;
            int last = input[0];
            for(int i = 1; i < input.Length;i++)
            {
                count += input[i] > last ? 1 : 0;
                last = input[i];
            }

            return count.ToString();
        }

        [Part2]
        public string Part2()
        {
            int count = 0;
            int last = input[0] + input[1] + input[2];
            for(int i = 1; i < input.Length; i++)
            {
                if (i + 2 >= input.Length)
                    continue;

                int sum = input[i] + input[i + 1] + input[i + 2];
                count += sum > last ? 1 : 0;
                last = sum;
            }

            return count.ToString();
        }
    }
}
