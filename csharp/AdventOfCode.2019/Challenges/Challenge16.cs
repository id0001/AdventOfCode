using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(16)]
    public class Challenge16
    {
        private static readonly int[] BasePattern = new int[] { 0, 1, 0, -1 };

        private readonly IInputReader inputReader;
        private int[] originalInput;

        public Challenge16(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            originalInput = await inputReader.ReadLineAsync(16).Select(x => int.Parse(x.ToString())).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            for (int phase = 0; phase < 100; phase++)
            {
                var newInput = new int[originalInput.Length];
                for (int j = 0; j < newInput.Length; j++)
                {
                    for (int i = 0; i < originalInput.Length; i++)
                    {
                        newInput[j] += originalInput[i] * GetPatternValueAtIndex(j, i);
                    }
                }

                for (int i = 0; i < originalInput.Length; i++)
                {
                    originalInput[i] = Math.Abs(newInput[i]) % 10;
                }
            }

            return string.Join("", originalInput.Take(8));
        }

        [Part2]
        public string Part2()
        {
            int inputCount = originalInput.Length * 10000;
            int offset = int.Parse(string.Join("", originalInput.Take(7)));

            // Skip from offset
            int[] input = new int[inputCount - offset];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = GetInput(originalInput, i + offset);
            }

            for (int phase = 0; phase < 100; phase++)
            {
                // Looping in reverse makes this O(n) instead of o(n^2)
                int sum = 0;
                for(int j = input.Length-1; j >= 0; j--)
                {
                    sum += input[j];
                    input[j] = sum % 10;
                }
            }

            return string.Join("", input.Take(8));
        }

        private static int GetInput(int[] input, int index) => input[index % input.Length];

        private int GetPatternValueAtIndex(int outputIndex, int inputIndex)
        {
            int patternSize = (outputIndex + 1) * BasePattern.Length;
            int pi = (inputIndex + 1) % patternSize;
            int bpi = (int)Math.Floor(pi / (outputIndex + 1d));
            return BasePattern[bpi];
        }
    }
}
