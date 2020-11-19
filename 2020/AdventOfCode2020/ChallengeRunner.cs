
using AdventOfCodeLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	public class ChallengeRunner : BaseChallengeRunner
	{
		protected override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);
		}

		public override async Task RunMostRecentChallengeAsync()
		{
			var challenge = ChallengeLocator.GetMostRecentChallenge();

			var result = await challenge.RunAsync();

			Console.WriteLine();
			Console.WriteLine(new string('=', 40));
			Console.WriteLine("The solution is:");
			Console.WriteLine(result);
		}
	}
}
