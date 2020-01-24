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
			string[] input = await File.ReadAllLinesAsync("Assets/TestInput.txt");

			var asteroids = GetAsteroids(input);
			var center = new Point(11, 13);
			asteroids.Remove(center);

			var list = Calculate(asteroids, center);

			var doubleComparer = new DoubleComparer();

			var sorted = new SortedDictionary<double, SortedList<double, Point>>(list.GroupBy(a => a.Angle, doubleComparer)
				.ToDictionary(kv => kv.Key, kv => new SortedList<double, Point>(kv.ToDictionary(x => x.Length, x => x.Point, doubleComparer), doubleComparer)));

			var enumerated = EnumerateSortedAsteroids(sorted.Select(e=> e.Value).Select(e=>e.Values).ToList());
			var p = enumerated.Skip(199).First();

			return string.Empty;
		}

		private IEnumerable<Point> EnumerateSortedAsteroids(IList<IList<Point>> list)
		{
			int length = list.Sum(x => x.Count);
			int[] indices = new int[list.Count];
			for (int i = 0; i < length; i++)
			{
				int j = i % list.Count;
				if (indices[j] >= list[j].Count)
					continue;

				yield return list[j][indices[j]];
			};
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
			Point start = new Point(center.X, center.Y - 1);
			foreach (var asteroid in asteroids)
			{
				double angle = GetAngle(start, asteroid, center);
				double length = GetLength(asteroid, center);
				result.Add((asteroid, angle, length));
			}

			return result;
		}

		private double GetAngle(Point a, Point b, Point origin)
		{
			return GetAngle(a.X, a.Y, b.X, b.Y, origin.X, origin.Y);
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
			return angle < 0 ? 2 * Math.PI + angle : angle;
		}

		private class DoubleComparer : IComparer<double>, IEqualityComparer<double>
		{
			public int Compare([AllowNull] double x, [AllowNull] double y) => Equals(x, y) ? 0 : x < y ? -1 : 1;

			public bool Equals([AllowNull] double x, [AllowNull] double y) => Math.Abs(x - y) < 0.0001d;

			public int GetHashCode([DisallowNull] double obj) => HashCode.Combine(obj);
		}
	}
}
