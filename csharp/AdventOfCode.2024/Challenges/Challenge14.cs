using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(14)]
public class Challenge14(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var robots = await inputReader.ParseLinesAsync(14, ParseLine).ToListAsync();
        var size = new Point2(101, 103);

        var quadrants = new[]
        {
            new Rectangle(0, 0, (size.X-1) / 2, size.Y / 2),
            new Rectangle(size.X / 2 + 1, 0, size.X / 2, size.Y / 2),
            new Rectangle(0, size.Y / 2 + 1, size.X / 2, size.Y / 2),
            new Rectangle(size.X / 2 + 1, size.Y / 2 + 1, size.X / 2, size.Y / 2)
        };
        
        robots = Move(robots, size, 100).ToList();
        return quadrants.Select(q => robots.Count(r => q.Contains(r.Position))).Product().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var robots = await inputReader.ParseLinesAsync(14, ParseLine).ToListAsync();
        var size = new Point2(101, 103);

        for(var i = 100; ; i++)
        {
            var positions = Move(robots, size, i).Select(r => r.Position).ToHashSet();
            if (positions.Count(p => p.GetNeighbors().All(positions.Contains)) > 100)
                return i.ToString();
        }
    }

    private static IEnumerable<Robot> Move(IEnumerable<Robot> robots, Point2 size, int times) =>
        robots.Select(robot => robot with
        {
            Position = new Point2
            (
                (robot.Position.X + robot.Velocity.X * times).Mod(size.X), 
                (robot.Position.Y + robot.Velocity.Y * times).Mod(size.Y)
            )
        });

    private static Robot ParseLine(string line) =>
        line.Extract<int, int, int, int>(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)").Into(parts =>
            new Robot(new Point2(parts.First, parts.Second), new Point2(parts.Third, parts.Fourth)));

    private record Robot(Point2 Position, Point2 Velocity);
}
