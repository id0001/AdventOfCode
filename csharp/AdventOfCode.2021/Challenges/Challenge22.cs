using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(22)]
public class Challenge22
{
    private const string InputPattern = @"^(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)$";
    private readonly IInputReader _inputReader;
    private readonly List<Instruction> _instructions = new();

    public Challenge22(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in _inputReader.ReadLinesAsync(22))
        {
            var match = Regex.Match(line, InputPattern);

            var turnOn = match.Groups[1].Value == "on";
            var xFrom = int.Parse(match.Groups[2].Value);
            var xTo = int.Parse(match.Groups[3].Value);
            var yFrom = int.Parse(match.Groups[4].Value);
            var yTo = int.Parse(match.Groups[5].Value);
            var zFrom = int.Parse(match.Groups[6].Value);
            var zTo = int.Parse(match.Groups[7].Value);

            var cuboid = new Cube(xFrom, yFrom, zFrom,xTo - xFrom + 1, yTo - yFrom + 1, zTo - zFrom + 1);
            _instructions.Add(new Instruction(turnOn, cuboid));
        }
    }

    [Part1]
    public string Part1()
    {
        var map = new SparseSpatialMap<Point3, int, bool>();

        var bounds = new Cube(-50, -50, -50, 101, 101, 101);

        foreach (var instruction in _instructions)
        {
            if (!instruction.Cuboid.IntersectsWith(bounds))
                break;

            foreach (var point in instruction.Cuboid.Points) map.Set(point, instruction.State);
        }

        return map.Count(x => x.Value).ToString();
    }

    [Part2]
    public string Part2()
    {
        var onVolume = 0L;
        var onCubes = new List<Cube>();
        var offCubes = new List<Cube>();

        foreach (var instruction in _instructions)
        {
            var onIntersections = onCubes.Where(c => c.IntersectsWith(instruction.Cuboid))
                .Select(c => Cube.Intersect(c, instruction.Cuboid)).ToList();
            var offIntersections = offCubes.Where(c => c.IntersectsWith(instruction.Cuboid))
                .Select(c => Cube.Intersect(c, instruction.Cuboid)).ToList();

            foreach (var intersection in onIntersections)
            {
                onVolume -= intersection.LongVolume;
                offCubes.Add(intersection);
            }

            foreach (var intersection in offIntersections)
            {
                onVolume += intersection.LongVolume;
                onCubes.Add(intersection);
            }

            if (instruction.State)
            {
                onVolume += instruction.Cuboid.LongVolume;
                onCubes.Add(instruction.Cuboid);
            }
        }

        return onVolume.ToString();
    }

    private record Instruction(bool State, Cube Cuboid);
}