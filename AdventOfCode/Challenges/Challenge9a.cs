using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge9a : IChallenge
	{
		public string Id => "9a";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge9.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var runner = new SimpleRunner(program);
			runner.In.Write(1);
			runner.Execute();

			return runner.Out.Read().ToString();

		}
	}
}
