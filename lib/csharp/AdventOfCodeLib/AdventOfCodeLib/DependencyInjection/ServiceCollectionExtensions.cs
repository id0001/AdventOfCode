
using AdventOfCodeLib.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCodeLib.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddChallenges(this IServiceCollection services)
		{
			var types = (from a in AppDomain.CurrentDomain.GetAssemblies()
						 from t in a.GetTypes()
						 let c = t.GetCustomAttribute<ChallengeAttribute>()
						 where c != null
						 select (c, t)).ToDictionary(kv => kv.c.Day, kv => kv.t);

			foreach(var challengeType in types.Values)
			{
				services.TryAddTransient(challengeType);
			}

			services.TryAddSingleton<IChallengeLocator>(sp => new ChallengeLocator(sp, types));

			return services;
		}

		public static IServiceCollection AddChallengeInput(this IServiceCollection services, Action<ChallengeInputOptions> configure)
		{
			services.AddOptions<ChallengeInputOptions>().Configure(configure);
			services.TryAddSingleton<IChallengeInput, ChallengeInput>();
			return services;
		}
	}
}
