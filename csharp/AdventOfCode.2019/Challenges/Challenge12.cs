using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2019.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader InputReader)
{
    private Point3 _callisto;
    private Point3 _europe;
    private Point3 _ganymede;
    private Point3 _io;

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await InputReader.ReadLinesAsync(12).ToArrayAsync();
        _io = ConvertLineToVector(lines[0]);
        _europe = ConvertLineToVector(lines[1]);
        _ganymede = ConvertLineToVector(lines[2]);
        _callisto = ConvertLineToVector(lines[3]);
    }

    [Part1]
    public string Part1()
    {
        var moons = new[]
        {
            new Moon(_io),
            new Moon(_europe),
            new Moon(_ganymede),
            new Moon(_callisto)
        };

        for (var step = 0; step < 1000; step++)
        {
            for (var m1 = 0; m1 < 3; m1++)
            for (var m2 = m1; m2 < 4; m2++)
                Moon.ApplyGravity(moons[m1], moons[m2]);

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
            new Moon(_io),
            new Moon(_europe),
            new Moon(_ganymede),
            new Moon(_callisto)
        };

        var steps = 0;
        var xCycle = 0;
        var yCycle = 0;
        var zCycle = 0;

        do
        {
            for (var m1 = 0; m1 < 3; m1++)
            for (var m2 = m1 + 1; m2 < 4; m2++)
                Moon.ApplyGravity(moons[m1], moons[m2]);

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
        } while (xCycle == 0 || yCycle == 0 || zCycle == 0);

        // Find the least common multiplier between xCycle, yCycle and zCycle.
        var lcm = Euclid.LeastCommonMultiple(zCycle, xCycle, yCycle);

        return lcm.ToString();
    }

    private static Point3 ConvertLineToVector(string line)
    {
        var pattern = new Regex(@"^<x=(-?\d+), y=(-?\d+), z=(-?\d+)>$");

        var match = pattern.Match(line);
        if (!match.Success)
            throw new InvalidOperationException("Unable to read line.");

        var x = int.Parse(match.Groups[1].Value);
        var y = int.Parse(match.Groups[2].Value);
        var z = int.Parse(match.Groups[3].Value);

        return new Point3(x, y, z);
    }

    private class Moon(Point3 position)
    {
        public Point3 InitialPosition { get; } = position;

        public Point3 Position { get; set; } = position;

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
            var va = a.Velocity;
            var vb = b.Velocity;

            if (a.Position.X != b.Position.X)
            {
                if (a.Position.X < b.Position.X)
                {
                    va = va with {X = va.X + 1};
                    vb = vb with {X = vb.X - 1};
                }
                else
                {
                    va = va with {X = va.X - 1};
                    vb = vb with {X = vb.X + 1};
                }
            }

            if (a.Position.Y != b.Position.Y)
            {
                if (a.Position.Y < b.Position.Y)
                {
                    va = va with {Y = va.Y + 1};
                    vb = vb with {Y = vb.Y - 1};
                }
                else
                {
                    va = va with {Y = va.Y - 1};
                    vb = vb with {Y = vb.Y + 1};
                }
            }

            if (a.Position.Z != b.Position.Z)
            {
                if (a.Position.Z < b.Position.Z)
                {
                    va = va with {Z = va.Z + 1};
                    vb = vb with {Z = vb.Z - 1};
                }
                else
                {
                    va = va with {Z = va.Z - 1};
                    vb = vb with {Z = vb.Z + 1};
                }
            }

            a.Velocity = va;
            b.Velocity = vb;
        }
    }
}