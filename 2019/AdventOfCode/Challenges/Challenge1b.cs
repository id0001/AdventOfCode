using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge1b : IChallenge
	{
		public string Id => "1b";

		public async Task<string> RunAsync() => (await File.ReadAllLinesAsync("Assets/Challenge1.txt"))
			.Select(s => int.Parse(s))
			.Aggregate(0, (a, b) => a + CalculateFuelRequirement(b)).ToString();

		private int CalculateFuelRequirement(int mass)
		{
			if (mass == 0) return 0;

			return Math.Max(0, (mass / 3 - 2)) + CalculateFuelRequirement(Math.Max(0, (mass / 3 - 2)));
		}
	}
}
