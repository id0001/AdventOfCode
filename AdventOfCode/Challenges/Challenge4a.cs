using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge4a class TODO: Describe class here
	/// </summary>
	internal class Challenge4a : IChallenge
	{
		private const string RepeatingRule = @"(\d)\1+";

		public string Id => "4a";

		public Task<string> RunAsync()
		{
			int min = 152517;
			int max = 630395;

			List<int> matches = new List<int>();
			for (int pwd = min; pwd < max; pwd++)
			{
				if (MatchAdjacent(pwd.ToString()) && MatchNeverDecrease(pwd.ToString()))
				{
					matches.Add(pwd);
				}
			}

			return Task.FromResult(matches.Count.ToString());
		}

		// Two adjacent digits are the same (like 22 in 122345).
		private bool MatchAdjacent(string input)
		{
			return Regex.IsMatch(input, RepeatingRule);
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
