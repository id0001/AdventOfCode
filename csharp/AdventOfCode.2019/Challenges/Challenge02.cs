using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(2)]
	public class Challenge02
	{
		private readonly IInputReader inputReader;
		private long[] program;

		public Challenge02(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			program = await inputReader.ReadLineAsync<long>(2, ',').ToArrayAsync();
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			program[1] = 12;
			program[2] = 2;

			var cpu = new Cpu();
			cpu.SetProgram(program);
			return (await cpu.RunAsync()).ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			for (int noun = 0; noun < 100; noun++)
			{
				for (int verb = 0; verb < 100; verb++)
				{
					program[1] = noun;
					program[2] = verb;

					var cpu = new Cpu();
					cpu.SetProgram(program);
					int result = (int)await cpu.RunAsync();
					if (result == 19690720)
						return (100 * noun + verb).ToString();
				}
			}

			return string.Empty;
		}
	}
}
