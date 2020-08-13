using AdventOfCode.Challenges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class Program
	{
		private const string Url = "https://adventofcode.com/2019";

		private static IDictionary<string, IChallenge> _challenges;

		private static IDictionary<string, string> _answers = new Dictionary<string, string>
		{
			{ "1a", "3394106" },
			{ "1b", "5088280" },
			{ "2a", "4138658" },
			{ "2b", "7264" },
			{ "3a", "273" },
			{ "3b", "15622" },
			{ "4a", "1729" },
			{ "4b", "1172" },
			{ "5a", "7988899" },
			{ "5b", "13758663" },
			{ "6a", "142915" },
			{ "6b", "283" },
			{ "7a", "422858" },
			{ "7b", "14897241" },
			{ "8a", "1677" },
			{ "8b", "#  # ###  #  # #### ###  \r\n#  # #  # #  # #    #  # \r\n#  # ###  #  # ###  #  # \r\n#  # #  # #  # #    ###  \r\n#  # #  # #  # #    #    \r\n ##  ###   ##  #    #    \r\n"},
			{ "9a", "3100786347" },
			{ "9b", "87023" },
			{ "10a", "280" },
			{ "10b", "706" },
			{ "11a", "2883" },
			{ "11b", ".#....####.###...##..###..#.....##..####...\r\n.#....#....#..#.#..#.#..#.#....#..#....#...\r\n.#....###..#..#.#....#..#.#....#......#....\r\n.#....#....###..#....###..#....#.##..#.....\r\n.#....#....#....#..#.#....#....#..#.#......\r\n.####.####.#.....##..#....####..###.####...\r\n" },
			{ "12a", "8310" },
			{ "12b", "319290382980408" },
			{ "13a", "309" },
			{ "13b", "15410" },
			{ "14a", "1046184" },
			{ "14b", "1639374" }
		};

		async static Task Main(string[] args)
		{
			_challenges = LoadChallenges();

			string input = null;
			while (input == null || (!_challenges.ContainsKey(input) && input != "test"))
			{
				Console.Write("Enter challenge: ");
				input = Console.ReadLine();

				if (!_challenges.ContainsKey(input) && input != "test")
				{
					Console.WriteLine($"[ERROR] Challenge '{input}' does not exist.");
				}
			}

			Console.Clear();

			if (input == "test")
			{
				await PerformTestAllAsync();
			}
			else
			{
				await _challenges[input].RunAsync();
			}

			if (Debugger.IsAttached)
				Console.ReadKey(false);
		}

		private static IDictionary<string, IChallenge> LoadChallenges()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(e => !e.IsInterface && !e.IsAbstract && e.GetInterfaces().Contains(typeof(IChallenge))).ToList();

			return types.Select(e => (IChallenge)Activator.CreateInstance(e)).ToDictionary(kv => kv.Id);
		}

		private static async Task PerformTestAllAsync()
		{
			Console.WriteLine("Running tests...");
			foreach (var challenge in _answers.Keys)
			{
				Console.Write($"{challenge}... ");
				string result = await _challenges[challenge].RunAsync();
				if (result == _answers[challenge])
				{
					Console.WriteLine("OK.");
				}
				else
				{
					Console.WriteLine($"FAIL. Expected {_answers[challenge]}, got {result}.");
				}
			}
		}
	}
}
