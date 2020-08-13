
using AdventOfCode.Chemistry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge14b : IChallenge
	{
		private const string Ore = "ORE";
		private const string Fuel = "FUEL";

		public string Id => "14b";

		public async Task<string> RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge14.txt");

			var reactions = new Dictionary<string, ChemicalReaction>();

			foreach (var line in lines)
			{
				var reaction = new ChemicalReaction(line);
				reactions.Add(reaction.Output.Key, reaction);
			}

			long amount = CalculateFuelProduced(reactions);

			return amount.ToString();
		}

		private long CalculateFuelProduced(IDictionary<string, ChemicalReaction> reactions)
		{
			long lo = 0L;
			long hi = 1000000000000L;

			long expected = 1000000000000L;

			while(hi - lo > 1)
			{
				long mid = (long)Math.Floor((hi + lo) / 2d);
				long cost = CalculateOreCost(reactions, mid);
				if (cost > expected)
					hi = mid;
				else
					lo = mid;
			}

			return lo;
		}

		private long CalculateOreCost(IDictionary<string, ChemicalReaction> reactions, long amountOfFuel)
		{
			var supply = new Dictionary<string, long>();

			return Request(reactions, supply, Fuel, amountOfFuel);
		}

		private long Request(IDictionary<string, ChemicalReaction> reactions, IDictionary<string, long> supply, string component, long amount)
		{
			long oreNeeded = 0;

			if (!supply.ContainsKey(component))
				supply.Add(component, 0);

			if (component == Ore)
			{
				return amount;
			}

			if (amount <= supply[component])
			{
				supply[component] -= amount;
			}
			else
			{
				long needed = amount - supply[component];
				var reaction = reactions[component];
				long batches = (long)Math.Ceiling(needed / (double)reaction.Output.Value);
				foreach (var ingredient in reaction.Inputs)
				{
					oreNeeded += Request(reactions, supply, ingredient.Key, ingredient.Value * batches);
				}

				long leftover = batches * reaction.Output.Value - needed;
				supply[component] = leftover;
			}

			return oreNeeded;
		}
	}
}
