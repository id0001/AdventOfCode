using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(19)]
    public class Challenge19
    {
        private readonly IInputReader inputReader;
        List<Scanner> scanners = new List<Scanner>();

        public Challenge19(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            ISet<Point3> temp = new HashSet<Point3>();
            await foreach (var line in inputReader.ReadLinesAsync(19))
            {
                if (string.IsNullOrEmpty(line))
                {
                    scanners.Add(new Scanner(temp, 0, Point3.Zero));
                    temp = new HashSet<Point3>();
                    continue;
                }

                if (line.StartsWith("---"))
                    continue;

                string[] split = line.Split(',');
                var point = new Point3(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
                temp.Add(point);
            }

            scanners.Add(new Scanner(temp, 0, Point3.Zero));
        }

        [Part1]
        public string Part1()
        {
            return OrientScanners(scanners)
                .SelectMany(scanner => scanner.TransformedBeacons)
                .Distinct()
                .Count()
                .ToString();
        }

        [Part2]
        public string Part2()
        {
            var relativeScanners = OrientScanners(scanners);

            int max = int.MinValue;
            foreach(var scanner0 in relativeScanners)
            {
                foreach(var scanner1 in relativeScanners)
                {
                    if (scanner0 == scanner1)
                        continue;

                    int md = Math.Abs(scanner0.Origin.X - scanner1.Origin.X) + Math.Abs(scanner0.Origin.Y - scanner1.Origin.Y) + Math.Abs(scanner0.Origin.Z - scanner1.Origin.Z);
                    if (md > max)
                        max = md;
                }
            }

            return max.ToString();
        }

        private ISet<Scanner> OrientScanners(List<Scanner> scanners)
        {
            var queue = new Queue<Scanner>();
            var result = new HashSet<Scanner>();

            result.Add(scanners[0]);
            queue.Enqueue(scanners[0]);
            scanners.Remove(scanners[0]);

            while (queue.Count > 0)
            {
                var scanner0 = queue.Dequeue();
                foreach (var scanner1 in scanners.ToArray())
                {
                    var oriented = FindOrientation(scanner0, scanner1);
                    if (oriented is object)
                    {
                        result.Add(oriented);
                        queue.Enqueue(oriented);
                        scanners.Remove(scanner1);
                    }
                }
            }

            return result;
        }

        private Scanner FindOrientation(Scanner scanner0, Scanner scanner1)
        {
            var beacons0 = scanner0.TransformedBeacons;

            foreach (var beacon0 in beacons0)
            {
                for (int r = 0; r < 24; r++)
                {
                    scanner1 = scanner1.Rotate();

                    foreach (var beacon1 in scanner1.TransformedBeacons)
                    {
                        var delta = beacon0 - beacon1;

                        var overlappedBeacons = scanner1.Move(delta).TransformedBeacons.Intersect(beacons0).ToHashSet();
                        if (overlappedBeacons.Count >= 12)
                        {
                            return scanner1.Move(delta);
                        }
                    }
                }
            }

            return null;
        }

        private class Scanner
        {
            public Scanner(ISet<Point3> beacons, int rotation, Point3 origin)
            {
                Beacons = beacons;
                Rotation = rotation;
                Origin = origin;
            }

            public ISet<Point3> Beacons { get; }

            public int Rotation { get; }

            public Point3 Origin { get; }

            public Scanner Move(Point3 offset) => new Scanner(Beacons, Rotation, offset);

            public Scanner Rotate() => new Scanner(Beacons, (Rotation + 1) % 24, Origin);

            public ISet<Point3> TransformedBeacons => Beacons.Select(p => Origin + Rotate(p)).ToHashSet();

            private Point3 Rotate(Point3 p)
            {
                switch (Rotation % 6)
                {
                    case 0:
                        p = new Point3(p.X, p.Y, p.Z);
                        break;
                    case 1:
                        p = new Point3(-p.X, p.Y, -p.Z);
                        break;
                    case 2:
                        p = new Point3(p.Y, -p.X, p.Z);
                        break;
                    case 3:
                        p = new Point3(-p.Y, p.X, p.Z);
                        break;
                    case 4:
                        p = new Point3(p.Z, p.Y, -p.X);
                        break;
                    case 5:
                        p = new Point3(-p.Z, p.Y, p.X);
                        break;
                }

                switch (Rotation / 6 % 4)
                {
                    case 0:
                        return new Point3(p.X, p.Y, p.Z);
                    case 1:
                        return new Point3(p.X, -p.Z, p.Y);
                    case 2:
                        return new Point3(p.X, -p.Y, -p.Z);
                    case 3:
                        return new Point3(p.X, p.Z, -p.Y);
                }

                throw new InvalidOperationException();
            }
        }
    }
}
