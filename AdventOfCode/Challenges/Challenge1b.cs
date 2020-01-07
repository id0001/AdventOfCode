//-------------------------------------------------------------------------------------------------
//
// Challenge1b.cs -- The Challenge1b class.
//
// Copyright (c) 2019 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge1b class TODO: Describe class here
	/// </summary>
	internal class Challenge1b : IChallenge
	{
		public string Id => "1b";

		public async Task RunAsync() => Console.WriteLine((await File.ReadAllLinesAsync("Assets/Challenge11.txt"))
			.Select(s => int.Parse(s))
			.Aggregate(0, (a, b) => a + CalculateFuelRequirement(b)));

		private int CalculateFuelRequirement(int mass)
		{
			if (mass == 0) return 0;

			return Math.Max(0, (mass / 3 - 2)) + CalculateFuelRequirement(Math.Max(0, (mass / 3 - 2)));
		}
	}
}
