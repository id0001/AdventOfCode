//-------------------------------------------------------------------------------------------------
//
// Challenge6a.cs -- The Challenge6a class.
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
	/// The Challenge6a class TODO: Describe class here
	/// </summary>
	internal class Challenge6a : IChallenge
	{
		public string Id => "6a";

		public async Task RunAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("Assets/Challenge6.txt");

			var orbitalTree = new OrbitalTree(lines);

			int sum = orbitalTree.Sum(e => e.Depth);

			Console.WriteLine($"Sum: {sum}");
		}
	}
}
