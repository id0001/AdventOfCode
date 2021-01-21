using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(5)]
	public class Challenge05
	{
		private readonly IInputReader inputReader;
		private long[] program;

		public Challenge05(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			program = await inputReader.ReadLineAsync<long>(5, ',').ToArrayAsync();
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			long result = -1;
			var cpu = new Cpu();
			cpu.RegisterOutput(o => result = o);
			cpu.SetProgram(program);
			await cpu.RunAsync(1);

			return result.ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			long result = -1;
			var cpu = new Cpu();
			cpu.RegisterOutput(o => result = o);
			cpu.SetProgram(program);
			await cpu.RunAsync(5);

			return result.ToString();
		}
	}
}
