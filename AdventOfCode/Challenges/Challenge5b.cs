//-------------------------------------------------------------------------------------------------
//
// Challenge5b.cs -- The Challenge5b class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using AdventOfCode.IntCode;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge5b class TODO: Describe class here
	/// </summary>
	internal class Challenge5b : IChallenge
	{
		public string Id => "5b";

		public async Task RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge5.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var computer = new IntCodeComputer();
			computer.LoadProgram(program);
			int result = computer.Execute();

			Console.WriteLine(result);
		}
	}
}
