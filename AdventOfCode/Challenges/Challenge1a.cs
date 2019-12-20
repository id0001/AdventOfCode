//-------------------------------------------------------------------------------------------------
//
// Challenge1a.cs -- The Challenge1a class.
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
	/// The Challenge1a class TODO: Describe class here
	/// </summary>
	internal class Challenge1a
	{
		public async Task RunAsync() => Console.Out.WriteLine((await File.ReadAllLinesAsync("Assets/Challenge1.txt"))
			.Select(s => int.Parse(s))
			.Aggregate(0, (a, b) => a + ((b / 3) - 2)));
	}
}
