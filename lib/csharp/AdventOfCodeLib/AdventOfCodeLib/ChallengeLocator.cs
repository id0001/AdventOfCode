
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeLib
{
	public class ChallengeLocator : IChallengeLocator
	{
		private readonly IDictionary<(int, Part), IChallenge> challengeCache;

		public ChallengeLocator(IServiceProvider serviceProvider)
		{
			var challenges = serviceProvider.GetServices<IChallenge>();
			challengeCache = (from c in challenges
							  select c).ToDictionary(kv => (kv.Day, kv.Part), kv => kv);
		}

		public IChallenge GetChallenge(int day, Part part)
		{
			if (!challengeCache.TryGetValue((day, part), out var challenge))
				throw new KeyNotFoundException("The challenge for the given day and part was not found.");

			return challenge;
		}

		public IEnumerable<IChallenge> GetChallenges() => challengeCache.Values.AsEnumerable();

		public IChallenge GetMostRecentChallenge()
		{
			if (challengeCache.Count == 0)
				throw new InvalidOperationException("There are no challenges");

			var key = challengeCache.Keys.OrderBy(k => k.Item1).ThenBy(k => k.Item2).Last();
			return challengeCache[key];
		}
	}
}
