
using AdventOfCode.Chemistry;
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

			var chemStore = new ChemicalStore();
			int oreUsed = ProduceChemical(Fuel, reactions, chemStore);

			return oreUsed.ToString();
		}

		/// <summary>
		/// Produce a chemical by traversing down the reaction path and produce every neccessary chemical.
		/// </summary>
		/// <param name="name">The chemical to produce</param>
		/// <param name="reactions">All possible reactions</param>
		/// <param name="chemStore">The chemical store</param>
		/// <returns>The consumed ore amount</returns>
		private int ProduceChemical(string name, IDictionary<string, ChemicalReaction> reactions, ChemicalStore chemStore)
		{
			int oreConsumed = 0;

			// Perform the reaction and produce the chemica.
			var reaction = reactions[name];

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
					while (!chemStore.HasEnough(input.Key, input.Value))
					{
						oreConsumed += ProduceChemical(input.Key, reactions, chemStore);
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
