using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(6)]
	public class Challenge6
	{
		private readonly IInputReader inputReader;

		public Challenge6(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Part1]
		public async Task<string> Part1()
		{
			int yesCount = 0;
			ISet<char> answers = new HashSet<char>();
			await foreach (var line in inputReader.ReadLinesAsync(6))
			{
				if (string.IsNullOrEmpty(line))
				{
					yesCount += answers.Count;
					answers.Clear();
					continue;
				}

				foreach (char c in line)
				{
					answers.Add(c);
				}
			}

			yesCount += answers.Count;

			return yesCount.ToString();
		}

		[Part2]
		public async Task<string> Part2()
		{
			int yesCount = 0;
			int groupCount = 0;
			IDictionary<char, int> answers = new Dictionary<char, int>();
			await foreach (var line in inputReader.ReadLinesAsync(6))
			{
				if (string.IsNullOrEmpty(line))
				{
					yesCount += answers.Count(e => e.Value == groupCount);
					answers.Clear();
					groupCount = 0;
					continue;
				}

				groupCount++;

				foreach (char c in line)
				{
					if (!answers.ContainsKey(c))
						answers.Add(c, 0);

					answers[c] += 1;
				}
			}

			yesCount += answers.Count(e => e.Value == groupCount);

			return yesCount.ToString();
		}
	}
}
