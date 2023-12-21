using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2023.Challenges;

[Challenge(21)]
public class Challenge21
{
    private readonly IInputReader _inputReader;

    public Challenge21(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(21);
        var start = grid.FindPosition(x => x == 'S');
        var bounds = grid.Bounds();

        return Enumerable.Range(0, 64).Aggregate(new HashSet<Point2> { start }, (a, _) => a.SelectMany(p => p.GetNeighbors().Where(n => bounds.Contains(n) && grid[n.Y, n.X] != '#')).ToHashSet()).Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(21);
        return Polynomial.LagrangeInterpolate(CalculateFirstThreeDataPoints(grid), (26_501_365 - 65) / 131).ToString();
    }

    private Point2[] CalculateFirstThreeDataPoints(char[,] grid)
    {
        var half = grid.GetLength(1) / 2;
        var steps = new Point2[3];

        var on = new HashSet<Point2> { grid.FindPosition(c => c == 'S') };
        for (var i = 1; i <= grid.GetLength(1) * 2 + half; i++)
        {
            var newOn = new HashSet<Point2>();
            foreach (var p in on)
            {
                foreach (var neighbor in p.GetNeighbors().Where(n => !newOn.Contains(n) && grid[Euclid.Modulus(n.Y, grid.GetLength(0)), Euclid.Modulus(n.X, grid.GetLength(1))] != '#'))
                    newOn.Add(neighbor);
            }

            on = newOn;

            if (i == half)
                steps[0] = new Point2(0, on.Count);

            if (i == grid.GetLength(1) + half)
                steps[1] = new Point2(1, on.Count);

            if (i == grid.GetLength(1) * 2 + half)
            {
                steps[2] = new Point2(2, on.Count);
                break;
            }
        }

        return steps;
    }
}