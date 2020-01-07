//-------------------------------------------------------------------------------------------------
//
// Challenge3b.cs -- The Challenge3b class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge3b class TODO: Describe class here
	/// </summary>
	internal class Challenge3b : IChallenge
	{
		public string Id => "3b";

		public async Task RunAsync()
		{
			var lines = await File.ReadAllLinesAsync("Assets/Challenge3.txt");

			var wg1 = new WireGrid(lines[0]);
			var wg2 = new WireGrid(lines[1]);

			var intersections = WireGrid.FindIntersections(wg1, wg2).ToArray();
			if (intersections.Length == 0)
			{
				Console.WriteLine("No intersections");
			}
			else
			{
				Console.WriteLine($"Min nr of steps: {intersections.Select(p => (wg1[p], wg2[p])).Min(e => e.Item1.Steps + e.Item2.Steps)}");
			}
		}

		private class WireGrid
		{
			private readonly IDictionary<Point, WirePart> _grid = new Dictionary<Point, WirePart>();

			public WireGrid(string input)
			{
				string[] steps = input.Split(',');

				int x = 0;
				int y = 0;
				int nrSteps = 0;
				_grid.Add(new Point(), new WirePart('o', nrSteps));
				foreach (var step in steps)
				{
					string dir = step.Substring(0, 1);
					int length = int.Parse(step.Substring(1));

					for (int i = 0; i < length; i++)
					{
						nrSteps++;
						char c = '+';
						switch (dir)
						{
							case "U":
								c = '|';
								y--;
								break;
							case "R":
								c = '-';
								x++;
								break;
							case "D":
								c = '|';
								y++;
								break;
							case "L":
								c = '-';
								x--;
								break;
						}

						var p = new Point(x, y);
						if (_grid.ContainsKey(p))
						{
							_grid[p] = new WirePart('+', nrSteps);
						}
						else if (i + 1 == length && steps.Last() != step)
						{
							_grid[p] = new WirePart('+', nrSteps);
						}
						else
						{
							_grid[p] = new WirePart(c, nrSteps);
						}
					}
				}
			}

			public WirePart this[Point p] => _grid[p];

			public void PrintGrid()
			{
				int top = _grid.Keys.Min(e => e.Y) - 2;
				int bottom = _grid.Keys.Max(e => e.Y) + 2;
				int left = _grid.Keys.Min(e => e.X) - 2;
				int right = _grid.Keys.Max(e => e.X) + 2;

				StringBuilder sb = new StringBuilder();
				for (int y = top; y <= bottom; y++)
				{
					for (int x = left; x <= right; x++)
					{
						var p = new Point(x, y);

						if (p == Point.Zero)
						{
							sb.Append('o');
						}
						else if (!_grid.ContainsKey(p))
						{
							sb.Append('.');
						}
						else
						{
							sb.Append(_grid[p]);
						}
					}

					Console.WriteLine(sb);
					sb.Clear();
				}
			}

			public static IEnumerable<Point> FindIntersections(WireGrid a, WireGrid b)
			{
				return a._grid.Keys.Where(p => p != Point.Zero && b._grid.Keys.Contains(p));
			}
		}

		private class WirePart
		{
			public WirePart(char symbol, int steps)
			{
				Symbol = symbol;
				Steps = steps;
			}

			public char Symbol { get; set; }
			public int Steps { get; set; }
		}
	}
}
