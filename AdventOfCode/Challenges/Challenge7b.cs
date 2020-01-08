using AdventOfCode.IntCode;
using AdventOfCode.IntCode.Core;
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
	internal class Challenge7b : IChallenge
	{
		public string Id => "7b";

		public async Task<string> RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge7.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var perms = new List<int[]>() { new[] { 9, 8, 7, 6, 5 } };
			//GeneratePermutations(perms, new int[] { 5, 6, 7, 8, 9 }, 0, 4);

			//var ampA = new Amp('A', 9, program);
			//var ampB = new Amp('B', 8, program);
			//var ampC = new Amp('C', 7, program);
			//var ampD = new Amp('D', 6, program);
			//var ampE = new Amp('E', 5, program);

			//var ampArray = new Amp[5];

			//ampA.PipeTo(ampB)
			//	.PipeTo(ampC)
			//	.PipeTo(ampD)
			//	.PipeTo(ampE)
			//	.PipeTo(ampA);

			return "faak";
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
