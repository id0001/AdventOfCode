using AdventOfCode2019.IntCode.Core;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges
{
	[Challenge(2)]
	public class Challenge02
	{
		private readonly IInputReader _inputReader;

		public Challenge02(IInputReader inputReader)
		{
			_inputReader = inputReader;
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			var program = await _inputReader.ReadLineAsync<long>(2, ',').ToArrayAsync();
			program[1] = 12;
			program[2] = 2;

			var cpu = new Cpu();
			cpu.SetProgram(program);
			return (await cpu.StartAsync()).ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			var program = await _inputReader.ReadLineAsync<long>(2, ',').ToArrayAsync();

			for (var noun = 0; noun < 100; noun++)
			{
				for (var verb = 0; verb < 100; verb++)
				{
					program[1] = noun;
					program[2] = verb;

					var cpu = new Cpu();
					cpu.SetProgram(program);
					var result = (int)await cpu.StartAsync();
					if (result == 19690720)
						return (100 * noun + verb).ToString();
				}
			}

			return string.Empty;
		}
	}
}
