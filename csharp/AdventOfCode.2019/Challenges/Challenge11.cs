using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(11)]
	public class Challenge11
	{
		private readonly IInputReader inputReader;
		private long[] program;

		public Challenge11(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			program = await inputReader.ReadLineAsync<long>(11, ',').ToArrayAsync();
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			var locationHistory = new Dictionary<Point2, long>();
			var currentLocation = new Point2();
			locationHistory.Add(currentLocation, 0);
			int direction = 0;
			int action = 0;

			var cpu = new Cpu();
			cpu.SetProgram(program);
			cpu.RegisterOutput(o =>
			{
				switch (action)
				{
					case 0: // Paint location
						locationHistory[currentLocation] = o;
						break;
					case 1: // Change direction and move
						direction = o == 0 ? MathEx.Mod(direction - 1, 4) : MathEx.Mod(direction + 1, 4);
						currentLocation = NextLocation(currentLocation, direction);
						break;
					default:
						throw new NotSupportedException();
				}

				action = (action + 1) % 2;
			});

			cpu.RegisterInput(() =>
			{
				locationHistory.TryGetValue(currentLocation, out long color);
				cpu.WriteInput(color);
			});

			await cpu.StartAsync();

			return locationHistory.Count.ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			var locationHistory = new Dictionary<Point2, long>();
			var currentLocation = new Point2();
			locationHistory.Add(currentLocation, 1);
			int direction = 0;
			int action = 0;

			var cpu = new Cpu();
			cpu.SetProgram(program);
			cpu.RegisterOutput(o =>
			{
				switch (action)
				{
					case 0: // Paint location
						locationHistory[currentLocation] = o;
						break;
					case 1: // Change direction and move
						direction = o == 0 ? MathEx.Mod(direction - 1, 4) : MathEx.Mod(direction + 1, 4);
						currentLocation = NextLocation(currentLocation, direction);
						break;
					default:
						throw new NotSupportedException();
				}

				action = (action + 1) % 2;
			});

			cpu.RegisterInput(() =>
			{
				locationHistory.TryGetValue(currentLocation, out long color);
				cpu.WriteInput(color);
			});

			await cpu.StartAsync();



			return DrawHull(locationHistory);
		}

		private Point2 NextLocation(Point2 currentLocation, int direction)
		{
			return currentLocation + direction switch
			{
				0 => new Point2(0, -1),
				1 => new Point2(1, 0),
				2 => new Point2(0, 1),
				3 => new Point2(-1, 0),
				_ => throw new NotSupportedException()
			};
		}

		private static string DrawHull(Dictionary<Point2, long> locations)
		{
			var keys = locations.Keys;

			var leftMost = keys.Min(e => e.X);
			var rightMost = keys.Max(e => e.X);
			var topMost = keys.Min(e => e.Y);
			var bottomMost = keys.Max(e => e.Y);

			var rows = bottomMost - topMost + 1;
			var cols = rightMost - leftMost + 1;

			var sb = new StringBuilder();
			sb.AppendLine();
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					if (locations.TryGetValue(new Point2(x + leftMost, y + topMost), out long pvalue))
					{
						sb.Append(pvalue == 1 ? '#' : '.');
					}
					else
					{
						sb.Append('.');
					}
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
