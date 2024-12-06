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
        var pos = grid.Find(p => p == '^');

        var guard = new Pose2(pos, Face.Up);

        var visited = Simulate(grid, guard);
        return visited.DistinctBy(v => v.Position).Count().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(6);
        var pos = grid.Find(p => p == '^');
    
        var guard = new Pose2(pos, Face.Up);
        
        var visited = Simulate(grid, guard);

        var count = 0;
        var checkedPositions = new HashSet<Point2>();
        for (var i = 0; i < visited.Count; i++)
        {
            if (visited[i].Position == pos)
            {
                checkedPositions.Add(visited[i].Position);
                continue;
            }
            
            if(checkedPositions.Contains(visited[i].Position))
                continue;

            if (!SimulateFrom(grid, visited[i - 1], visited[i].Position, visited.Take(i).ToHashSet()))
                count++;
            
            checkedPositions.Add(visited[i].Position);
        }
    
        return count.ToString();
    }

    private List<Pose2> Simulate(char[,] grid, Pose2 guard)
    {
        var visited = new List<Pose2> {guard};

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
            visited.Add(guard);
        }

        return visited;
    }

    private bool SimulateFrom(char[,] grid, Pose2 guard, Point2 obstacle, HashSet<Pose2> visited)
    {
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
            if (!visited.Add(guard))
                return false; // loop detected
        }

        return true;
    }
}
