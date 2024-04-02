using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2016.Challenges;

[Challenge(13)]
public class Challenge13()
{
    private const int Input = 1352;

    [Part1]
    public string Part1()
    {
        var start = new Point2(1, 1);
        var bfs = new BreadthFirstSearch<Point2>(GetAdjacent);

        bfs.TryPath(start, p => p == new Point2(31, 39), out var path);

        return (path.Count() - 1).ToString();
    }

    [Part2]
    public string Part2()
    {
        var start = new Point2(1, 1);
        var bfs = new BreadthFirstSearch<Point2>(GetAdjacent);

        return bfs.FloodFill(start, 50).Count().ToString();
    }

    private static IEnumerable<Point2> GetAdjacent(Point2 pos) => pos
        .GetNeighbors()
        .Where(p => p.X >= 0 && p.Y >= 0 && !IsWall(p.X, p.Y));

    private static bool IsWall(int x, int y) => Convert.ToString(x * x + 3 * x + 2 * x * y + y + y * y + Input, 2).Count(x => x == '1') % 2 != 0;
}
