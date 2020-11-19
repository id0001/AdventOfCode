using AdventOfCode.IntCode.Devices;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge13b : IChallenge
	{
		public string Id => "13b";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge13.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			program[0] = 2;
			var arcade = new ArcadeCabinet(program);
			arcade.Run();

			return arcade.ScreenBuffer[0, 0].ToString();
		}
	}
}
