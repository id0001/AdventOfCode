
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
			var targetType = typeof(IChallenge);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => targetType.IsAssignableFrom(p));

			services.TryAddSingleton<IChallengeLocator, ChallengeLocator>();

			foreach (var type in types)
			{
				services.TryAddTransient(typeof(IChallenge), type);
			}

			return services;
		}
	}
}
