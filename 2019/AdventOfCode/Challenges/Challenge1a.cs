using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge1a : IChallenge
	{
		public string Id => "1a";

		public async Task<string> RunAsync() => (await File.ReadAllLinesAsync("Assets/Challenge1.txt"))
			.Select(s => int.Parse(s))
			.Aggregate(0, (a, b) => a + ((b / 3) - 2)).ToString();
	}
}
