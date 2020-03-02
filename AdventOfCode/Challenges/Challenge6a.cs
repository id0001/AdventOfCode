using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge6a : IChallenge
	{
		public string Id => "6a";

		public async Task<string> RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge6.txt");

			var orbitalTree = new OrbitalTree(lines);

			int sum = orbitalTree.Sum(e => e.Depth);

			return sum.ToString();
		}
	}
}
