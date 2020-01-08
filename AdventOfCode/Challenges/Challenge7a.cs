using AdventOfCode.IntCode;
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
	/// The Challenge7a class TODO: Describe class here
	/// </summary>
	internal class Challenge7a : IChallenge
	{
		public string Id => "7a";

		public async Task<string> RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge7.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var perms = new List<int[]>();
			GeneratePermutations(perms, new int[] { 0, 1, 2, 3, 4 }, 0, 4);

			int highest = int.MinValue;
			int signal = 0;
			foreach(var permutation in perms)
			{
				foreach(var phase in permutation)
				{
					var computer = new SimpleRunner(program);
					computer.QueueInput(phase);
					computer.QueueInput(signal);
					computer.Execute();
					signal = computer.ReadOutput();
				}

				if(signal > highest)
				{
					highest = signal;
				}

				signal = 0;
			}

			return highest.ToString();
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
