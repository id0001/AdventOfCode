
using System.Collections.Generic;

namespace AdventOfCodeLib
{
	public interface IChallengeLocator
	{
		IChallenge GetChallenge(int day, Part part);

		IChallenge GetMostRecentChallenge();

		IEnumerable<IChallenge> GetChallenges();
	}
}
