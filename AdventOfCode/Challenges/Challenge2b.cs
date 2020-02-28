using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge2b : IChallenge
	{
		public string Id => "2b";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge2.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			for (int noun = 0; noun < 100; noun++)
			{
				for (int verb = 0; verb < 100; verb++)
				{
					program[1] = noun;
					program[2] = verb;
					var computer = new SimpleRunner(program);
					int result = (int)computer.Execute();
					if (result == 19690720)
					{
						return (100 * noun + verb).ToString();
					}
				}
			}

			return string.Empty;
		}
	}
}
