using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(13)]
	public class Challenge13
	{
		private readonly IInputReader inputReader;
		private long[] program;

		public Challenge13(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			program = await inputReader.ReadLineAsync<long>(13, ',').ToArrayAsync();
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			int[,] screenBuffer = new int[64, 64];
			var outputBuffer = new Queue<int>();

			int ballX = 0;
			int paddleX = 0;

			var cpu = new Cpu();
			cpu.SetProgram(program);

			cpu.RegisterOutput(o =>
			{
				outputBuffer.Enqueue((int)o);

				if (outputBuffer.Count == 3)
				{
					int x = outputBuffer.Dequeue();
					int y = outputBuffer.Dequeue();
					int id = outputBuffer.Dequeue();

					if (x == -1 && y == 0)
					{
						// Draw score
						screenBuffer[0, 0] = id;
					}
					else
					{
						// Draw tile
						screenBuffer[y + 2, x] = id;

						if (id == 4)
							ballX = x;

						if (id == 3)
							paddleX = x;
					}
				}
			});

			cpu.RegisterInput(() =>
			{
				if (ballX == paddleX)
					cpu.WriteInput(0);
				else if (ballX < paddleX)
					cpu.WriteInput(-1);
				else
					cpu.WriteInput(1);
			});

			await cpu.StartAsync();

			return screenBuffer.OfType<int>().Count(x => x == 2).ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			int[,] screenBuffer = new int[64, 64];
			var outputBuffer = new Queue<int>();

			int ballX = 0;
			int paddleX = 0;

			var cpu = new Cpu();
			program[0] = 2;
			cpu.SetProgram(program);

			cpu.RegisterOutput(o =>
			{
				outputBuffer.Enqueue((int)o);

				if (outputBuffer.Count == 3)
				{
					int x = outputBuffer.Dequeue();
					int y = outputBuffer.Dequeue();
					int id = outputBuffer.Dequeue();

					if (x == -1 && y == 0)
					{
						// Draw score
						screenBuffer[0, 0] = id;
					}
					else
					{
						// Draw tile
						screenBuffer[y + 2, x] = id;

						if (id == 4)
							ballX = x;

						if (id == 3)
							paddleX = x;
					}
				}
			});

			cpu.RegisterInput(() =>
			{
				if (ballX == paddleX)
					cpu.WriteInput(0);
				else if (ballX < paddleX)
					cpu.WriteInput(-1);
				else
					cpu.WriteInput(1);
			});

			await cpu.StartAsync();

			return screenBuffer[0,0].ToString();
		}
	}
}
