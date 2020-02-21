using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootahDaAsteroid
{
	public class Program
	{
		private static int _ox = 8;
		private static int _oy = 3;

		public static async Task Main(string[] args)
		{
			var map = await LoadProgramAsync();

			ReprintAsteroidField(map);

			var list = MapAsteroids(map).GroupBy(e => e.angle)
				.OrderBy(e => e.Key)
				.Select(e => new Queue<(int x, int y)>(e.OrderBy(f => f.distance).Select(f => (f.x, f.y))))
				.ToList();

			int delay = 100;
			ReprintAsteroidField(map);
			await Task.Delay(delay);
			Console.ReadKey(true);

			int i = 0;
			int t = 0;
			while (list.Sum(e => e.Count) > 0)
			{
				if (list[i].Count > 0)
				{
					var item = list[i].Dequeue();

					map[item.y, item.x] = 0;
					t++;

					ReprintPoint(item.x, item.y, '.');
					await Task.Delay(delay);
					Console.ReadKey(true);
				}

				i = (i + 1) % list.Count;
				if (t == 199)
				{
					break;
				}
			}

			ReprintAsteroidField(map);
			Console.ReadKey(true);
		}

		private static void ReprintAsteroidField(int[,] map)
		{
			var sb = new StringBuilder();
			for (int y = 0; y < map.GetLength(0); y++)
			{
				for (int x = 0; x < map.GetLength(1); x++)
				{
					sb.Append(map[y, x] > 0 ? 'x' : '.');
				}

				sb.AppendLine();
			}

			Console.Clear();
			Console.WriteLine(sb);
		}

		private static void ReprintPoint(int x, int y, char c)
		{
			Console.CursorLeft = x;
			Console.CursorTop = y;
			Console.Write(c);
		}

		private static async Task<int[,]> LoadProgramAsync()
		{
			string[] lines = await File.ReadAllLinesAsync("./Assets/input.txt");

			int[,] map = new int[lines.Length, lines[0].Length];

			for (int y = 0; y < map.GetLength(0); y++)
			{
				for (int x = 0; x < map.GetLength(1); x++)
				{
					map[y, x] = lines[y][x] == '#' ? 1 : 0;
				}
			}

			return map;
		}

		private static double GetAngle(double ax, double ay, double bx, double by, double ox, double oy)
		{
			var angle = Math.Atan2(by - oy, bx - ox);
			angle += (Math.PI / 2d);

			if (angle < 0)
				angle += Math.PI * 2;

			return angle;
		}

		private static double GetDistance(double ax, double ay, double ox, double oy)
		{
			double x = ax - ox;
			double y = ay - oy;

			return Math.Sqrt((x * x) + (y * y));
		}

		private static List<(int x, int y, double angle, double distance)> MapAsteroids(int[,] map)
		{
			List<(int, int, double, double)> result = new List<(int, int, double, double)>();

			for (int y = 0; y < map.GetLength(0); y++)
			{
				for (int x = 0; x < map.GetLength(1); x++)
				{
					if (map[y, x] == 1)
						result.Add((x, y, GetAngle(_ox, _oy - 1, x, y, _ox, _oy), GetDistance(x, y, _ox, _oy)));
				}
			}

			return result;
		}
	}
}
