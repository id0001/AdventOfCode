using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge2a : IChallenge
	{
		public string Id => "2a";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge2.txt")).Split(',').Select(s => long.Parse(s)).ToArray();
			program[1] = 12;
			program[2] = 2;

			var computer = new SimpleRunner(program);
			return computer.Execute().ToString();
		}
	}
}
