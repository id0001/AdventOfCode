using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => (await inputReader.ReadGridAsync(12))
        .Into(GetRegions)
        .Sum(r => r.Count * CountPerimeter(r))
        .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadGridAsync(12))
        .Into(GetRegions)
        .Sum(r => r.Count * CountEdges(r))
        .ToString();

    private static int CountPerimeter(ISet<Point2> points)
        => points.SelectMany(p => p.GetNeighbors().Except(points)).Count();

    private static int CountEdges(ISet<Point2> points) => points.Sum(p => CountCornerVertices(points, p));

    private static int CountCornerVertices(ISet<Point2> points, Point2 p)
    {
        var corners = 0;
        if (HasTopLeftCorner(points, p))
            corners++;

        if (HasTopRightCorner(points, p))
            corners++;

        if (HasDownLeftCorner(points, p))
            corners++;

        if (HasDownRightCorner(points, p))
            corners++;

        return corners;
    }

    private static bool HasTopLeftCorner(ISet<Point2> points, Point2 p) =>
        (points.Contains(p.Up), points.Contains(p.Left)) switch
        {
            (false, false) => true,
            (true, true) when !points.Contains(p.Up.Left) => true,
            _ => false
        };

    private static bool HasTopRightCorner(ISet<Point2> points, Point2 p) =>
        (points.Contains(p.Up), points.Contains(p.Right)) switch
        {
            (false, false) => true,
            (true, true) when !points.Contains(p.Up.Right) => true,
            _ => false
        };

    private static bool HasDownLeftCorner(ISet<Point2> points, Point2 p) =>
        (points.Contains(p.Down), points.Contains(p.Left)) switch
        {
            (false, false) => true,
            (true, true) when !points.Contains(p.Down.Left) => true,
            _ => false
        };

    private static bool HasDownRightCorner(ISet<Point2> points, Point2 p) =>
        (points.Contains(p.Down), points.Contains(p.Right)) switch
        {
            (false, false) => true,
            (true, true) when !points.Contains(p.Down.Right) => true,
            _ => false
        };

    private static IList<ISet<Point2>> GetRegions(char[,] grid)
    {
        var visited = new HashSet<Point2>();
        var regions = new List<ISet<Point2>>();
        foreach (var (p, _) in grid.AsEnumerable())
        {
            if (visited.Contains(p))
                continue;

            var region = grid
                .Path(p, GetAdjacent)
                .FloodFill()
                .ToHashSet();

            regions.Add(region);
            visited = visited.Concat(region).ToHashSet();
        }

        return regions;
    }

    private static IEnumerable<Point2> GetAdjacent(char[,] grid, Point2 p)
        => p.GetNeighbors().Where(n => grid.Bounds().Contains(n) && grid[n.Y, n.X] == grid[p.Y, p.X]);
}