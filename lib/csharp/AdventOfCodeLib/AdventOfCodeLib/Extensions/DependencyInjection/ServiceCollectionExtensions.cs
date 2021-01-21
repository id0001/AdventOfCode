
using AdventOfCodeLib.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCodeLib.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddChallenges(this IServiceCollection services, IChallengeTypeProvider challengeTypeProvider)
		{
			foreach (var challengeType in challengeTypeProvider.Values)
			{
				services.TryAddTransient(challengeType);
			}

			services.TryAddSingleton(challengeTypeProvider);

			return services;
		}

		public static IServiceCollection AddChallengeInput(this IServiceCollection services, Action<InputReaderOptions> configure)
		{
			services.AddOptions<InputReaderOptions>().Configure(configure);
			services.TryAddSingleton<IInputReader, InputReader>();
			return services;
		}

		public static IServiceCollection AddChallengeHost(this IServiceCollection services, IChallengeTypeProvider challengeTypeProvider)
		{
			services.AddHostedService<ChallengeHost>();
			services.AddChallenges(challengeTypeProvider);
			return services;
		}
	}
}
