using AdventOfCodeLib.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	public abstract class BaseChallengeRunner
	{
		private IChallengeLocator challengeLocator;

		public BaseChallengeRunner()
		{
			IServiceCollection services = new ServiceCollection();
			ConfigureServices(services);

			ServiceProvider = services.BuildServiceProvider();
		}

		protected IServiceProvider ServiceProvider { get; }

		protected IChallengeLocator ChallengeLocator => challengeLocator ?? (challengeLocator = ServiceProvider.GetRequiredService<IChallengeLocator>());

		public virtual Task RunMostRecentChallengeAsync()
		{
			return Task.CompletedTask;
		}

		public virtual Task RunTestsAsync()
		{
			return Task.CompletedTask;
		}

		protected virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddChallenges();
		}
	}
}
