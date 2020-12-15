using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(15)]
	public class Challenge15
	{
		private int[] startingNumbers = new int[] { 6, 13, 1, 15, 2, 0 };
		public Challenge15()
		{
		}

		[Part1]
		public string Part1()
		{
			var history = new List<int>(startingNumbers);
			var lookup = new Dictionary<int, int>();

			for (int i = 0; i < history.Count - 1; i++)
				lookup.Add(history[i], i);

			for (int i = startingNumbers.Length; i < 2020; i++)
			{
				int last = history[i - 1];

				if (!lookup.ContainsKey(last))
				{
					history.Add(0);
					lookup.Add(last, i - 1);
				}
				else
				{
					int diff = (i - 1) - lookup[last];
					lookup[last] = i - 1;
					history.Add(diff);
				}
			}

			return history[history.Count - 1].ToString();
		}

		[Part2]
		public string Part2()
		{
			var history = new List<int>(startingNumbers);
			var lookup = new Dictionary<int, int>();

			for (int i = 0; i < history.Count - 1; i++)
				lookup.Add(history[i], i);

			for (int i = startingNumbers.Length; i < 30000000; i++)
			{
				int last = history[i - 1];

				if (!lookup.ContainsKey(last))
				{
					history.Add(0);
					lookup.Add(last, i - 1);
				}
				else
				{
					int diff = (i - 1) - lookup[last];
					lookup[last] = i - 1;
					history.Add(diff);
				}
			}

			return history[history.Count - 1].ToString();
		}
	}
}
