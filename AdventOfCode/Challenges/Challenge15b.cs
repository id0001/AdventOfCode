using AdventOfCode.DataStructures;
using AdventOfCode.IntCode.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge15b : IChallenge
	{
		public string Id => "15b";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge15.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var droid = new RepairDroid(program);

			droid.MapLayout();

			Point startPoint = droid.MappedSpace.FirstOrDefault(e => e.Value == 2).Key;

			var fillMap = new HashSet<Point>();
			var lockedMap = new HashSet<Point>();

			fillMap.Add(startPoint);
			droid.MappedSpace[startPoint] = 3;

			int minutes = 0;
			while (fillMap.Any())
			{
				bool filledASpace = false;
				var points = fillMap.ToHashSet();

				foreach (var point in points)
				{
					fillMap.Remove(point);
					lockedMap.Add(point);

					for (int y = point.Y - 1; y <= point.Y + 1; y++)
					{
						for (int x = point.X - 1; x <= point.X + 1; x++)
						{
							if (x == point.X ^ y == point.Y)
							{
								Point neighbor = new Point(x, y);

								if (!droid.MappedSpace.ContainsKey(neighbor) || droid.MappedSpace[neighbor] == 0)
									continue;

								if (!points.Contains(neighbor) && !lockedMap.Contains(neighbor))
								{
									fillMap.Add(neighbor);
									droid.MappedSpace[neighbor] = 3;
									filledASpace = true;
								}
							}
						}
					}
				}

				if (filledASpace)
					minutes++;
			}

			return minutes.ToString();
		}

		public void PrintSpace(IDictionary<Point, int> mappedSpace, int minutes)
		{
			var points = mappedSpace.Keys.ToHashSet();

			if (points.Count == 0)
				return;

			int xlo = points.Min(p => p.X);
			int xhi = points.Max(p => p.X);
			int ylo = points.Min(p => p.Y);
			int yhi = points.Max(p => p.Y);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"minutes: {minutes}                       ");

			for (int y = ylo; y <= yhi; y++)
			{
				for (int x = xlo; x <= xhi; x++)
				{
					if (mappedSpace.TryGetValue(new Point(x, y), out int value))
					{
						sb.Append(value switch
						{
							0 => '#',
							1 => '.',
							2 => '.',
							3 => 'O',
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
	}
}
