using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	public interface IChallenge
	{
		public int Day { get; }

		public Part Part { get; }

		Task<string> RunAsync();
	}
}
