using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(4)]
	public class Challenge04
	{
		private const string RepeatingRule = @"(\d)\1+";

		[Part1]
		public string Part1()
		{
			int min = 153517;
			int max = 630395;

			List<int> matches = new List<int>();
			for(int pwd = min; pwd < max; pwd++)
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
			int min = 153517;
			int max = 630395;

			List<int> matches = new List<int>();
			for (int pwd = min; pwd < max; pwd++)
			{
				if (MatchAdjacent2(pwd.ToString()) && MatchNeverDecrease(pwd.ToString()))
				{
					matches.Add(pwd);
				}
			}

			return matches.Count.ToString();
		}

		// Two adjacent digits are the same (like 22 in 122345).
		private bool MatchAdjacent(string input)
		{
			return Regex.IsMatch(input, RepeatingRule);
		}

		// Two adjacent digits are the same but not 3 (like 22 in 122345).
		private bool MatchAdjacent2(string input)
		{
			int[] digits = input.Select(i => int.Parse(i.ToString())).ToArray();

			int last = digits[0];
			for (int i = 1; i < digits.Length; i++)
			{
				if (digits[i] == last)
				{
					if (i - 1 == 0) // front
					{
						if (digits[i + 1] != last)
							return true;
					}
					else if (i + 1 == digits.Length) /// back
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
		private bool MatchNeverDecrease(string input)
		{
			int[] digits = input.Select(i => int.Parse(i.ToString())).ToArray();

			int a = digits[0];
			int b = digits[1];
			for (int i = 1; i < digits.Length; i++)
			{
				b = digits[i];
				if (b < a) return false;
				a = b;
			}

			return true;
		}
	}
}
