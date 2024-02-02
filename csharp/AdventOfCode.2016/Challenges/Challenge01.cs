using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => Point2.ManhattanDistance(Point2.Zero,
        (await inputReader.ReadAllTextAsync(1))
        .SplitBy(", ")
        .Aggregate(new Pose2(Point2.Zero, Face.Up),
            (pose, line) => (line[0], line[1..].As<int>()) switch
            {
                ('R', var a) => pose.TurnRight().Step(a),
                ('L', var a) => pose.TurnLeft().Step(a),
                _ => throw new NotImplementedException()
            })
        .Position).ToString();

    [Part2]
    public async Task<string> Part2Async()
    {
        HashSet<Point2> visited = [Point2.Zero];
        var pose = new Pose2(Point2.Zero, Face.Up);

        var text = await inputReader.ReadAllTextAsync(1);
        foreach (var (dir, amount) in text.SplitBy(", ").Select(line => (line[0], line[1..].As<int>())))
        {
            pose = dir == 'R' ? pose.TurnRight() : pose.TurnLeft();
            for (var i = 0; i < amount; i++)
            {
                pose = pose.Step();
                if (!visited.Add(pose.Position))
                    return Point2.ManhattanDistance(Point2.Zero, pose.Position).ToString();
            }
        }

        return string.Empty;
    }
}