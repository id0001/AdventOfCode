
using AdventOfCode.Chemistry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge14a : IChallenge
	{
		private const string Ore = "ORE";
		private const string Fuel = "FUEL";

		public string Id => "14a";

		public async Task<string> RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge14.txt");

			var reactions = new Dictionary<string, ChemicalReaction>();

			foreach (var line in lines)
			{
				var reaction = new ChemicalReaction(line);
				reactions.Add(reaction.Output.Key, reaction);
			}

			int oreUsed = CalculateOreCost(reactions, 1);

			return oreUsed.ToString();
		}

		private int CalculateOreCost(IDictionary<string, ChemicalReaction> reactions, int amountOfFuel)
		{
			var supply = new Dictionary<string, int>();

			return Request(reactions, supply, Fuel, amountOfFuel);
		}

		private int Request(IDictionary<string, ChemicalReaction> reactions, IDictionary<string, int> supply, string component, int amount)
		{
			int oreNeeded = 0;

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
				int needed = amount - supply[component];
				var reaction = reactions[component];
				int batches = (int)Math.Ceiling(needed / (double)reaction.Output.Value);
				foreach (var ingredient in reaction.Inputs)
				{
					oreNeeded += Request(reactions, supply, ingredient.Key, ingredient.Value * batches);
				}

				int leftover = batches * reaction.Output.Value - needed;
				supply[component] = leftover;
			}

			return oreNeeded;
		}
	}
}
