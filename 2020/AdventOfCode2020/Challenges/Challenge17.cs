using AdventOfCodeLib;
using AdventOfCodeLib.Collections;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(17)]
	public class Challenge17
	{
		private readonly IInputReader inputReader;

		public Challenge17(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}


		[Part1]
		public async Task<string> Part1Async()
		{
			var state = new SparseSpatialMap<Point3, bool>();

			string[] lines = await inputReader.ReadLinesAsync(17).ToArrayAsync();
			for (int y = 0; y < lines.Length; y++)
			{
				for (int x = 0; x < lines[y].Length; x++)
				{
					if (lines[y][x] == '#')
						state.Add(new Point3(x, y, 0), true);
				}
			}

			//PrintSlice(state, 0);

			for (int i = 0; i < 6; i++)
			{
				var newState = new SparseSpatialMap<Point3, bool>();

				for (int z = state.Bounds.GetMin(2) - 1; z < state.Bounds.GetMax(2) + 1; z++)
				{
					for (int y = state.Bounds.GetMin(1) - 1; y < state.Bounds.GetMax(1) + 1; y++)
					{
						for (int x = state.Bounds.GetMin(0) - 1; x < state.Bounds.GetMax(0) + 1; x++)
						{
							var p = new Point3(x, y, z);
							var neighbors = state.GetNeighbors(p).ToArray();
							switch (state.GetValue(p, false))
							{
								case true when neighbors.Count(n => n.Value) is 2 or 3:
									newState.AddOrUpdate(p, true);
									break;
								case false when neighbors.Count(n => n.Value) == 3:
									newState.AddOrUpdate(p, true);
									break;
								default:
									newState.Remove(p);
									break;
							}
						}
					}
				}

				state = newState;

				//Console.WriteLine($"After {i + 1} cycle{(i > 0 ? "s" : "")}:");
				//Console.WriteLine();
				//for (int z = state.Bounds.GetMin(2); z < state.Bounds.GetMax(2); z++)
				//{
				//	Console.WriteLine($"z={z}");
				//	PrintSlice(state, z);
				//}
			}

			return state.Values.Count(e => e).ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			var state = new SparseSpatialMap<Point4, bool>();

			string[] lines = await inputReader.ReadLinesAsync(17).ToArrayAsync();
			for (int y = 0; y < lines.Length; y++)
			{
				for (int x = 0; x < lines[y].Length; x++)
				{
					if (lines[y][x] == '#')
						state.Add(new Point4(x, y, 0, 0), true);
				}
			}

			for (int i = 0; i < 6; i++)
			{
				var newState = new SparseSpatialMap<Point4, bool>();

				for (int w = state.Bounds.GetMin(3) - 1; w < state.Bounds.GetMax(3) + 1; w++)
				{
					for (int z = state.Bounds.GetMin(2) - 1; z < state.Bounds.GetMax(2) + 1; z++)
					{
						for (int y = state.Bounds.GetMin(1) - 1; y < state.Bounds.GetMax(1) + 1; y++)
						{
							for (int x = state.Bounds.GetMin(0) - 1; x < state.Bounds.GetMax(0) + 1; x++)
							{
								var p = new Point4(x, y, z, w);
								var neighbors = state.GetNeighbors(p).ToArray();
								switch (state.GetValue(p, false))
								{
									case true when neighbors.Count(n => n.Value) is 2 or 3:
										newState.AddOrUpdate(p, true);
										break;
									case false when neighbors.Count(n => n.Value) == 3:
										newState.AddOrUpdate(p, true);
										break;
									default:
										newState.Remove(p);
										break;
								}
							}
						}
					}
				}

				state = newState;

			}

			return state.Values.Count(e => e).ToString();
		}

		public static void PrintSlice<T>(SparseSpatialMap<T, bool> map, params int[] fixedDimensions) where T : IPoint, new()
		{
			StringBuilder sb = new StringBuilder();

			for (int y = map.Bounds.GetMin(1); y < map.Bounds.GetMax(1); y++)
			{
				for (int x = map.Bounds.GetMin(0); x < map.Bounds.GetMax(0); x++)
				{
					object[] args = new[] { x, y }.Concat(fixedDimensions).Cast<object>().ToArray();
					var p = (T)Activator.CreateInstance(typeof(T), args);
					if (map.ContainsKey(p))
					{
						sb.Append(map[p] ? '#' : '.');
					}
					else
					{
						sb.Append('.');
					}
				}

				sb.AppendLine();
			}

			Console.WriteLine(sb);
		}


		private struct Point4 : IPoint
		{
			public int Dimensions => 4;

			public int X;
			public int Y;
			public int Z;
			public int W;

			public Point4(int x, int y, int z, int w)
			{
				X = x;
				Y = y;
				Z = z;
				W = w;
			}

			public IEnumerable<IPoint> GetNeighbors()
			{
				for (int w = -1; w <= 1; w++)
				{
					for (int z = -1; z <= 1; z++)
					{
						for (int y = -1; y <= 1; y++)
						{
							for (int x = -1; x <= 1; x++)
							{
								if (x == 0 && y == 0 && z == 0 && w == 0)
									continue;

								yield return new Point4(this.X + x, this.Y + y, this.Z + z, this.W + w);
							}
						}
					}
				}
			}

			public int GetValue(int dimension)
			{
				return dimension switch
				{
					0 => X,
					1 => Y,
					2 => Z,
					3 => W,
					_ => throw new IndexOutOfRangeException()
				};
			}
		}
	}
}