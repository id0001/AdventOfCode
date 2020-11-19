using AdventOfCode.DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public class Challenge10b : IChallenge
	{
		public string Id => "10b";

		public async Task<string> RunAsync()
		{
			string[] input = await File.ReadAllLinesAsync("Assets/Challenge10.txt");

			var asteroids = GetAsteroids(input);
			var center = new Point(20, 18);
			asteroids.Remove(center);

			var cmp = new AsteroidComparer();

			var list = Calculate(asteroids, center).GroupBy(e => e.Angle, cmp)
				.OrderBy(e => e.Key)
				.Select(e => new List<Point>(e.OrderBy(f => f.Length).Select(f => f.Point)))
				.ToList();

			var enumerated = EnumerateSortedAsteroids(list).ToList();
			var p = enumerated.Skip(199).First();

			return ((p.X * 100) + p.Y).ToString();
		}

		private IEnumerable<Point> EnumerateSortedAsteroids(List<List<Point>> list)
		{
			int length = list.Sum(x => x.Count);
			int amount = length;
			int[] indices = new int[list.Count];

			int i = 0;
			while (amount > 0)
			{
				if (indices[i] < list[i].Count)
				{
					var p = list[i][indices[i]];
					amount--;
					yield return p;
				}

				i = (i + 1) % list.Count;
			}
		}

		private IList<Point> GetAsteroids(string[] input)
		{
			var list = new List<Point>();
			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '#')
					{
						list.Add(new Point(x, y));
					}
				}
			}

			return list;
		}

		private IList<(Point Point, double Angle, double Length)> Calculate(IList<Point> asteroids, Point center)
		{
			var result = new List<(Point, double, double)>();
			foreach (var asteroid in asteroids)
			{
				double angle = GetAngle(asteroid, center);
				double length = GetLength(asteroid, center);
				result.Add((asteroid, angle, length));
			}

			return result;
		}

		private double GetAngle(Point b, Point origin)
		{
			return GetAngle(b.X, b.Y, origin.X, origin.Y);
		}

		private double GetLength(Point a, Point origin)
		{
			double x = a.X - origin.X;
			double y = a.Y - origin.Y;

			return Math.Sqrt((x * x) + (y * y));
		}

		private double GetAngle(double bx, double by, double ox, double oy)
		{
			var angle2 = Math.Atan2(by - oy, bx - ox);
			angle2 += (Math.PI / 2d);

			if (angle2 < 0)
				angle2 += Math.PI * 2;

			return angle2;
		}

		private class AsteroidComparer : EqualityComparer<double>
		{
			public override bool Equals([AllowNull] double x, [AllowNull] double y) => Math.Abs(x - y) < 0.000001d;

			public override int GetHashCode([DisallowNull] double obj) => obj.GetHashCode();
		}
	}
}
