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
        public string Part1() => Enumerable.Range(1, input.Length - 1).Aggregate(0, (a, i) => a += input[i] > input[i - 1] ? 1 : 0).ToString();


        [Part2]
        public string Part2() => Enumerable.Range(1, input.Length - 3).Aggregate(0, (a, i) => a += input[i] + input[i + 1] + input[i + 2] > input[i - 1] + input[i] + input[i + 1] ? 1 : 0).ToString();
    }
}
