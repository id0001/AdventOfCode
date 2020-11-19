using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public interface IChallenge
	{
		string Id { get; }

		Task<string> RunAsync();
	}
}
