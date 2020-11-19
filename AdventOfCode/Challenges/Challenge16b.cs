using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge16b : IChallenge
	{
		private static readonly int[] BasePattern = new int[] { 0, 1, 0, -1 };

		public string Id => "16b";

		public async Task<string> RunAsync2()
		{
			int[] originalInput = (await File.ReadAllTextAsync("Assets/Challenge16.txt")).Select(s => int.Parse(s.ToString())).ToArray();

			int inputLength = originalInput.Length * 10000;
			int skip = 0; // int.Parse(string.Join("", originalInput.Take(7)));
			int sliceSize = 8;

			int[] input = new int[sliceSize];

			for (int i = 0; i < sliceSize; i++)
			{
				int expi = (skip + i) % originalInput.Length;
				input[i] = originalInput[expi];
			}

			// Loop over all phases
			for (int phase = 0; phase < 100; phase++)
			{
				var output = new int[sliceSize];

				// slice between skip and skip + sliceSize
				for (int y = skip; y < skip + sliceSize; y++)
				{
					// slice between skip and skip + sliceSize but start from y because every index under y is 0 in the pattern
					for (int x = y; x < skip + sliceSize; x++)
					{
						int patternIndex = FindPatternIndex(x, y);
						if (BasePattern[patternIndex] == 0) // if pattern value is 0 move to index with next pattern value.
						{
							x += (y + 1);
							patternIndex++;
							if (x >= skip + sliceSize)
								break;
						}

						output[y - skip] += input[x - skip] * BasePattern[patternIndex];
					}
				}

				input = output.Select(e => Math.Abs(e) % 10).ToArray();
			}

			string result = string.Join("", input);
			Console.WriteLine(result);

			return result;
		}

		public async Task<string> RunAsync()
		{
			int[] originalInput = (await File.ReadAllTextAsync("Assets/Challenge16.txt")).Select(s => int.Parse(s.ToString())).ToArray();

			int totalLength = originalInput.Length * 10000;
			int sliceSize = 8;
			int skip = int.Parse(string.Join("", originalInput.Take(7)));

			int[] input = new int[totalLength - skip];
			for (int i = 0; i < input.Length; i++)
			{
				input[i] = originalInput[(skip + i) % originalInput.Length];
			}

			// Loop over all phases
			for (int phase = 0; phase < 100; phase++)
			{
				var output = new int[input.Length];

				// slice between skip and skip + sliceSize
				for (int y = skip; y < skip + input.Length; y++)
				{
					// slice between skip and skip + sliceSize but start from y because every index under y is 0 in the pattern
					for (int x = y; x < skip + input.Length; x++)
					{
						int patternIndex = FindPatternIndex(x, y);
						if (BasePattern[patternIndex] == 0) // if pattern value is 0 move to index with next pattern value.
						{
							x += (y + 1);
							patternIndex++;
							if (x >= skip+input.Length)
								break;
						}

						output[y-skip] += input[x-skip] * BasePattern[patternIndex];
					}
				}

				input = output.Select(e => Math.Abs(e) % 10).ToArray();
			}

			string result = string.Join("", input.Take(sliceSize));
			Console.WriteLine(result);
			return result;
		}

		private int FindPatternIndex(int x, int y)
		{
			int totalLength = (y + 1) * BasePattern.Length; // [0+1]=(0,1,0,-1) [1+1]=(0,0,1,1,0,0,-1,-1) [2+1]=(0,0,0,1,1,1,0,0,0,-1,-1,-1)
			int pi = (x + 1) % totalLength; // shift pattern to left by 1: (x + 1). Modulo with length to get index on pattern.
			return (int)Math.Floor(pi / (y + 1d)); // divide by (y+1) to get the pattern value normalized to base pattern size.
		}
	}
}
