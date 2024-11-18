using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(23)]
public class Challenge23(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var bots = await inputReader.ParseLinesAsync(23, ParseLine).ToListAsync();

        var strongest = bots.MaxBy(x => x.Radius)!;
        return bots.Count(b => strongest.IsInRange(b.Center)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var bots = await inputReader.ParseLinesAsync(23, ParseLine).ToListAsync();

        var minX = bots.Min(b => b.Center.X);
        var maxX = bots.Max(b => b.Center.X);
        var minY = bots.Min(b => b.Center.Y);
        var maxY = bots.Max(b => b.Center.Y);
        var minZ = bots.Min(b => b.Center.Z);
        var maxZ = bots.Max(b => b.Center.Z);

        var pq = new PriorityQueue<Cube, Priority>(new PriorityComparer());

        // Round to the nearest power of 2 to make subdividing work better
        var cube = ToNearestPowerOf2(new Cube(minX, minY, minZ, maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1));

        pq.Enqueue(cube, new Priority(1000, Point3.ManhattanDistance(Point3.Zero, cube.Position), cube.Width));

        while (pq.Count > 0)
        {
            var current = pq.Dequeue();

            if (current.Width == 1)
                return Point3.ManhattanDistance(Point3.Zero, current.Position).ToString();

            foreach (var subCube in current.Subdivide(2))
            {
                var c = bots.Count(b => Intersects(subCube, b));
                var d = Point3.ManhattanDistance(Point3.Zero, subCube.Position);
                pq.Enqueue(subCube, new Priority(c, d, subCube.Width));
            }
        }

        return string.Empty;
    }

    private static Cube ToNearestPowerOf2(Cube cube)
    {
        var largest = new[] {cube.Width, cube.Height, cube.Depth}.Max();

        var s = 0;
        var p = 0;
        while (s < largest)
        {
            s = (int) Math.Pow(2, p);
            p++;
        }

        return cube with {Width = s, Height = s, Depth = s};
    }

    private static bool Intersects(Cube cube, Nanobot bot)
    {
        // Make sure the coordinate is 1 less than the width/height/depth to be 'in' the cube
        var cubeVerts = new[]
        {
            new Point3(cube.Left, cube.Top, cube.Front),
            new Point3(cube.Right - 1, cube.Top, cube.Front),
            new Point3(cube.Right - 1, cube.Bottom - 1, cube.Front),
            new Point3(cube.Left, cube.Bottom - 1, cube.Front),
            new Point3(cube.Left, cube.Top, cube.Back - 1),
            new Point3(cube.Right - 1, cube.Top, cube.Back - 1),
            new Point3(cube.Right - 1, cube.Bottom - 1, cube.Back - 1),
            new Point3(cube.Left, cube.Bottom - 1, cube.Back - 1)
        };

        return bot.Vertices.Any(cube.Contains) || cubeVerts.Any(bot.IsInRange);
    }

    private static Nanobot ParseLine(string line)
        => line.Extract<int, int, int, int>(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)").Into(res =>
            new Nanobot(new Point3(res.First, res.Second, res.Third), res.Fourth));

    private record Nanobot(Point3 Center, int Radius)
    {
        public Point3[] Vertices =>
        [
            new(Center.X, Center.Y, Center.Z),
            new(Center.X - Radius, Center.Y, Center.Z),
            new(Center.X + Radius, Center.Y, Center.Z),
            new(Center.X, Center.Y - Radius, Center.Z),
            new(Center.X, Center.Y + Radius, Center.Z),
            new(Center.X, Center.Y, Center.Z - Radius),
            new(Center.X, Center.Y, Center.Z + Radius)
        ];

        public bool IsInRange(Point3 p) => GetDistance(p) <= Radius;

        private long GetDistance(Point3 p) => Point3.ManhattanDistance(p, Center);
    }

    private record Priority(int BotsInRange, int DistanceToOrigin, long Size);

    private class PriorityComparer : IComparer<Priority>
    {
        public int Compare(Priority? x, Priority? y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            if (x.BotsInRange != y.BotsInRange)
                return y.BotsInRange - x.BotsInRange;

            if (x.DistanceToOrigin != y.DistanceToOrigin)
                return x.DistanceToOrigin - y.DistanceToOrigin;

            return (int) (x.Size - y.Size);
        }
    }
}