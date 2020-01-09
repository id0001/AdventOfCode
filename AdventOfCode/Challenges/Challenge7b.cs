using AdventOfCode.IntCode;
using AdventOfCode.IntCode.Core;
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
	internal class Challenge7b : IChallenge
	{
		public string Id => "7b";

		public async Task<string> RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge7.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var perms = new List<int[]>();
			GeneratePermutations(perms, new int[] { 5, 6, 7, 8, 9 }, 0, 4);

			int highest = int.MinValue;
			foreach (var permutation in perms)
			{
				var ampA = new Amp("A", program, permutation[0]);
				var ampB = new Amp("B", program, permutation[1]);
				var ampC = new Amp("C", program, permutation[2]);
				var ampD = new Amp("D", program, permutation[3]);
				var ampE = new Amp("E", program, permutation[4]);

				ampA.PipeTo(ampB);
				ampB.PipeTo(ampC);
				ampC.PipeTo(ampD);
				ampD.PipeTo(ampE);
				ampE.PipeTo(ampA);

				ampA.Initialize();
				ampB.Initialize();
				ampC.Initialize();
				ampD.Initialize();
				ampE.Initialize();

				ampA.In.Write(0);

				Queue<Amp> executionQueue = new Queue<Amp>(new[] { ampA, ampB, ampC, ampD, ampE });
				while (!ampE.IsHalted || executionQueue.Count > 0)
				{
					var amp = executionQueue.Dequeue();
					amp.Execute();

					if (!amp.IsHalted)
					{
						executionQueue.Enqueue(amp);
					}
				}

				int result = ampE.Out.ReadToEnd().LastOrDefault();
				if (result > highest)
				{
					highest = result;
				}
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
