using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(14)]
	public class Challenge14
	{
		private readonly IInputReader inputReader;

		public Challenge14(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{

		}

		[Part1]
		public async Task<string> Part1Async()
		{
			var memory = new Dictionary<ulong, ulong>();

			string mask = null;

			await foreach (var line in inputReader.ReadLinesAsync(14))
			{
				if (line.StartsWith("mask = "))
				{
					mask = line.Substring("mask = ".Length);
				}
				else
				{
					var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
					ulong addr = ulong.Parse(match.Groups[1].Value);
					ulong value = ulong.Parse(match.Groups[2].Value);

					if (!memory.ContainsKey(addr))
						memory.Add(addr, 0);

					memory[addr] = ApplyMask(value, mask);
				}
			}

			return memory.Sum(kv => (long)kv.Value).ToString();
		}

		private ulong ApplyMask(ulong value, string mask)
		{
			for (int i = 0; i < mask.Length; i++)
			{
				switch (mask[i])
				{
					case '0':
						value = value & MaskFor(i);
						break;
					case '1':
						value = (value & MaskFor(i)) + (1ul << (35 - i));
						break;
				}
			}

			return value;
		}

		private void PrintBin(ulong v)
		{
			Console.WriteLine(Convert.ToString((long)v, 2).PadLeft(64, '0'));
		}

		private ulong MaskFor(int i)
		{
			ulong realMask = 0x0000_000F_FFFF_FFFF;
			return realMask - (1ul << (35 - i));
		}
	}
}
