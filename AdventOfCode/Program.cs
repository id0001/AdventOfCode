using AdventOfCode.Challenges;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class Program
	{
		private const string Url = "https://adventofcode.com/2019";

		private static readonly IDictionary<string, Func<Task>> _challenges = new Dictionary<string, Func<Task>>()
		{
			{ "1a", () => new Challenge1a().RunAsync()  },
			{ "1b", () => new Challenge1b().RunAsync()  },
			{ "2a", () => new Challenge2a().RunAsync()  },
			{ "2b", () => new Challenge2b().RunAsync()  },
		};

		async static Task Main(string[] args)
		{
			string input = null;
			while (input == null || !_challenges.ContainsKey(input))
			{
				Console.Write("Enter challenge: ");
				input = Console.ReadLine();

				if (!_challenges.ContainsKey(input))
				{
					Console.WriteLine($"[ERROR] Challenge '{input}' does not exist.");
				}
			}

			Console.Clear();
			await _challenges[input]();

			if (Debugger.IsAttached)
				Console.ReadKey(false);
		}
	}
}
