
using AdventOfCodeLib;
using AdventOfCodeLib.Extensions.DependencyInjection;
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
			var result1 = await RunPart1Async(await ChallengeLocator.GetMostRecentChallengeAsync());
			if (result1 != null)
			{
				Console.WriteLine(new string('=', 40));
				Console.WriteLine("The solution for part 1 is:");
				Console.WriteLine(result1);
				Console.WriteLine();
			}

			var result2 = await RunPart2Async(await ChallengeLocator.GetMostRecentChallengeAsync());
			if (result2 != null)
			{
				Console.WriteLine(new string('=', 40));
				Console.WriteLine("The solution for part 2 is:");
				Console.WriteLine(result2);
				Console.WriteLine();
			}
		}
	}
}
