using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges
{
	[Challenge(14)]
	public class Challenge14
	{
		private readonly IInputReader _inputReader;
		private readonly Dictionary<string, Reaction> _reactions = new();

		public Challenge14(IInputReader inputReader)
		{
			_inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			await foreach (var line in _inputReader.ReadLinesAsync(14))
			{
				var inputs = new Dictionary<string, int>();

				var inputOutput = line.Trim().Split("=>");

				var inputList = inputOutput[0].Trim().Split(',');
				var outputSplit = inputOutput[1].Trim().Split(' ');

				foreach (var item in inputList)
				{
					var itemSplit = item.Trim().Split(' ');
					var inputAmount = int.Parse(itemSplit[0]);
					var inputType = itemSplit[1];
					inputs.Add(inputType, inputAmount);
				}

				var outputAmount = int.Parse(outputSplit[0]);
				var outputType = outputSplit[1];

				_reactions.Add(outputType, new Reaction(new KeyValuePair<string, int>(outputType, outputAmount), inputs));
			}
		}

		[Part1]
		public string Part1()
		{
			var oreUsed = Request("FUEL", 1, new Dictionary<string, long>());

			return oreUsed.ToString();
		}

		[Part2]
		public string Part2()
		{
			// Do a binary search to find the max produced fuel amount for 1 trillion ore.

			var lo = 0L;
			var hi = 1_000_000_000_000L;

			const long expected = 1_000_000_000_000L;

			while (hi - lo > 1L)
			{
				var mid = (long)Math.Floor((hi + lo) / 2d);
				var cost = Request("FUEL", mid, new Dictionary<string, long>());
				if (cost > expected)
					hi = mid;
				else
					lo = mid;
			}

			return lo.ToString();
		}

		private long Request(string component, long amount, Dictionary<string, long> supply)
		{
			if (!supply.ContainsKey(component))
				supply.Add(component, 0);

			// Take all from supply.
			if (supply[component] > amount)
			{
				supply[component] -= amount;
				return 0;
			}

			// Take some from supply and make the rest.
			amount -= supply[component];
			supply[component] = 0;

			// Ore is the base case.
			if (component == "ORE")
				return amount;

			var reaction = _reactions[component];

			var amountOfReactionsNeeded = (long)Math.Ceiling(amount / (double)reaction.Output.Value);
			var oreNeeded = reaction.Input.Sum(input => Request(input.Key, amountOfReactionsNeeded * input.Value, supply));
			var leftover = amountOfReactionsNeeded * reaction.Output.Value - amount;
			supply[component] += leftover;

			return oreNeeded;
		}

		private record Reaction(KeyValuePair<string, int> Output, Dictionary<string, int> Input);
	}
}
