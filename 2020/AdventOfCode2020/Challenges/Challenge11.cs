using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(11)]
	public class Challenge11
	{
		private readonly IInputReader inputReader;
		private char[] input;
		private int width;
		private int height;

		public Challenge11(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			string[] lines = await inputReader.ReadLinesAsync(11).ToArrayAsync();
			height = lines.Length;
			width = lines[0].Length;
			input = lines.SelectMany(line => line.ToCharArray()).ToArray();
		}

		[Part1]
		public string Part1()
		{
			char[] state = new char[input.Length];
			Array.Copy(input, state, state.Length);

			bool stateChanged = false;
			do
			{
				stateChanged = false;
				char[] newState = new char[state.Length];
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						stateChanged |= state[Index(x, y)] switch
						{
							'.' => Ignore(newState, x, y),
							'L' => Occupy1(state, newState, x, y),
							'#' => Empty1(state, newState, x, y),
							_ => throw new NotSupportedException()
						};
					}
				}

				state = newState;
			}
			while (stateChanged);

			return state.Count(e => e == '#').ToString();
		}

		[Part2]
		public string Part2()
		{
			char[] state = new char[input.Length];
			Array.Copy(input, state, state.Length);

			bool stateChanged = false;
			do
			{
				stateChanged = false;
				char[] newState = new char[state.Length];
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						stateChanged |= state[Index(x, y)] switch
						{
							'.' => Ignore(newState, x, y),
							'L' => Occupy2(state, newState, x, y),
							'#' => Empty2(state, newState, x, y),
							_ => throw new NotSupportedException()
						};
					}
				}

				state = newState;
			}
			while (stateChanged);

			return state.Count(e => e == '#').ToString();

		}

		private void PrintState(char[] state)
		{
			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					sb.Append(state[Index(x, y)]);
				}

				sb.AppendLine();
			}

			Console.WriteLine(sb);
			Console.WriteLine();
		}

		private bool Ignore(char[] newState, int px, int py)
		{
			newState[Index(px, py)] = '.';
			return false;
		}

		private bool Occupy1(char[] oldState, char[] newState, int px, int py)
		{
			for (int y = py - 1; y <= py + 1; y++)
			{
				for (int x = px - 1; x <= px + 1; x++)
				{
					if (!(x == px && y == py) && WithinBoundaries(x, y) && oldState[Index(x, y)] == '#')
					{
						newState[Index(px, py)] = 'L';
						return false;
					}
				}
			}

			newState[Index(px, py)] = '#';
			return true;
		}

		private bool Occupy2(char[] oldState, char[] newState, int px, int py)
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (!(x == 0 && y == 0) && CastRay(oldState, px, py, x, y) == '#')
					{
						newState[Index(px, py)] = 'L';
						return false;
					}
				}
			}

			newState[Index(px, py)] = '#';
			return true;
		}

		private bool Empty1(char[] oldState, char[] newState, int px, int py)
		{
			int count = 0;
			for (int y = py - 1; y <= py + 1; y++)
			{
				for (int x = px - 1; x <= px + 1; x++)
				{
					if (!(x == px && y == py) && WithinBoundaries(x, y) && oldState[Index(x, y)] == '#' && ++count >= 4)
					{
						newState[Index(px, py)] = 'L';
						return true;
					}
				}
			}

			newState[Index(px, py)] = '#';
			return false;
		}

		private bool Empty2(char[] oldState, char[] newState, int px, int py)
		{
			int count = 0;
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (!(x == 0 && y == 0) && CastRay(oldState, px, py, x, y) == '#' && ++count >= 5)
					{
						newState[Index(px, py)] = 'L';
						return true;
					}
				}
			}

			newState[Index(px, py)] = '#';
			return false;
		}

		private int Index(int x, int y) => (y * width) + x;

		private bool WithinBoundaries(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

		private char CastRay(char[] map, int sx, int sy, int dx, int dy)
		{
			char c = '.';
			int x = sx + dx;
			int y = sy + dy;
			while (WithinBoundaries(x, y))
			{
				if (map[Index(x, y)] != '.')
				{
					c = map[Index(x, y)];
					break;
				}

				x += dx;
				y += dy;
			}

			return c;
		}
	}
}
