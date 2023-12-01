using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2022.Challenges;

[Challenge(24)]
public class Challenge24
{
    private readonly IInputReader _inputReader;

    public Challenge24(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(24);

        var start = new Point2(1, 0);
        var goal = new Point2(120, 26);

        var state = new State(start, 0);

        var weight = new Func<State, State, int>((_, _) => 1);
        var heuristic = new Func<State, int>(c => Point2.ManhattanDistance(c.Position, goal));
        var neighbors = new Func<State, IEnumerable<State>>(c => GetAdjecent(grid, c));
        var goalReached = new Func<State, bool>(c => c.Position == goal);

        var astar = new AStar<State>(neighbors, weight, heuristic);
        astar.TryPath(state, goalReached, out _, out var minutes);

        return minutes.ToString();
    }


    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(24);

        var start = new Point2(1, 0);
        var goal = new Point2(120, 26);

        var state = new State2(start, 0, 0);

        var weight = new Func<State2, State2, int>((_, _) => 1);
        var heuristic = new Func<State2, int>(c =>
        {
            if (c.Phase == 1)
                return Point2.ManhattanDistance(c.Position, start);

            return Point2.ManhattanDistance(c.Position, goal);
        });

        var neighbors = new Func<State2, IEnumerable<State2>>(c => GetAdjecent2(grid, start, goal, c));
        var goalReached = new Func<State2, bool>(c => c.Position == goal && c.Phase == 2);

        var astar = new AStar<State2>(neighbors, weight, heuristic);
        astar.TryPath(state, goalReached, out _, out var minutes);

        return minutes.ToString();
    }

    private static IEnumerable<State> GetAdjecent(char[,] grid, State current)
    {
        foreach (var neighbor in current.Position.GetNeighbors())
        {
            if (neighbor.X < 0 || neighbor.X >= grid.GetLength(1) || neighbor.Y < 0 ||
                neighbor.Y >= grid.GetLength(0) || grid[neighbor.Y, neighbor.X] == '#')
                continue;

            if (!HasBlizzard(grid, neighbor, current.Time + 1))
                yield return new State(neighbor, current.Time + 1);
        }

        if (!HasBlizzard(grid, current.Position, current.Time + 1))
            yield return new State(current.Position, current.Time + 1);
    }

    private static IEnumerable<State2> GetAdjecent2(char[,] grid, Point2 start, Point2 goal, State2 current)
    {
        var nextPhase = current.Phase;
        if (current.Position == goal && current.Phase == 0)
            nextPhase = 1;
        else if (current.Position == start && current.Phase == 1)
            nextPhase = 2;

        foreach (var neighbor in current.Position.GetNeighbors())
        {
            if (neighbor.X < 0 || neighbor.X >= grid.GetLength(1) || neighbor.Y < 0 ||
                neighbor.Y >= grid.GetLength(0) || grid[neighbor.Y, neighbor.X] == '#')
                continue;

            if (!HasBlizzard(grid, neighbor, current.Time + 1))
                yield return new State2(neighbor, nextPhase, current.Time + 1);
        }

        if (!HasBlizzard(grid, current.Position, current.Time + 1))
            yield return new State2(current.Position, nextPhase, current.Time + 1);
    }

    private static bool HasBlizzard(char[,] grid, Point2 p, int time)
    {
        return grid[Wrap(p.Y - time, grid.GetLength(0)), p.X] == 'v'
               || grid[Wrap(p.Y + time, grid.GetLength(0)), p.X] == '^'
               || grid[p.Y, Wrap(p.X - time, grid.GetLength(1))] == '>'
               || grid[p.Y, Wrap(p.X + time, grid.GetLength(1))] == '<';
    }

    private static int Wrap(int v, int max)
    {
        return Euclid.Modulus(v - 1, max - 2) + 1;
    }

    public record State(Point2 Position, int Time);

    public record State2(Point2 Position, int Phase, int Time);
}