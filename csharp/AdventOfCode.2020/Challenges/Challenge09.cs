using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(9)]
	public class Challenge09
	{
		private readonly IInputReader inputReader;

		public Challenge09(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Part1]
		public async Task<string> Part1Async()
		{
			CircularBuffer<long> buffer = new CircularBuffer<long>(25);
			int index = 0;

			await foreach (long value in inputReader.ReadLinesAsync<long>(9))
			{
				if (index >= 25)
				{
					if (!Validate(buffer.ToArray(), value))
						return value.ToString();
				}

				buffer.Add(value);
				index++;
			}

			return null;
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			long target = 1930745883;

			CircularBuffer<long> buffer = new CircularBuffer<long>(25);
			int index = 0;

			await foreach (long value in inputReader.ReadLinesAsync<long>(9))
			{
				if (index >= 25)
				{
					long[] set = FindContiguousSet(buffer, target);
					if (set.Length > 0)
						return (set.Min() + set.Max()).ToString();
				}

				buffer.Add(value);
				index++;
			}

			return null;
		}

		private bool Validate(IList<long> buffer, long value)
		{
			for(int y = 0; y < buffer.Count; y++)
			{
				for (int x = y; x < buffer.Count; x++)
				{
					if (value == buffer[x] + buffer[y])
					{
						return true;
					}
				}
			}

			return false;
		}

		private long[] FindContiguousSet(IList<long> buffer, long target)
		{
			long sum = buffer[0] + buffer[1];

			if (sum == target)
				return buffer.Take(2).ToArray();

			for (int i = 2; i < buffer.Count; i++)
			{
				sum += buffer[i];
				if(sum == target)
				{
					return buffer.Take(i + 1).ToArray();
				}
			}

			return Array.Empty<long>();
		}
	}
}
