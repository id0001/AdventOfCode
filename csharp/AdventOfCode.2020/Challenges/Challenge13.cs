using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(13)]
	public class Challenge13
	{
		private readonly IInputReader inputReader;
		private int earliestDepartureTime;
		private int[] busses;
		private IDictionary<int, int> offsets = new SortedDictionary<int, int>();

		public Challenge13(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			string[] lines = await inputReader.ReadLinesAsync(13).ToArrayAsync();
			earliestDepartureTime = int.Parse(lines[0]);
			busses = lines[1].Split(',').Where(e => e != "x").Select(e => int.Parse(e)).ToArray();

			int offset = 0;
			foreach (string s in lines[1].Split(','))
			{
				if (s != "x")
				{
					offsets.Add(int.Parse(s), offset);
				}

				offset++;
			}
		}

		[Part1]
		public string Part1()
		{
			var ordered = busses.ToDictionary(kv => kv, kv => (int)(Math.Ceiling(earliestDepartureTime / (double)kv) * kv)).OrderBy(kv => kv.Value).ToArray();
			return (ordered[0].Key * (ordered[0].Value - earliestDepartureTime)).ToString();
		}

		[Part2]
		public string Part2()
		{
			long totalMod = MathEx.Product(busses.Select(e => (long)e).ToArray());
			long total = 0;

			for(int i = 1; i < busses.Length; i++)
			{
				long bi = busses[i] - offsets[busses[i]];
				long ni = totalMod / busses[i];
				long xi = MathEx.ModInverse(ni, busses[i]);
				total += bi * ni * xi;
			}
			

			return (total % totalMod).ToString();
		}

		public bool Departs(int bus, long t)
		{
			return t % bus == 0;
		}
	}
}
