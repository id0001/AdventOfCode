using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(3)]
	public class Challenge3
	{
		private readonly IChallengeInput challengeInput;

		private string[] input;

		public Challenge3(IChallengeInput challengeInput)
		{
			this.challengeInput = challengeInput;
		}

		[Setup]
		public async Task SetupAsync()
		{
			input = await challengeInput.ReadLinesAsync(3).ToArrayAsync();
		}

		[Part1]
		public string Part1()
		{
			return TraverseSlope(3, 1).ToString();
		}

		[Part2]
		public string Part2()
		{
			return (TraverseSlope(1, 1) * TraverseSlope(3, 1) * TraverseSlope(5, 1) * TraverseSlope(7, 1) * TraverseSlope(1, 2)).ToString();
		}

		private long TraverseSlope(int dx, int dy)
		{
			int x = 0;
			int y = 0;

			int treeCount = 0;
			while (y < input.Length)
			{
				if (input[y][x] == '#')
					treeCount++;

				x = (x+dx) % input[0].Length;
				y += dy;
			}

			return treeCount;
		}
	}
}
