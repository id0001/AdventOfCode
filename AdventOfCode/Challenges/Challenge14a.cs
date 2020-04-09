
using AdventOfCode.Chemistry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			string[] lines = await File.ReadAllLinesAsync("Assets/TestInput.txt");
			//string[] lines = await File.ReadAllLinesAsync("Assets/Challenge14.txt");

			var reactions = new Dictionary<string, ChemicalReaction>();

			foreach (var line in lines)
			{
				var reaction = new ChemicalReaction(line);
				reactions.Add(reaction.ElementProduced, reaction);
			}

			var dict = new Dictionary<string, int>();
			var fuelReaction = reactions[Fuel];
			int amount = PerformReaction(fuelReaction, 1,dict , reactions);

			return "";
		}

		private int PerformReaction(ChemicalReaction reaction, int amountNeeded, IDictionary<string, int> produced, IDictionary<string, ChemicalReaction> allReactions)
		{
			if (produced.TryGetValue(reaction.ElementProduced, out int alreadyProduced))
			{
				int toReduce = Math.Min(amountNeeded, alreadyProduced);
				amountNeeded -= toReduce;
				produced[reaction.ElementProduced] -= toReduce;
			}

			int times = (int)Math.Ceiling((float)amountNeeded / reaction.AmountProduced);
			int rest = amountNeeded % reaction.AmountProduced;

			int oreCost = 0;
			foreach (var requiredChemicals in reaction.Requirements)
			{
				int rcount = requiredChemicals.Value * times;

				if (requiredChemicals.Key == Ore)
				{
					oreCost += rcount;
				}
				else
				{
					oreCost += PerformReaction(allReactions[requiredChemicals.Key], rcount, produced, allReactions);
				}
			}

			if (!produced.ContainsKey(reaction.ElementProduced))
			{
				produced.Add(reaction.ElementProduced, 0);
			}

			produced[reaction.ElementProduced] += rest;

			return oreCost;
		}
	}
}
