using AdventOfCode.DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
    public class Challenge10a : IChallenge
    {
        public string Id => "10a";

        public async Task<string> RunAsync()
        {
            string[] input = await File.ReadAllLinesAsync("Assets/Challenge10.txt");

            var asteroids = GetAsteroids(input);
            var lineOfSight = CalculateLineOfSight(asteroids);

            var best = lineOfSight.OrderByDescending(e => e.Value.Count).First();
            return best.Value.Count.ToString();
        }

        private IList<Vector2> GetAsteroids(string[] input)
        {
            var list = new List<Vector2>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        list.Add(new Vector2(x, y));
                    }
                }
            }

            return list;
        }

        private IDictionary<Vector2, IList<Vector2>> CalculateLineOfSight(IList<Vector2> asteroids)
        {
            var result = new Dictionary<Vector2, IList<Vector2>>();

            foreach (var p0 in asteroids)
            {
                result.Add(p0, new List<Vector2>());
                foreach (var p1 in asteroids)
                {
                    if (p0 == p1) continue;

                    if (CanSee(asteroids, p0, p1))
                    {
                        result[p0].Add(p1);
                    }
                }
            }

            return result;
        }

        private bool CanSee(IList<Vector2> asteroids, Vector2 p0, Vector2 p1)
        {
            foreach (var p2 in asteroids)
            {
                if (p2 == p0 || p2 == p1) continue;

				var angle = GetAngle(p1.X, p1.Y, p2.X, p2.Y, p0.X, p0.Y);

                if (Math.Abs(angle) < 0.001d && GetLengthSquared(p2,p0) < GetLengthSquared(p1,p0)) return false;
            }

            return true;
        }

		private double GetAngle(Point a, Point b, Point origin)
		{
			return GetAngle(a.X, a.Y, b.X, b.Y, origin.X, origin.Y);
		}

		private double GetLengthSquared(Vector2 a, Vector2 origin)
		{
			double x = a.X - origin.X;
			double y = a.Y - origin.Y;

			return (x * x) + (y * y);
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
	}
}
