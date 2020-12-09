using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(1)]
	public class Challenge1
	{
		private readonly IInputReader inputReader;

		private int[] input;

		public Challenge1(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			input = await inputReader.ReadLinesAsync<int>(1).ToArrayAsync();
		}

		[Part1]
		public string Part1()
		{
			for(int y = 0; y < input.Length; y++)
			{
				for(int x = 0; x < input.Length; x++)
				{
					if (x == y)
						continue;

					if(input[x] + input[y] == 2020)
					{
						return (input[x] * input[y]).ToString();
					}
				}
			}

			return "-1";
		}

		[Part2]
		public string Part2()
		{
			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input.Length; x++)
				{
					for(int z = 0; z < input.Length; z++)
					{
						if (x == y || x == z || y == z)
							continue;

						if (input[x] + input[y] + input[z] == 2020)
						{
							return (input[x] * input[y] * input[z]).ToString();
						}
					}
				}
			}

			return "-1";
		}
	}
}
