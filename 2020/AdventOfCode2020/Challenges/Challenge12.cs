using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(12)]
	public class Challenge12
	{
		private readonly IInputReader inputReader;

		public Challenge12(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		private async Task SetupAsync()
		{

		}

		[Part1]
		public async Task<string> Part1Async()
		{
			int x = 0;
			int y = 0;
			int a = 0; // east

			await foreach (string line in inputReader.ReadLinesAsync(12))
			{
				char action = line[0];
				int value = int.Parse(line.Substring(1));

				switch (action)
				{
					case 'N':
						y -= value;
						break;
					case 'E':
						x += value;
						break;
					case 'S':
						y += value;
						break;
					case 'W':
						x -= value;
						break;
					case 'L':
						a = MathEx.Mod(a - value, 360);
						break;
					case 'R':
						a = MathEx.Mod(a + value, 360);
						break;
					case 'F':
						double r = MathEx.ToRadians(a);
						x += (int)Math.Cos(r) * value;
						y += (int)Math.Sin(r) * value;
						break;
				}
			}

			return (Math.Abs(x) + Math.Abs(y)).ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			Point2 s = new Point2(0, 0);
			Point2 w = new Point2(10, -1);

			await foreach (string line in inputReader.ReadLinesAsync(12))
			{
				char action = line[0];
				int value = int.Parse(line.Substring(1));

				switch (action)
				{
					case 'N':
						w.Y -= value;
						break;
					case 'E':
						w.X += value;
						break;
					case 'S':
						w.Y += value;
						break;
					case 'W':
						w.X -= value;
						break;
					case 'L':
						w = Point2.Turn(w, s, -MathEx.ToRadians(value));
						break;
					case 'R':
						w = Point2.Turn(w, s, MathEx.ToRadians(value));
						break;
					case 'F':
						double r = Math.Atan2(w.Y - s.Y, w.X - s.X);
						double dist = Point2.Distance(s, w);

						Point2 d = new Point2((int)Math.Round(Math.Cos(r) * dist), (int)Math.Round(Math.Sin(r) * dist));
						s += d * value;
						w += d * value;
						break;
				}
			}

			return (Math.Abs(s.X) + Math.Abs(s.Y)).ToString();
		}
	}
}
