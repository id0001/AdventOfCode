using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(8)]
	public class Challenge08
	{
		private readonly IInputReader inputReader;
		private int[] rawInput;

		public Challenge08(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			rawInput = await inputReader.ReadLineAsync(8).Select(Convert.ToInt32).ToArrayAsync();
		}

		[Part1]
		public string Part1()
		{
			int width = 25;
			int height = 6;
			int ppl = width * height;
			int layers = rawInput.Length / ppl;

			var segments = Enumerable.Range(0, layers).Select(i => new ArraySegment<int>(rawInput, i * ppl, ppl)).ToArray();
			var leastZeros = segments.OrderBy(s => s.Count(x => x == 0)).First();

			int c1 = 0;
			int c2 = 0;

			foreach (int el in leastZeros)
			{
				c1 += el == 1 ? 1 : 0;
				c2 += el == 2 ? 1 : 0;
			}

			return (c1 * c2).ToString();
		}

		[Part2]
		public string Part2()
		{
			int width = 25;
			int height = 6;
			int ppl = width * height;
			int layers = rawInput.Length / ppl;

			var segments = Enumerable.Range(0, layers).Select(i => new ArraySegment<int>(rawInput, i * ppl, ppl)).ToArray();

			var image = Enumerable.Range(0, ppl).Select(i =>
			{
				for (int si = 0; si < segments.Length; si++)
				{
					if (segments[si][i] != 2)
						return segments[si][i];
				}

				return -1;
			}).ToArray();

			var sb = new StringBuilder();
			sb.AppendLine();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					sb.Append(image[(y * width) + x] == 1 ? '#' : ' ');
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
