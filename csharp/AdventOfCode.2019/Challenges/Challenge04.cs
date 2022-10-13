using System.Text.RegularExpressions;
using AdventOfCode.Core;

namespace AdventOfCode2019.Challenges
{
	[Challenge(4)]
	public class Challenge04
	{
		private const string RepeatingRule = @"(\d)\1+";

		[Part1]
		public string Part1()
		{
			const int min = 153517;
			const int max = 630395;

			var matches = new List<int>();
			for(var pwd = min; pwd < max; pwd++)
			{
				if (MatchAdjacent(pwd.ToString()) && MatchNeverDecrease(pwd.ToString()))
				{
					matches.Add(pwd);
				}
			}

			return matches.Count.ToString();
		}

		[Part2]
		public string Part2()
		{
			const int min = 153517;
			const int max = 630395;

			var matches = new List<int>();
			for (var pwd = min; pwd < max; pwd++)
			{
				if (MatchAdjacent2(pwd.ToString()) && MatchNeverDecrease(pwd.ToString()))
				{
					matches.Add(pwd);
				}
			}

			return matches.Count.ToString();
		}

		// Two adjacent digits are the same (like 22 in 122345).
		private static bool MatchAdjacent(string input) => Regex.IsMatch(input, RepeatingRule);

		// Two adjacent digits are the same but not 3 (like 22 in 122345).
		private static bool MatchAdjacent2(string input)
		{
			var digits = input.Select(i => int.Parse(i.ToString())).ToArray();

			var last = digits[0];
			for (var i = 1; i < digits.Length; i++)
			{
				if (digits[i] == last)
				{
					if (i - 1 == 0) // front
					{
						if (digits[i + 1] != last)
							return true;
					}
					else if (i + 1 == digits.Length) // back
					{
						if (digits[i - 2] != last)
							return true;
					}
					else // middle
					{
						if (digits[i - 2] != last && digits[i + 1] != last)
							return true;
					}
				}

				last = digits[i];
			}

			return false;
		}

		// Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
		private static bool MatchNeverDecrease(string input)
		{
			var digits = input.Select(i => int.Parse(i.ToString())).ToArray();

			var a = digits[0];
			for (var i = 1; i < digits.Length; i++)
			{
				var b = digits[i];
				if (b < a) return false;
				a = b;
			}

			return true;
		}
	}
}
