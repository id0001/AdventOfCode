using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using System.Text;

namespace AdventOfCode2017.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(19);
        var start = Array.IndexOf(grid.GetRow(0).ToArray(), '|');

        return FollowToEnd(grid, new Point2(start, 0)).Result;
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(19);
        var start = Array.IndexOf(grid.GetRow(0).ToArray(), '|');

        return FollowToEnd(grid, new Point2(start, 0)).Steps.ToString();
    }

    private static (string Result, int Steps) FollowToEnd(char[,] grid, Point2 start)
    {
        var result = new StringBuilder();
        var dir = Face.Down;
        var p = start + dir;
        var steps = 1;
        var bounds = new Rectangle(0, 0, grid.GetLength(1), grid.GetLength(0));

        while (bounds.Contains(p) && grid[p.Y, p.X] != ' ')
        {
            if (grid[p.Y, p.X] is not '|' and not '-' and not '+' and not ' ')
                result.Append(grid[p.Y, p.X]);
            else if (grid[p.Y, p.X] == '+')
                dir = GetNewDirection(grid, dir, p, bounds);

            p += dir;
            steps++;
        }

        return (result.ToString(), steps);
    }

    private static Point2 GetNewDirection(char[,] grid, Point2 dir, Point2 current, Rectangle bounds) => dir == Face.Up || dir == Face.Down
            ? ChooseDirection([current.Left, current.Right], '|', bounds, grid, current)
            : ChooseDirection([current.Up, current.Down], '-', bounds, grid, current);

    private static Point2 ChooseDirection(IEnumerable<Point2> options, char oppositeDirectionalChar, Rectangle bounds, char[,] grid, Point2 current)
        => options.Single(n => bounds.Contains(n) && !new[] { ' ', oppositeDirectionalChar }.Contains(grid[n.Y, n.X])) - current;
}