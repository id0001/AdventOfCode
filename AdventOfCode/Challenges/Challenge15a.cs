using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge15a : IChallenge
	{
		public string Id => "15a";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge15.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var droid = new RepairDroid(program);

			droid.MapLayout();

			int moves = droid.CalculateShortestPathToDefect();

			return moves.ToString();
		}
	}
}
