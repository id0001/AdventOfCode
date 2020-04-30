using System;
using System.Collections.Generic;


namespace AdventOfCode.Chemistry
{
	public class ChemicalStore
	{
		private IDictionary<string, int> store;

		public ChemicalStore()
		{
			store = new Dictionary<string, int>();
		}

		public void Modify(string name, int amount)
		{
			if (!store.ContainsKey(name))
				store.Add(name, 0);

			store[name] += amount;
		}

		public bool HasEnough(string name, int desiredAmount) => store.ContainsKey(name) ? store[name] >= desiredAmount : false;
	}
}
