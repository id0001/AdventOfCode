//-------------------------------------------------------------------------------------------------
//
// Challenge7a.cs -- The Challenge7a class.
//
// Copyright (c) 2020 Marel. All rights reserved.
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
	/// The Challenge7a class TODO: Describe class here
	/// </summary>
	internal class Challenge7a : IChallenge
	{
		public string Id => "7a";

		public async Task RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge7.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var perms = new List<int[]>();
			GeneratePermutations(perms, new int[] { 0, 1, 2, 3, 4 }, 0, 4);

			var computer = new IntCodeComputer();

			int output = 0;
			Queue<int> inputs = new Queue<int>();

			computer.OnInput = () =>
			{
				Console.WriteLine($">>>: {inputs.Peek()}");
				return inputs.Dequeue();
			};

			computer.OnPrint = (v) =>
			{
				Console.WriteLine($"<<<: {v}");
				output = v;
			};

			int highest = int.MinValue;
			foreach (var permutation in perms)
			{
				output = 0;
				foreach (var signal in permutation)
				{
					computer.LoadProgram(program);
					inputs.Enqueue(signal);
					inputs.Enqueue(output);
					computer.Execute();
				}

				if (output > highest)
					highest = output;
			}

			Console.WriteLine();
			Console.WriteLine($"Hightest: {highest}");
		}

		private void GeneratePermutations(List<int[]> perms, int[] numbers, int start, int end)
		{
			if (end == start)
			{
				int[] perm = new int[5];
				Array.Copy(numbers, 0, perm, 0, 5);
				perms.Add(perm);
			}
			else
			{
				for (int i = start; i <= end; i++)
				{
					Swap(ref numbers[start], ref numbers[i]);
					GeneratePermutations(perms, numbers, start + 1, end);
					Swap(ref numbers[start], ref numbers[i]);
				}
			}
		}

		private void Swap(ref int a, ref int b)
		{
			if (a == b) return;

			int t = a;
			a = b;
			b = t;
		}
	}
}
