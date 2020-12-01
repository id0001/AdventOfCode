
using AdventOfCodeLib;
using AdventOfCodeLib.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	public class ChallengeRunner : BaseChallengeRunner
	{
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddChallengeInput(options =>
			{
				options.InputFolder = "Inputs";
			});
		}

		public override async Task RunMostRecentChallengeAsync()
		{
			var challenge = await ChallengeLocator.GetMostRecentChallengeAsync();

			var result1 = await RunPart1Async(challenge);
			if (result1 != null)
			{
				Console.WriteLine();
				Console.WriteLine(new string('=', 40));
				Console.WriteLine("The solution for part 1 is:");
				Console.WriteLine(result1);
			}

			var result2 = await RunPart2Async(challenge);
			if (result2 != null)
			{
				Console.WriteLine();
				Console.WriteLine(new string('=', 40));
				Console.WriteLine("The solution for part 2 is:");
				Console.WriteLine(result2);
			}
		}
	}
}
