using AdventOfCode.Lib;
using AdventOfCode.Lib.Comparers;
using AdventOfCode.Lib.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
	[Challenge(12)]
	public class Challenge12
	{
		private readonly IInputReader inputReader;
		private Point3 io;
		private Point3 europe;
		private Point3 ganymede;
		private Point3 callisto;

		public Challenge12(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			var lines = await inputReader.ReadLinesAsync(12).ToArrayAsync();
			io = ConvertLineToVector(lines[0]);
			europe = ConvertLineToVector(lines[1]);
			ganymede = ConvertLineToVector(lines[2]);
			callisto = ConvertLineToVector(lines[3]);
		}

		[Part1]
		public string Part1()
		{
			var moons = new[]
			{
				new Moon(io),
				new Moon(europe),
				new Moon(ganymede),
				new Moon(callisto)
			};

			for (int step = 0; step < 1000; step++)
			{
				for (int m1 = 0; m1 < 3; m1++)
				{
					for (int m2 = m1; m2 < 4; m2++)
					{
						Moon.ApplyGravity(moons[m1], moons[m2]);
					}
				}

				foreach (var moon in moons)
					moon.ApplyVelocity();
			}

			return moons.Sum(m => m.TotalEnergy).ToString();
		}

		[Part2]
		public string Part2()
		{
			var moons = new[]
			{
				new Moon(io),
				new Moon(europe),
				new Moon(ganymede),
				new Moon(callisto)
			};

			int steps = 0;
			int xCycle = 0;
			int yCycle = 0;
			int zCycle = 0;

			do
			{
				for (int m1 = 0; m1 < 3; m1++)
				{
					for (int m2 = m1 + 1; m2 < 4; m2++)
					{
						Moon.ApplyGravity(moons[m1], moons[m2]);
					}
				}

				foreach (var moon in moons)
					moon.ApplyVelocity();

				steps++;

				// if state of x on all moons is in its initial state, set xCycle to the amount of steps.
				if (moons.All(m => m.Velocity.X == 0 && m.Position.X == m.InitialPosition.X) && xCycle == 0)
					xCycle = steps;

				// if state of y on all moons is in its initial state, set yCycle to the amount of steps.
				if (moons.All(m => m.Velocity.Y == 0 && m.Position.Y == m.InitialPosition.Y) && yCycle == 0)
					yCycle = steps;

				// if state of z on all moons is in its initial state, set zCycle to the amount of steps.
				if (moons.All(m => m.Velocity.Z == 0 && m.Position.Z == m.InitialPosition.Z) && zCycle == 0)
					zCycle = steps;
			}
			while (xCycle == 0 || yCycle == 0 || zCycle == 0);

			// Find the least common multiplier between xCycle, yCycle and zCycle.
			long lcm = MathEx.Lcm(MathEx.Lcm(zCycle, xCycle), yCycle);

			return lcm.ToString();
		}

		private Point3 ConvertLineToVector(string line)
		{
			var pattern = new Regex(@"^<x=(-?\d+), y=(-?\d+), z=(-?\d+)>$");

			var match = pattern.Match(line);
			if (!match.Success)
				throw new InvalidOperationException("Unable to read line.");

			int x = int.Parse(match.Groups[1].Value);
			int y = int.Parse(match.Groups[2].Value);
			int z = int.Parse(match.Groups[3].Value);

			return new Point3(x, y, z);
		}

		private class Moon
		{
			public Moon(Point3 position)
			{
				Position = position;
				InitialPosition = position;
			}

			public Point3 InitialPosition { get; }

			public Point3 Position { get; set; }

			public Point3 Velocity { get; set; } = Point3.Zero;

			public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

			public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

			public int TotalEnergy => PotentialEnergy * KineticEnergy;

			public void ApplyVelocity()
			{
				Position += Velocity;
			}

			public static void ApplyGravity(Moon a, Moon b)
			{
				Point3 va = a.Velocity;
				Point3 vb = b.Velocity;

				if (a.Position.X != b.Position.X)
				{
					(va.X, vb.X) = a.Position.X < b.Position.X
						? (va.X + 1, vb.X - 1)
						: (va.X - 1, vb.X + 1);
				}

				if (a.Position.Y != b.Position.Y)
				{
					(va.Y, vb.Y) = a.Position.Y < b.Position.Y
						? (va.Y + 1, vb.Y - 1)
						: (va.Y - 1, vb.Y + 1);
				}

				if (a.Position.Z != b.Position.Z)
				{
					(va.Z, vb.Z) = a.Position.Z < b.Position.Z
						? (va.Z + 1, vb.Z - 1)
						: (va.Z - 1, vb.Z + 1);
				}

				a.Velocity = va;
				b.Velocity = vb;
			}
		}
	}
}
