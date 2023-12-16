using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(14)]
public class Challenge14
{
    private readonly IInputReader _inputReader;

    public Challenge14(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(14);
        var (round, cubes) = Extract(grid);
        round = MoveUp(round, cubes);

        return round.Sum(p => grid.GetLength(0) - p.Y).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(14);
        var (round, cubes) = Extract(grid);

        var totalCycles = 1_000_000_000;
        var states = new Dictionary<string, int>();
        var cycleFound = false;
        for (var i = 0; i < totalCycles; i++)
        {
            states.TryAdd(GetState(round, cubes, grid), i);
            round = MoveUp(round, cubes);
            round = MoveLeft(round, cubes);
            round = MoveDown(round, cubes, grid.GetLength(0));
            round = MoveRight(round, cubes, grid.GetLength(1));

            if (cycleFound)
                continue;

            var state = GetState(round, cubes, grid);
            if (states.TryGetValue(state, out var cycle))
            {
                var rest = (totalCycles - cycle) % (i + 1 - cycle) + 1;
                totalCycles = i + rest;
                cycleFound = true;
            }
        }

        return round.Sum(p => grid.GetLength(0) - p.Y).ToString();
    }

    private static (ISet<Point2> Round, ISet<Point2> Cubes) Extract(char[,] grid)
    {
        var round = new HashSet<Point2>();
        var cubes = new HashSet<Point2>();

        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
            switch (grid[y, x])
            {
                case 'O':
                    round.Add(new Point2(x, y));
                    break;
                case '#':
                    cubes.Add(new Point2(x, y));
                    break;
            }

        return (round, cubes);
    }

    public ISet<Point2> MoveUp(ISet<Point2> round, ISet<Point2> cubes)
    {
        var set = new HashSet<Point2>();

        foreach (var p in round.OrderBy(p => p.Y))
        {
            var y = p.Y;
            while (y - 1 >= 0 && !set.Contains(new Point2(p.X, y - 1)) && !cubes.Contains(new Point2(p.X, y - 1)))
                y--;

            set.Add(new Point2(p.X, y));
        }

        return set;
    }

    public ISet<Point2> MoveDown(ISet<Point2> round, ISet<Point2> cubes, int size)
    {
        var set = new HashSet<Point2>();

        foreach (var p in round.OrderByDescending(p => p.Y))
        {
            var y = p.Y;
            while (y + 1 < size && !set.Contains(new Point2(p.X, y + 1)) && !cubes.Contains(new Point2(p.X, y + 1)))
                y++;

            set.Add(new Point2(p.X, y));
        }

        return set;
    }

    public ISet<Point2> MoveLeft(ISet<Point2> round, ISet<Point2> cubes)
    {
        var set = new HashSet<Point2>();

        foreach (var p in round.OrderBy(p => p.X))
        {
            var x = p.X;
            while (x - 1 >= 0 && !set.Contains(new Point2(x - 1, p.Y)) && !cubes.Contains(new Point2(x - 1, p.Y)))
                x--;

            set.Add(new Point2(x, p.Y));
        }

        return set;
    }

    public ISet<Point2> MoveRight(ISet<Point2> round, ISet<Point2> cubes, int size)
    {
        var set = new HashSet<Point2>();

        foreach (var p in round.OrderByDescending(p => p.X))
        {
            var x = p.X;
            while (x + 1 < size && !set.Contains(new Point2(x + 1, p.Y)) && !cubes.Contains(new Point2(x + 1, p.Y)))
                x++;

            set.Add(new Point2(x, p.Y));
        }

        return set;
    }

    private string GetState(ISet<Point2> round, ISet<Point2> cubes, char[,] grid)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
                if (round.Contains(new Point2(x, y)))
                    sb.Append('O');
                else if (cubes.Contains(new Point2(x, y)))
                    sb.Append('#');
                else
                    sb.Append('.');

            sb.AppendLine();
        }

        return sb.ToString();
    }
}