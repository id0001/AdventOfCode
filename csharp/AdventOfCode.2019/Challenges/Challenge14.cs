using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(14)]
	public class Challenge14
	{
		private readonly IInputReader inputReader;
		private Dictionary<string, Reaction> reactions;

		public Challenge14(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			reactions = new Dictionary<string, Reaction>();

			await foreach (var line in inputReader.ReadLinesAsync(14))
			{
				var inputs = new Dictionary<string, int>();

				string[] inputOutput = line.Trim().Split("=>");

				string[] inputList = inputOutput[0].Trim().Split(',');
				string[] outputSplit = inputOutput[1].Trim().Split(' ');

				foreach (var item in inputList)
				{
					string[] itemSplit = item.Trim().Split(' ');
					int inputAmount = int.Parse(itemSplit[0]);
					string inputType = itemSplit[1];
					inputs.Add(inputType, inputAmount);
				}

				int outputAmount = int.Parse(outputSplit[0]);
				string outputType = outputSplit[1];

				reactions.Add(outputType, new Reaction(new KeyValuePair<string, int>(outputType, outputAmount), inputs));
			}
		}

		[Part1]
		public string Part1()
		{
			long oreUsed = Request("FUEL", 1, new Dictionary<string, long>());

			return oreUsed.ToString();
		}

		[Part2]
		public string Part2()
		{
			// Do a binary search to find the max produced fuel amount for 1 trillion ore.

			long lo = 0L;
			long hi = 1_000_000_000_000L;

			long expected = 1_000_000_000_000L;

			while (hi - lo > 1L)
			{
				long mid = (long)Math.Floor((hi + lo) / 2d);
				long cost = Request("FUEL", mid, new Dictionary<string, long>());
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

			long oreNeeded = 0;

			var reaction = reactions[component];

			long amountOfReactionsNeeded = (long)Math.Ceiling(amount / (double)reaction.Output.Value);

			foreach (var input in reaction.Input)
			{
				oreNeeded += Request(input.Key, amountOfReactionsNeeded * input.Value, supply);
			}

			long leftover = amountOfReactionsNeeded * reaction.Output.Value - amount;
			supply[component] += leftover;

			return oreNeeded;
		}

		private record Reaction(KeyValuePair<string, int> Output, Dictionary<string, int> Input);
	}
}
