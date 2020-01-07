//-------------------------------------------------------------------------------------------------
//
// Challenge6b.cs -- The Challenge6b class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge6b class TODO: Describe class here
	/// </summary>
	internal class Challenge6b : IChallenge
	{
		public string Id => "6b";

		public async Task RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge6.txt");

			var orbitalTree = new OrbitalTree(lines);

			var transfers = orbitalTree.FindTransfers("YOU", "SAN");

			Console.WriteLine($"Transfers: {transfers}");
		}
	}
}
