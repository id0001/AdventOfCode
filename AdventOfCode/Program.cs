using AdventOfCode.Challenges;
using System;
using System.Collections;
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

		async static Task Main(string[] args)
		{
			_challenges = LoadChallenges();

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
			await _challenges[input].RunAsync();

			if (Debugger.IsAttached)
				Console.ReadKey(false);
		}

		private static IDictionary<string, IChallenge> LoadChallenges()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(e => !e.IsInterface && !e.IsAbstract && e.GetInterfaces().Contains(typeof(IChallenge))).ToList();

			return types.Select(e => (IChallenge)Activator.CreateInstance(e)).ToDictionary(kv => kv.Id);
		}
	}
}
