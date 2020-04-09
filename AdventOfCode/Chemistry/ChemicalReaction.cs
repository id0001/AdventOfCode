
using System;
using System.Collections.Generic;


namespace AdventOfCode.Chemistry
{
	public class ChemicalReaction
	{
		public ChemicalReaction(string input)
		{
			InputString = input;

			string[] io = input.Split("=>", StringSplitOptions.RemoveEmptyEntries);

			if (io.Length != 2)
				throw new ArgumentException($"The input was not valid: {input}");

			string[] inputs = io[0].Split(',', StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in inputs)
			{
				(int amount, string name) = ParsePart(item.Trim());
				Requirements.Add(name, amount);
			}

			(AmountProduced, ElementProduced) = ParsePart(io[1].Trim());
		}

		public IDictionary<string, int> Requirements = new Dictionary<string, int>();

		public string InputString { get; }

		public string ElementProduced { get; }

		public int AmountProduced { get; }

		/// <summary>
		/// Parse the amount and name of the element: '3 ABCD' -> {3} {ABCD}
		/// </summary>
		/// <param name="part">The part</param>
		/// <returns>A tuple with the amount and the name of the element</returns>
		private (int Amount, string Name) ParsePart(string part)
		{
			string[] split = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (split.Length != 2)
				throw new ArgumentException($"The input was not valid: {part}");

			if (!int.TryParse(split[0].Trim(), out int amount))
				throw new ArgumentException($"The input was not valid: {part}");

			return (amount, split[1].Trim());
		}
	}
}
