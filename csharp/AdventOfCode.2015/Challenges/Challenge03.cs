using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        var map = new Dictionary<Point2, int>();
        var location = Point2.Zero;

        await foreach (var c in inputReader.ReadLineAsync(3))
        {
            map.TryAdd(location, 0);
            map[location]++;
            location = c switch
            {
                '^' => location.Up,
                '>' => location.Right,
                'v' => location.Down,
                '<' => location.Left,
                _ => throw new NotSupportedException()
            };
        }

        return map.Count.ToString();
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        var map = new Dictionary<Point2, int>();
        Point2[] locations = [Point2.Zero, Point2.Zero];
        var index = 0;

        await foreach (var c in inputReader.ReadLineAsync(3))
        {
            map.TryAdd(locations[index], 0);
            map[locations[index]]++;
            locations[index] = c switch
            {
                '^' => locations[index].Up,
                '>' => locations[index].Right,
                'v' => locations[index].Down,
                '<' => locations[index].Left,
                _ => throw new NotSupportedException()
            };

            index = (index + 1) % 2;
        }

        return map.Count.ToString();
    }
}