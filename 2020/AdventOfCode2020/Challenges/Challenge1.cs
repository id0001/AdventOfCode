using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(1)]
	public class Challenge1
	{
		private readonly IChallengeInput challengeInput;

		private ISet<int> input;

		public Challenge1(IChallengeInput challengeInput)
		{
			this.challengeInput = challengeInput;
		}

		[Setup]
		public async Task SetupAsync()
		{
			input = await challengeInput.ReadIntegersAsync(1).ToHashSetAsync();
		}

		[Part1]
		public string Part1()
		{
			foreach (var n in input)
			{
				int remaining = 2020 - n;
				if (input.Contains(remaining))
				{
					return (n * remaining).ToString();
				}
			}

			return "-1";
		}

		[Part2]
		public string Part2()
		{
			int[] n = input.ToArray();
			for (int y = 0; y < n.Length - 1; y++)
			{
				for (int x = y + 1; x < n.Length; x++)
				{
					int remaining = 2020 - n[y] - n[x];
					if (input.Contains(remaining))
					{
						return (n[y] * n[x] * remaining).ToString();
					}
				}
			}

			return "-1";
		}
	}
}
