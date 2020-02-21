using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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

			var calc1 = Calculate(asteroids, center);
			var calc2 = Calculate2(asteroids, center);

			var filtered = calc1.Zip(calc2).Where(p => !(p.First.Point == p.Second.Point && p.First.Angle == p.Second.Angle && p.First.Length == p.Second.Length)).ToList();

			var l1 = calc1.GroupBy(e => e.Angle, cmp).ToDictionary(e => e.Key, e => e.ToList());
			var l2 = calc2.GroupBy(e => e.Angle, cmp).ToDictionary(e => e.Key, e => e.ToList());

			var list = Calculate(asteroids, center).GroupBy(e => e.Angle, cmp)
				.OrderBy(e => e.Key)
				.Select(e => new List<Point>(e.OrderBy(f => f.Length).Select(f => f.Point)))
				.ToList();

			var list2 = Calculate2(asteroids, center).GroupBy(e => e.Angle, cmp)
				.OrderBy(e => e.Key)
				.Select(e => new List<Point>(e.OrderBy(f => f.Length).Select(f => f.Point)))
				.ToList();

			var enumerated = EnumerateSortedAsteroids(list).ToList();
			var enumerated2 = EnumerateSortedAsteroids(list2).ToList();
			var p = enumerated.Skip(199).First();
			var p2 = enumerated2.Skip(199).First();

			Console.WriteLine($"{p} -> {(p.X * 100) + p.Y}");
			Console.WriteLine($"{p2} -> {(p2.X * 100) + p2.Y}");
			return string.Empty;
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
			Point start = new Point(center.X, 0);
			foreach (var asteroid in asteroids)
			{
				double angle = GetAngle(start, asteroid, center);
				double length = GetLength(asteroid, center);
				result.Add((asteroid, angle, length));
			}

			return result;
		}

		private IList<(Point Point, double Angle, double Length)> Calculate2(IList<Point> asteroids, Point center)
		{
			var result = new List<(Point, double, double)>();
			foreach (var asteroid in asteroids)
			{
				double angle = GetAngle2(asteroid, center);
				double length = GetLength(asteroid, center);
				result.Add((asteroid, angle, length));
			}

			return result;
		}

		private double GetAngle(Point a, Point b, Point origin)
		{
			return GetAngle(a.X, a.Y, b.X, b.Y, origin.X, origin.Y);
		}

		private double GetAngle2(Point b, Point origin)
		{
			return GetAngle2(b.X, b.Y, origin.X, origin.Y);
		}

		private double GetLength(Point a, Point origin)
		{
			double x = a.X - origin.X;
			double y = a.Y - origin.Y;

			return Math.Sqrt((x * x) + (y * y));
		}

		private double GetAngle(double ax, double ay, double bx, double by, double ox, double oy)
		{
			double lax = ax - ox;
			double lay = ay - oy;
			double lbx = bx - ox;
			double lby = by - oy;
			double maga = Math.Sqrt((lax * lax) + (lay * lay));
			double magb = Math.Sqrt((lbx * lbx) + (lby * lby));

			double cross = (lax * lby) - (lay * lbx);

			double dot = ((lax * lbx) + (lay * lby)) / (maga * magb);
			double angle = Math.Acos(Math.Max(-1d, Math.Min(dot, 1d)));
			angle *= Math.Sign(cross != 0 ? cross : 1);
			angle = angle < 0 ? 2 * Math.PI + angle : angle;

			return angle;
		}

		private double GetAngle2(double bx, double by, double ox, double oy)
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
