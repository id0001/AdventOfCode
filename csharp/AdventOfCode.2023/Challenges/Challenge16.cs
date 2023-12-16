using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(16)]
public class Challenge16
{
    private readonly IInputReader _inputReader;

    public Challenge16(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(16);
        var start = new Beam(Point2.Zero, new Point2(1, 0));
        return Energize(grid, start).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(16);

        var max = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            max = Math.Max(max, Energize(grid, new Beam(new Point2(0, y), new Point2(1, 0))));
            max = Math.Max(max, Energize(grid, new Beam(new Point2(grid.GetLength(1) - 1, y), new Point2(-1, 0))));
        }

        for (var x = 0; x < grid.GetLength(1); x++)
        {
            max = Math.Max(max, Energize(grid, new Beam(new Point2(x, 0), new Point2(0, 1))));
            max = Math.Max(max, Energize(grid, new Beam(new Point2(x, grid.GetLength(0) - 1), new Point2(0, -1))));
        }

        return max.ToString();
    }

    private static int Energize(char[,] grid, Beam start)
    {
        var visited = new HashSet<Beam>();
        var beams = new Queue<Beam>();
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
                case '|' when beam.Direction.X != 0:
                    beams.Enqueue(new Beam(beam.Position with {Y = beam.Position.Y - 1}, new Point2(0, -1)));
                    beams.Enqueue(new Beam(beam.Position with {Y = beam.Position.Y + 1}, new Point2(0, 1)));
                    break;
                case '-' when beam.Direction.Y != 0:
                    beams.Enqueue(new Beam(beam.Position with {X = beam.Position.X - 1}, new Point2(-1, 0)));
                    beams.Enqueue(new Beam(beam.Position with {X = beam.Position.X + 1}, new Point2(1, 0)));
                    break;
                case '/':
                    beams.Enqueue(beam.Direction switch
                    {
                        (0, 1) => new Beam(beam.Position with {X = beam.Position.X - 1}, new Point2(-1, 0)),
                        (0, -1) => new Beam(beam.Position with {X = beam.Position.X + 1}, new Point2(1, 0)),
                        (1, 0) => new Beam(beam.Position with {Y = beam.Position.Y - 1}, new Point2(0, -1)),
                        (-1, 0) => new Beam(beam.Position with {Y = beam.Position.Y + 1}, new Point2(0, 1)),
                        _ => throw new NotImplementedException()
                    });
                    break;
                case '\\':
                    beams.Enqueue(beam.Direction switch
                    {
                        (0, -1) => new Beam(beam.Position with {X = beam.Position.X - 1}, new Point2(-1, 0)),
                        (0, 1) => new Beam(beam.Position with {X = beam.Position.X + 1}, new Point2(1, 0)),
                        (-1, 0) => new Beam(beam.Position with {Y = beam.Position.Y - 1}, new Point2(0, -1)),
                        (1, 0) => new Beam(beam.Position with {Y = beam.Position.Y + 1}, new Point2(0, 1)),
                        _ => throw new NotImplementedException()
                    });
                    break;
                default:
                    var next = beam.Position + beam.Direction;
                    beams.Enqueue(new Beam(next, beam.Direction));
                    break;
            }
        }

        return visited.Select(x => x.Position).Distinct().Count();
    }

    private record Beam(Point2 Position, Point2 Direction);
}