using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(3)]
	public class Challenge03
	{
		private readonly IInputReader inputReader;

		public Challenge03(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			var lines = await inputReader.ReadLinesAsync(3).ToArrayAsync();

			var wire1 = GetWire(lines[0].Split(','));
			var wire2 = GetWire(lines[1].Split(','));

			var intersection = wire1.Keys.Intersect(wire2.Keys);

			return intersection.Select(p => ManhattanDistance(p)).Min().ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			var lines = await inputReader.ReadLinesAsync(3).ToArrayAsync();

			var wire1 = GetWire(lines[0].Split(','));
			var wire2 = GetWire(lines[1].Split(','));

			var intersections = wire1.Keys.Intersect(wire2.Keys);

			return intersections.Select(p => wire1[p] + wire2[p]).Min().ToString();
		}

		private int ManhattanDistance(Point2 p) => Math.Abs(p.X) + Math.Abs(p.Y);

		private IDictionary<Point2, int> GetWire(string[] moves)
		{
			var dict = new Dictionary<Point2, int>();

			var current = Point2.Zero;
			int steps = 0;
			foreach (var move in moves)
			{
				char dir = move[0];
				int amount = int.Parse(move.Substring(1));

				for (int i = 0; i < amount; i++)
				{
					switch (dir)
					{
						case 'U':
							current += new Point2(0, -1);
							break;
						case 'R':
							current += new Point2(1, 0);
							break;
						case 'D':
							current += new Point2(0, 1);
							break;
						case 'L':
							current += new Point2(-1, 0);
							break;
						default:
							throw new NotImplementedException();
					}

					steps++;
					if (!dict.ContainsKey(current))
						dict.Add(current, steps);
				}
			}

			return dict;
		}
	}
}
