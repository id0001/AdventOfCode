using AdventOfCodeLib.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	public abstract class BaseChallengeRunner
	{
		private IChallengeLocator challengeLocator;

		public BaseChallengeRunner()
		{
			IServiceCollection services = new ServiceCollection();
			ConfigureServicesInternal(services);

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
		}

		protected async Task<string> RunPart1Async(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part1Attribute>() != null);
			if (runMethod != null)
			{
				var result = runMethod.Invoke(challenge, null);
				if (result != null && result is Task<string> t)
				{
					return await t;
				}

				return (string)result;
			}

			return null;
		}

		protected async Task<string> RunPart2Async(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part2Attribute>() != null);
			if (runMethod != null)
			{
				var result = runMethod.Invoke(challenge, null);
				if (result != null && result is Task<string> t)
				{
					return await t;
				}

				return (string)result;
			}

			return null;
		}

		private void ConfigureServicesInternal(IServiceCollection services)
		{
			services.AddChallenges();
			ConfigureServices(services);
		}
	}
}
