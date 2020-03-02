using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge13a : IChallenge
	{
		public string Id => "13a";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge13.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var arcade = new ArcadeCabinet(program);
			arcade.Run();

			return arcade.ScreenBuffer.OfType<int>().Count(c => c == 2).ToString();
		}
	}
}
