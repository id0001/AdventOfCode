using AdventOfCode.DataStructures;
using AdventOfCode.IntCode.Core;
using AdventOfCode.Pathfinding;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AdventOfCode.IntCode.Devices
{
	internal class RepairDroid : BaseComputer
	{
		private static Random Rand = new Random();

		public RepairDroid(long[] program)
			: base()
		{
			Cpu.LoadProgram(program);
		}

		public IDictionary<Point, int> MappedSpace { get; set; } = new Dictionary<Point, int>();

		public Point CurrentLocation { get; set; }

		public void MapLayout()
		{
			var toDiscover = new Dictionary<Point, Point>();

			Point nextLocation = Point.Zero;
			CurrentLocation = Point.Zero;
			MappedSpace.Add(Point.Zero, 1);
			AddNeighboursToDiscoverySet(toDiscover, CurrentLocation);

			var moves = new Queue<Point>();

			void ProcessOutput(long output)
			{
				if (!MappedSpace.ContainsKey(nextLocation))
					MappedSpace.Add(nextLocation, (int)output);

				switch (output)
				{
					case 0: // Wall
						break;
					case 1: // Empty space
					case 2: // Finish
						CurrentLocation = nextLocation;
						AddNeighboursToDiscoverySet(toDiscover, CurrentLocation);
						break;
				}

				//PrintSpace(true);
				//Thread.Sleep(20);
			}

			void PerformAction()
			{
				if (moves.Any())
				{
					nextLocation = moves.Dequeue();
				}
				else if (toDiscover.Count == 0)
				{
					Cpu.Halt();
					return;
				}
				else
				{
					nextLocation = ChooseTarget(toDiscover.Keys, CurrentLocation);
					if (!NextTo(CurrentLocation, nextLocation))
					{
						if (!Dijkstra.TryPath(ConvertSpaceForPathFinding(), CurrentLocation, toDiscover[nextLocation], out Point[] path))
						{
							throw new InvalidOperationException("Could not find path.");
						}

						for (int i = 1; i < path.Length; i++)
						{
							moves.Enqueue(path[i]);
						}

						nextLocation = moves.Dequeue();
					}
					else
					{
						toDiscover.Remove(nextLocation);
					}
				}

				int move = GetMove(CurrentLocation, nextLocation);
				In.Write(move);
			}

			RunCpu(ProcessOutput, PerformAction);
		}

		public int CalculateShortestPathToDefect()
		{
			var defectLocation = MappedSpace.FirstOrDefault(e => e.Value == 2).Key;
			if (defectLocation == default)
				throw new InvalidOperationException(@"No defect location found");

			var path = Dijkstra.Path(ConvertSpaceForPathFinding(), Point.Zero, defectLocation);
			return path.Length - 1;
		}

		public void PrintSpace(bool printDroidLocation = false)
		{
			var points = MappedSpace.Keys.ToHashSet();

			if (points.Count == 0)
				return;

			int xlo = points.Min(p => p.X);
			int xhi = points.Max(p => p.X);
			int ylo = points.Min(p => p.Y);
			int yhi = points.Max(p => p.Y);

			StringBuilder sb = new StringBuilder();
			for (int y = ylo; y <= yhi; y++)
			{
				for (int x = xlo; x <= xhi; x++)
				{
					if (printDroidLocation && CurrentLocation == new Point(x, y))
					{
						sb.Append('D');
					}
					else if (MappedSpace.TryGetValue(new Point(x, y), out int value))
					{
						sb.Append(value switch
						{
							0 => '#',
							1 => '.',
							2 => 'X',
							3 => '*',
							_ => '!'
						}); ;
					}
					else
					{
						sb.Append(' ');
					}
				}

				sb.AppendLine();
			}

			Console.CursorTop = 0;
			Console.CursorLeft = 0;
			Console.WriteLine(sb);
		}

		private void RunCpu(Action<long> processOutput, Action performAction)
		{
			Cpu.Start();

			while (!Cpu.IsHalted)
			{
				Cpu.Next();

				// Read output
				foreach (long output in Out.ReadToEnd())
				{
					processOutput(output);
				}

				// Write input
				if (Cpu.State == ExecutionState.ActionRequired)
				{
					performAction();
				}
			}
		}

		private bool NextTo(Point a, Point b)
		{
			int y = Math.Abs(a.Y - b.Y);
			int x = Math.Abs(a.X - b.X);
			return (x == 1 && y == 0) || (y == 1 && x == 0);
		}

		private void AddNeighboursToDiscoverySet(IDictionary<Point, Point> discoverySet, Point source)
		{
			for (int y = source.Y - 1; y <= source.Y + 1; y++)
			{
				for (int x = source.X - 1; x <= source.X + 1; x++)
				{
					var p = new Point(x, y);

					if (x == source.X ^ y == source.Y && !MappedSpace.ContainsKey(p) && !discoverySet.ContainsKey(p))
					{
						discoverySet.Add(p, source);
					}
				}
			}
		}

		private int GetMove(Point source, Point target)
		{
			if (source == target)
				throw new ArgumentException("Location and target cannot be the same.");

			int y = target.Y - source.Y;
			int x = target.X - source.X;

			if (y < 0) return 1;
			if (y > 0) return 2;
			if (x < 0) return 3;
			if (x > 0) return 4;

			return 0;
		}

		private Point ChooseTarget(IEnumerable<Point> targets, Point source)
		{
			if (!targets.Any())
				throw new ArgumentException("Targets cannot be empty");

			return targets.First();
		}

		private IDictionary<Point, bool> ConvertSpaceForPathFinding() => MappedSpace.Select(e => new KeyValuePair<Point, bool>(e.Key, e.Value != 0)).ToDictionary(kv => kv.Key, kv => kv.Value);

		private class PriorityComparer : IComparer<(int Score, Point Vertex)>
		{
			public int Compare([AllowNull] (int Score, Point Vertex) x, [AllowNull] (int Score, Point Vertex) y)
			{
				return x.Score - y.Score;
			}
		}
	}
}
