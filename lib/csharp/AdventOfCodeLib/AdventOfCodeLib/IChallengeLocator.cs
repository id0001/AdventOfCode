
using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	public interface IChallengeLocator
	{
		Task<object> GetMostRecentChallengeAsync();

		Task<object> GetChallengeAsync(int day);
	}
}
