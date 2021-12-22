using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(22)]
    public class Challenge22
    {
        private const string InputPattern = @"^(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)$";
        private readonly IInputReader inputReader;
        private List<Instruction> instructions;

        public Challenge22(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            instructions = new List<Instruction>();
            await foreach (var line in inputReader.ReadLinesAsync(22))
            {
                var match = Regex.Match(line, InputPattern);

                bool turnOn = match.Groups[1].Value == "on" ? true : false;
                int xFrom = int.Parse(match.Groups[2].Value);
                int xTo = int.Parse(match.Groups[3].Value);
                int yFrom = int.Parse(match.Groups[4].Value);
                int yTo = int.Parse(match.Groups[5].Value);
                int zFrom = int.Parse(match.Groups[6].Value);
                int zTo = int.Parse(match.Groups[7].Value);

                var cubeoid = new Cube(new Point3(xFrom, yFrom, zFrom), new Point3(xTo - xFrom + 1, yTo - yFrom + 1, zTo - zFrom + 1));
                instructions.Add(new Instruction(turnOn, cubeoid));
            }
        }

        [Part1]
        public string Part1()
        {
            var map = new SparseSpatialMap<Point3, bool>();

            var bounds = new Cube(-50, -50, -50, 101, 101, 101);

            foreach (var instruction in instructions)
            {
                if (!instruction.Cuboid.IntersectsWith(bounds))
                    break;

                foreach (var point in instruction.Cuboid.Points)
                {
                    map.AddOrUpdate(point, instruction.State);
                }
            }

            return map.Count(x => x.Value).ToString();
        }

        [Part2]
        public string Part2()
        {
            long onVolume = 0L;
            List<Cube> onCubes = new List<Cube>();
            List<Cube> offCubes = new List<Cube>();

            foreach (var instruction in instructions)
            {
                var onIntersections = onCubes.Where(c => c.IntersectsWith(instruction.Cuboid)).Select(c => Cube.Intersect(c, instruction.Cuboid)).ToList();
                var offIntersections = offCubes.Where(c => c.IntersectsWith(instruction.Cuboid)).Select(c => Cube.Intersect(c, instruction.Cuboid)).ToList();

                foreach (var intersection in onIntersections)
                {
                    onVolume -= intersection.Volume;
                    offCubes.Add(intersection);
                }

                foreach (var intersection in offIntersections)
                {
                    onVolume += intersection.Volume;
                    onCubes.Add(intersection);
                }

                if (instruction.State)
                {
                    onVolume += instruction.Cuboid.Volume;
                    onCubes.Add(instruction.Cuboid);
                }
            }

            return onVolume.ToString();
        }

        private record Instruction(bool State, Cube Cuboid);
    }
}
