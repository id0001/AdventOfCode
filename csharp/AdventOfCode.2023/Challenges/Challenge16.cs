using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(16);
        var start = new Pose2(Point2.Zero, new Point2(1, 0));
        return Energize(grid, start).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(16);

        var max = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            max = Math.Max(max, Energize(grid, new Pose2(new Point2(0, y), Face.Right)));
            max = Math.Max(max, Energize(grid, new Pose2(new Point2(grid.GetLength(1) - 1, y), Face.Left)));
        }

        for (var x = 0; x < grid.GetLength(1); x++)
        {
            max = Math.Max(max, Energize(grid, new Pose2(new Point2(x, 0), Face.Down)));
            max = Math.Max(max, Energize(grid, new Pose2(new Point2(x, grid.GetLength(0) - 1), Face.Up)));
        }

        return max.ToString();
    }

    private static int Energize(char[,] grid, Pose2 start)
    {
        var visited = new HashSet<Pose2>();
        var beams = new Queue<Pose2>();
        beams.Enqueue(start);

        while (beams.Count > 0)
        {
            var beam = beams.Dequeue();
            if (beam.Position.X < 0 || beam.Position.X >= grid.GetLength(1) || beam.Position.Y < 0 ||
                beam.Position.Y >= grid.GetLength(0))
                continue;

            if (visited.Contains(beam))
                continue;

            visited.Add(beam);

            switch (grid[beam.Position.Y, beam.Position.X])
            {
                case '|' when beam.Face.X != 0:
                case '-' when beam.Face.Y != 0:
                    beams.Enqueue(beam.TurnLeft().Step());
                    beams.Enqueue(beam.TurnRight().Step());
                    break;
                case '/':
                    beams.Enqueue(beam.Face switch
                    {
                        (0, -1) => beam.TurnRight().Step(), // Up
                        (1, 0) => beam.TurnLeft().Step(), // Right
                        (0, 1) => beam.TurnRight().Step(), // Down
                        (-1, 0) => beam.TurnLeft().Step(), // Left
                        _ => throw new ArgumentOutOfRangeException()
                    });
                    break;
                case '\\':
                    beams.Enqueue(beam.Face switch
                    {
                        (0, -1) => beam.TurnLeft().Step(), // Up
                        (1, 0) => beam.TurnRight().Step(), // Right
                        (0, 1) => beam.TurnLeft().Step(), // Down
                        (-1, 0) => beam.TurnRight().Step(), // Left
                        _ => throw new ArgumentOutOfRangeException()
                    });
                    break;
                default:
                    beams.Enqueue(beam.Step());
                    break;
            }
        }

        return visited.Select(x => x.Position).Distinct().Count();
    }
}