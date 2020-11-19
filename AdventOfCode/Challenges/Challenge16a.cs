using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge16a : IChallenge
	{
		private static readonly int[] BasePattern = new int[] { 0, 1, 0, -1 };

		public string Id => "16a";

		public async Task<string> RunAsync()
		{
			int[] input = (await File.ReadAllTextAsync("Assets/Challenge16.txt")).Select(s => int.Parse(s.ToString())).ToArray();

			for (int phase = 0; phase < 100; phase++)
			{
				var newInput = new int[input.Length];
				for (int j = 0; j < newInput.Length; j++)
				{
					for (int i = 0; i < input.Length; i++)
					{
						newInput[j] += input[i] * GetPatternValueAtIndex(j, i);
					}
				}

				for (int i = 0; i < input.Length; i++)
				{
					input[i] = Math.Abs(newInput[i]) % 10;
				}
			}

			Console.WriteLine(string.Join("", input));

			string result = string.Join("", input.Take(8));
			return result;
		}

		private int GetPatternValueAtIndex(int outputIndex, int inputIndex)
		{
			int patternSize = (outputIndex + 1) * BasePattern.Length;
			int pi = (inputIndex + 1) % patternSize;
			int bpi = (int)Math.Floor(pi / (outputIndex + 1d));
			return BasePattern[bpi];
		}
	}
}
