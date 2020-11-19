using System;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			var challengeRunner = new ChallengeRunner();
			await challengeRunner.RunMostRecentChallengeAsync();
			Console.ReadKey();
		}
	}
}
