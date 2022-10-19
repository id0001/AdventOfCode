using AdventOfCode.Lib;
using AdventOfCode2019.IntCode.Core;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2019.Challenges;

[Challenge(11)]
public class Challenge11
{
    private readonly IInputReader _inputReader;

    public Challenge11(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(11, ',').ToArrayAsync();

        var locationHistory = new Dictionary<Point2, long>();
        var currentLocation = new Point2();
        locationHistory.Add(currentLocation, 0);
        var direction = 0;
        var action = 0;

        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o =>
        {
            switch (action)
            {
                case 0: // Paint location
                    locationHistory[currentLocation] = o;
                    break;
                case 1: // Change direction and move
                    direction = o == 0 ? Euclid.Modulus(direction - 1, 4) : Euclid.Modulus(direction + 1, 4);
                    currentLocation = NextLocation(currentLocation, direction);
                    break;
                default:
                    throw new NotSupportedException();
            }

            action = (action + 1) % 2;
        });

        cpu.RegisterInput(() =>
        {
            locationHistory.TryGetValue(currentLocation, out var color);
            cpu.WriteInput(color);
        });

        await cpu.StartAsync();

        return locationHistory.Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var program = await _inputReader.ReadLineAsync<long>(11, ',').ToArrayAsync();

        var locationHistory = new Dictionary<Point2, long>();
        var currentLocation = new Point2();
        locationHistory.Add(currentLocation, 1);
        var direction = 0;
        var action = 0;

        var cpu = new Cpu();
        cpu.SetProgram(program);
        cpu.RegisterOutput(o =>
        {
            switch (action)
            {
                case 0: // Paint location
                    locationHistory[currentLocation] = o;
                    break;
                case 1: // Change direction and move
                    direction = o == 0 ? Euclid.Modulus(direction - 1, 4) : Euclid.Modulus(direction + 1, 4);
                    currentLocation = NextLocation(currentLocation, direction);
                    break;
                default:
                    throw new NotSupportedException();
            }

            action = (action + 1) % 2;
        });

        cpu.RegisterInput(() =>
        {
            locationHistory.TryGetValue(currentLocation, out var color);
            cpu.WriteInput(color);
        });

        await cpu.StartAsync();


        return DrawHull(locationHistory);
    }

    private static Point2 NextLocation(Point2 currentLocation, int direction)
    {
        return currentLocation + direction switch
        {
            0 => new Point2(0, -1),
            1 => new Point2(1, 0),
            2 => new Point2(0, 1),
            3 => new Point2(-1, 0),
            _ => throw new NotSupportedException()
        };
    }

    private static string DrawHull(Dictionary<Point2, long> locations)
    {
        var keys = locations.Keys;

        var leftMost = keys.Min(e => e.X);
        var rightMost = keys.Max(e => e.X);
        var topMost = keys.Min(e => e.Y);
        var bottomMost = keys.Max(e => e.Y);

        var rows = bottomMost - topMost + 1;
        var cols = rightMost - leftMost + 1;

        var sb = new StringBuilder();
        sb.AppendLine();
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
                if (locations.TryGetValue(new Point2(x + leftMost, y + topMost), out var pvalue))
                    sb.Append(pvalue == 1 ? '#' : '.');
                else
                    sb.Append('.');

            sb.AppendLine();
        }

        return sb.ToString();
    }
}