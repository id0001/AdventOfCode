using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	internal class Challenge12b : IChallenge
	{
		public string Id => "12b";

		public async Task<string> RunAsync()
		{
			var lines = await File.ReadAllLinesAsync("Assets/Challenge12.txt");

			var v0 = ConvertLineToVector(lines[0]);
			var v1 = ConvertLineToVector(lines[1]);
			var v2 = ConvertLineToVector(lines[2]);
			var v3 = ConvertLineToVector(lines[3]);

			var moons = new[]
			{
				new Moon("io", v0),
				new Moon("europe", v1),
				new Moon("ganymede", v2),
				new Moon("callisto", v3)
			};


			int steps = 0;

			int xCycle = 0;
			int yCycle = 0;
			int zCycle = 0;

			do
			{
				for (int y = 0; y < 3; y++)
				{
					for (int x = y + 1; x < 4; x++)
					{
						Moon.ApplyGravity(moons[y], moons[x]);
					}
				}

				foreach (var moon in moons)
				{
					moon.ApplyVelocity();
				}

				steps++;

				if (moons.All(m => m.IsInitialStateX) && xCycle == 0)
				{
					xCycle = steps;
				}

				if (moons.All(m => m.IsInitialStateY) && yCycle == 0)
				{
					yCycle = steps;
				}

				if (moons.All(m => m.IsInitialStateZ) && zCycle == 0)
				{
					zCycle = steps;
				}

			} while (!(xCycle > 0 && yCycle > 0 && zCycle > 0));

			return Lcm(Lcm(zCycle, xCycle), yCycle).ToString();
		}

		private long Gcd(long a, long b)
		{
			if (b == 0)
				return a;

			return Gcd(b, a % b);
		}

		private long Lcm(long a, long b) => (a / Gcd(a, b)) * b;

		private Vector3 ConvertLineToVector(string line)
		{
			var pattern = new Regex(@"^<x=(-?\d+), y=(-?\d+), z=(-?\d+)>$");

			var match = pattern.Match(line);
			if (!match.Success)
				throw new InvalidOperationException("Unable to read line.");

			float x = float.Parse(match.Groups[1].Value);
			float y = float.Parse(match.Groups[2].Value);
			float z = float.Parse(match.Groups[3].Value);

			return new Vector3(x, y, z);
		}
	}
}
