using System;
using System.Collections.Generic;


namespace AdventOfCode.Chemistry
{
	public class ChemicalStore
	{
		private IDictionary<string, long> store;

		public ChemicalStore()
		{
			store = new Dictionary<string, long>();
		}

		public long this[string key] => store[key];

		public void Modify(string name, long amount)
		{
			if (!store.ContainsKey(name))
				store.Add(name, 0);

			store[name] += amount;
		}

		public bool HasEnough(string name, long desiredAmount) => store.ContainsKey(name) ? store[name] >= desiredAmount : false;
	}
}
