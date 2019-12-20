//-------------------------------------------------------------------------------------------------
//
// Challenge2a.cs -- The Challenge2a class.
//
// Copyright (c) 2019 Marel. All rights reserved.
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
	/// The Challenge2a class TODO: Describe class here
	/// </summary>
	internal class Challenge2b
	{
		public async Task RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge2.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var computer = new IntCodeComputer();

			for (int noun = 0; noun < 100; noun++)
			{
				for (int verb = 0; verb < 100; verb++)
				{
					program[1] = noun;
					program[2] = verb;
					computer.LoadProgram(program);
					int result = computer.Execute();
					if (result == 19690720)
					{
						Console.WriteLine($"noun: {noun}, verb: {verb}, result: {100 * noun + verb}");
						return;
					}
				}
			}
		}
	}
}
