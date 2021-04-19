using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(9)]
	public class Challenge09
	{
		private readonly IInputReader inputReader;
		private long[] program;

		public Challenge09(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			program = await inputReader.ReadLineAsync<long>(9, ',').ToArrayAsync();
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			long output = 0;
			var cpu = new Cpu();
			cpu.SetProgram(program);
			cpu.RegisterOutput(o => output = o);
			await cpu.StartAsync(1);
			return output.ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			long output = 0;
			var cpu = new Cpu();
			cpu.SetProgram(program);
			cpu.RegisterOutput(o => output = o);
			await cpu.StartAsync(2);
			return output.ToString();
		}
	}
}
