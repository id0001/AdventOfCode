using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            Console.WriteLine($"{best.Key}: {best.Value.Count}");
            return best.Value.Count.ToString();
        }

        private IList<Vector> GetAsteroids(string[] input)
        {
            var list = new List<Vector>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        list.Add(new Vector(x, y));
                    }
                }
            }

            return list;
        }

        private IDictionary<Vector, IList<Vector>> CalculateLineOfSight(IList<Vector> asteroids)
        {
            var result = new Dictionary<Vector, IList<Vector>>();

            foreach (var p0 in asteroids)
            {
                result.Add(p0, new List<Vector>());
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

        private bool CanSee(IList<Vector> asteroids, Vector p0, Vector p1)
        {
            foreach (var p2 in asteroids)
            {
                if (p2 == p0 || p2 == p1) continue;

                var a = p2 - p0;
                var b = p1 - p0;

                var v = Vector.Dot(a, b) / (a.Length() * b.Length());
                var angle = v <= -1 ? Math.PI : v >= 1 ? 0 : Math.Acos(v);

                if (Math.Abs(angle) < 0.001d && a.Length() < b.Length()) return false;
            }

            return true;
        }
    }
}
