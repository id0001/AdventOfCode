//-------------------------------------------------------------------------------------------------
//
// Challenge2a.cs -- The Challenge2a class.
//
// Copyright (c) 2019 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using AdventOfCode.IntCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge2a class TODO: Describe class here
	/// </summary>
	internal class Challenge2a
	{
		public async Task RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge2.txt")).Split(',').Select(s => int.Parse(s)).ToArray();
			program[1] = 12;
			program[2] = 2;

			var computer = new IntCodeComputer();
			computer.LoadProgram(program);
			int result = computer.Execute();

			Console.WriteLine(result);
		}
	}
}
