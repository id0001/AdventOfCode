
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	public class ChallengeLocator : IChallengeLocator
	{
		private readonly IServiceProvider serviceProvider;
		private readonly IDictionary<int, Type> challengeTypes;

		public ChallengeLocator(IServiceProvider serviceProvider, IDictionary<int, Type> challengeTypes)
		{
			this.serviceProvider = serviceProvider;
			this.challengeTypes = challengeTypes;
		}

		public async Task<object> GetMostRecentChallengeAsync()
		{
			if (challengeTypes.Count == 0)
				throw new InvalidOperationException("There are no challenges");

			var key = challengeTypes.Keys.OrderBy(k => k).Last();
			return await GetChallengeAsync(key);
		}

		public async Task<object> GetChallengeAsync(int day)
		{
			if (!challengeTypes.TryGetValue(day, out Type challengeType))
				throw new KeyNotFoundException("Challenge not found");

			var challenge = serviceProvider.GetRequiredService(challengeType);
			await SetupChallengeAsync(challenge);

			return challenge;
		}

		private async Task SetupChallengeAsync(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var setupMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<SetupAttribute>() != null);
			if(setupMethod != null)
			{
				var result = setupMethod.Invoke(challenge, null);
				if(result != null && result is Task t)
				{
					await t;
				}
			}
		}
	}
}
