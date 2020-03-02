using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge6b : IChallenge
	{
		public string Id => "6b";

		public async Task<string> RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge6.txt");

			var orbitalTree = new OrbitalTree(lines);

			var transfers = orbitalTree.FindTransfers("YOU", "SAN");

			return transfers.ToString();
		}
	}
}
