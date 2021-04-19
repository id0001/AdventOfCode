using AdventOfCode.Lib;
using System.Collections.Generic;

namespace AdventOfCode2020.Challenges
{
	[Challenge(15)]
	public class Challenge15
	{
		private static readonly int[] StartingNumbers = new int[] { 6, 13, 1, 15, 2, 0 };

		[Part1]
		public string Part1()
		{
			return RunGame(StartingNumbers, 2020).ToString();
		}

		[Part2]
		public string Part2()
		{
			return RunGame(StartingNumbers, 30000000).ToString();
		}

		private static int RunGame(int[] startingNumbers, int turns)
		{
			var lookup = new Dictionary<int, int>();

			for (int i = 0; i < startingNumbers.Length - 1; i++)
				lookup.Add(startingNumbers[i], i);

			int last = 0;

			for (int i = startingNumbers.Length; i < turns; i++)
			{
				if (!lookup.ContainsKey(last))
				{
					lookup.Add(last, i - 1);
					last = 0;
				}
				else
				{
					int diff = (i - 1) - lookup[last];
					lookup[last] = i - 1;
					last = diff;
				}
			}

			return last;
		}
	}
}
