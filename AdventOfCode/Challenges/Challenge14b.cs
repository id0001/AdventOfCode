
using AdventOfCode.Chemistry;
using System;
using System.Collections.Generic;
using System.IO;
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
			//string[] lines = await File.ReadAllLinesAsync("Assets/TestInput.txt");

			var reactions = new Dictionary<string, ChemicalReaction>();

			foreach (var line in lines)
			{
				var reaction = new ChemicalReaction(line);
				reactions.Add(reaction.Output.Key, reaction);
			}

			var exactOreRequirement = CalculateExactOreAmount(reactions[Fuel], 1d, reactions);
			long amountNeeded = (long)(1000000000000L / exactOreRequirement);

			int ore = ProduceChemical(Fuel, amountNeeded, reactions, new ChemicalStore());

			return amountNeeded.ToString();
		}

		private double CalculateExactOreAmount(ChemicalReaction reaction, double amountNeeded, IDictionary<string, ChemicalReaction> reactions)
		{
			if (reaction.Inputs.ContainsKey(Ore))
			{
				return ((double)reaction.Inputs[Ore] / reaction.Output.Value) * amountNeeded;
			}

			double oreNeeded = 0d;

			foreach (var input in reaction.Inputs)
			{
				var inputReaction = reactions[input.Key];
				oreNeeded += CalculateExactOreAmount(inputReaction, input.Value * amountNeeded / reaction.Output.Value, reactions);
			}

			return oreNeeded;
		}

		/// <summary>
		/// Produce a chemical by traversing down the reaction path and produce every neccessary chemical.
		/// </summary>
		/// <param name="name">The chemical to produce</param>
		/// <param name="reactions">All possible reactions</param>
		/// <param name="chemStore">The chemical store</param>
		/// <returns>The consumed ore amount</returns>
		private int ProduceChemical(string name, long amount, IDictionary<string, ChemicalReaction> reactions, ChemicalStore chemStore)
		{
			int oreConsumed = 0;

			// Perform the reaction and produce the chemical.
			var reaction = reactions[name];

			var loopAmount = (long)Math.Ceiling((double)amount / reaction.Output.Value);

			// Go over inputs
			foreach (var input in reaction.Inputs)
			{
				if (input.Key == Ore)
				{
					oreConsumed += input.Value;
					continue;
				}
				else
				{
					while (!chemStore.HasEnough(input.Key, input.Value * amount))
					{
						oreConsumed += ProduceChemical(input.Key, input.Value * amount, reactions, chemStore);
					}
				}

				// Consume input to produce output
				chemStore.Modify(input.Key, -input.Value);
			}

			// Add produced amount to the store.
			chemStore.Modify(name, reaction.Output.Value);
			return oreConsumed;
		}
	}
}
