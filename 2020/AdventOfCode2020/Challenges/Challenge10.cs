using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(10)]
	public class Challenge10
	{
		private readonly IInputReader inputReader;
		private int[] input;

		public Challenge10(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			input = await inputReader.ReadLinesAsync<int>(10).OrderBy(e => e).ToArrayAsync();
		}

		[Part1]
		public string Part1()
		{
			int[] differences = new[] { 0, 0, 1 };

			int prev = 0;
			foreach (int adapter in input)
			{
				differences[adapter - prev - 1]++;
				prev = adapter;
			}

			return (differences[0] * differences[2]).ToString();
		}

		[Part2]
		public string Part2()
		{
			int max = input[input.Length - 1] + 3;
			int[] set = input.Prepend(0).Append(max).ToArray();
			int[] groupCount = new[] { 0, 0, 0 };

			int c = 0;
			for (int i = 0; i < set.Length; i++)
			{
				if (i == 0 || i == set.Length - 1 || set[i] - set[i - 1] == 3 || set[i + 1] - set[i - 1] > 3)
				{
					if (c > 0)
						groupCount[c - 1]++;

					c = 0;
					continue;
				}

				c++;
			}

			if (c > 0)
				groupCount[c - 1]++;

			long count = (long)Math.Pow(2, groupCount[0]) * (long)Math.Pow(4, groupCount[1]) * (long)Math.Pow(7, groupCount[2]);
			return count.ToString();
		}
	}
}
