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
	/// The Challenge3a class TODO: Describe class here
	/// </summary>
	internal class Challenge3a : IChallenge
	{
		public string Id => "3a";

		public async Task<string> RunAsync()
		{
			var lines = await File.ReadAllLinesAsync("Assets/Challenge3.txt");

			var wg1 = new WireGrid(lines[0]);
			var wg2 = new WireGrid(lines[1]);

			var intersections = WireGrid.FindIntersections(wg1, wg2).ToArray();
			return intersections.Min(e => e.ManhattanDistance).ToString();
		}

		private class WireGrid
		{
			private readonly IDictionary<Point, char> _grid = new Dictionary<Point, char>();

			public WireGrid(string input)
			{
				string[] steps = input.Split(',');

				int x = 0;
				int y = 0;
				_grid.Add(new Point(), 'o');
				foreach (var step in steps)
				{
					string dir = step.Substring(0, 1);
					int length = int.Parse(step.Substring(1));

					for (int i = 0; i < length; i++)
					{
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
							_grid[p] = '+';
						}
						else if (i + 1 == length && steps.Last() != step)
						{
							_grid[p] = '+';
						}
						else
						{
							_grid[p] = c;
						}
					}
				}
			}

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
	}
}
