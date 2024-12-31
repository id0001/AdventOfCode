using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(6)]
public class Challenge06(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(6);
        var start = grid.Find(p => p == '^');
        var guard = new Pose2(start, Face.Up);

        var visited = Simulate(grid, guard);
        return visited.DistinctBy(v => v.Position).Count().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(6);
        var start = grid.Find(p => p == '^');
        var guard = new Pose2(start, Face.Up);

        var count = 0;
        var checkedPositions = new HashSet<Pose2>();
        var toCheck =
            Simulate(grid, guard).DistinctBy(v => v.Position).ToList(); // Takes the first pose for every position.
        for (var i = 0; i < toCheck.Count; i++)
        {
            if (toCheck[i].Position == start)
            {
                checkedPositions.Add(toCheck[i]);
                continue;
            }

            if (!SimulateFrom(grid, toCheck[i - 1], toCheck[i].Position, checkedPositions))
                count++;

            checkedPositions.Add(toCheck[i]);
        }

        return count.ToString();
    }

    private static List<Pose2> Simulate(char[,] grid, Pose2 guard)
    {
        List<Pose2> path = [guard];
        var bounds = grid.Bounds();
        while (true)
        {
            var next = guard.Ahead;
            if (!bounds.Contains(next))
                break;

            if (grid[next.Y, next.X] == '#')
            {
                guard = guard.TurnRight();
                continue;
            }

            guard = guard.Step();
            path.Add(guard);
        }

        return path;
    }

    private static bool SimulateFrom(char[,] grid, Pose2 guard, Point2 obstacle, IReadOnlySet<Pose2> visitedBase)
    {
        var visited = new HashSet<Pose2>();
        var bounds = grid.Bounds();
        while (true)
        {
            var next = guard.Ahead;
            if (!bounds.Contains(next))
                break;

            if (grid[next.Y, next.X] == '#' || next == obstacle)
            {
                guard = guard.TurnRight();
                continue;
            }

            guard = guard.Step();
            if (visitedBase.Contains(guard) || !visited.Add(guard))
                return false; // loop detected
        }

        return true;
    }
}