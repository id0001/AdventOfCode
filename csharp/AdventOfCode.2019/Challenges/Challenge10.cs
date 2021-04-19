using AdventOfCode.Lib;
using AdventOfCode.Lib.Comparers;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(10)]
	public class Challenge10
	{
		private readonly IInputReader inputReader;
		private readonly DoubleComparer doubleComparer;

		private List<Point2> asteroids;

		public Challenge10(IInputReader inputReader)
		{
			this.inputReader = inputReader;
			this.doubleComparer = new DoubleComparer();
		}

		[Setup]
		public async Task SetupAsync()
		{
			asteroids = new List<Point2>();
			int y = 0;
			await foreach (var line in inputReader.ReadLinesAsync(10))
			{
				for (int x = 0; x < line.Length; x++)
				{
					if (line[x] == '#')
						asteroids.Add(new Point2(x, y));
				}

				y++;
			}
		}

		[Part1]
		public string Part1()
		{
			var visibleMap = asteroids.ToDictionary(kv => kv, kv => 0);

			for (int i = 0; i < asteroids.Count; i++)
			{
				var origin = asteroids[i];
				for (int j = i + 1; j < asteroids.Count; j++)
				{
					var target = asteroids[j];

					var d0 = Distance(origin, target);
					var a0 = Angle(origin, target);

					bool blocked = false;
					for (int k = 0; k < asteroids.Count; k++)
					{
						if (k == i || k == j)
							continue;

						var test = asteroids[k];
						var a1 = Angle(origin, test);

						if (doubleComparer.Equals(a0,a1) && Distance(origin, test) < d0)
						{
							// Equal angle and distance is lower.
							// Target is blocked.
							blocked = true;
							break;
						}
					}

					if (!blocked)
					{
						visibleMap[origin]++;
						visibleMap[target]++;
					}
				}
			}

			var best = visibleMap.OrderByDescending(kv => kv.Value).First();

			return best.Value.ToString();
		}

		[Part2]
		public string Part2()
		{
			var center = new Point2(20, 18);
			asteroids.Remove(center);

			var list = asteroids.Select(p => new { Point = p, Angle = Angle(center, p), Distance = Distance(center, p) })
				.GroupBy(t => t.Angle, doubleComparer)
				.OrderBy(g => g.Key)
				.Select(g => g.OrderBy(x => x.Distance).Select(x => x.Point).ToList())
				.ToList();

			var p = EnumerateSortedAsteroids(list)
				.Skip(199)
				.First();

			return (p.X * 100 + p.Y).ToString();
		}

		private IEnumerable<Point2> EnumerateSortedAsteroids(List<List<Point2>> list)
		{
			int length = list.Sum(x => x.Count);
			int[] indices = new int[list.Count];
			int i = 0;
			for (int k = 0; k < length; k++)
			{
				if (indices[i] < list[i].Count)
				{
					var p = list[i][indices[i]];
					yield return p;
					indices[i]++;
				}

				i = (i + 1) % list.Count;
			}
		}

		private double Distance(Point2 origin, Point2 target) => Point2.DistanceSquared(origin, target);

		private double Angle(Point2 origin, Point2 target)
		{
			var angle = Vector2.AngleOnCircle(target.ToVector2(), origin.ToVector2());
			angle += Math.PI / 2d;
			return angle % (Math.PI * 2);
		}
	}
}
