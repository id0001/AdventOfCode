//-------------------------------------------------------------------------------------------------
//
// Challenge9a.cs -- The Challenge9a class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using AdventOfCode.IntCode.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge9a class TODO: Describe class here
	/// </summary>
	internal class Challenge9b : IChallenge
	{
		public string Id => "9b";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge9.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var runner = new SimpleRunner(program);
			runner.In.Write(2);
			runner.Execute();

			return runner.Out.Read().ToString();
		}
	}
}
