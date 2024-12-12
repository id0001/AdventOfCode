using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2024.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(12);
        var regions = GetRegions(grid);
        return regions.Sum(r => r.Count * CountPerimeter(r)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(12);
        var regions = GetRegions(grid);
        return regions.Sum(r => r.Count * CountCorners(r)).ToString();
    }

    private static int CountPerimeter(ISet<Point2> points)
    {
        int perimeter = 0;
        foreach (var p in points)
        {
            foreach (var neighbor in p.GetNeighbors())
            {
                if(!points.Contains(neighbor))
                    perimeter++;
            }
        }
        
        return perimeter;
    }
    
    private static int CountCorners(ISet<Point2> points)
    {
        int corners = 0;
        foreach (var p in points)
        {
            if(HasTopLeftCorner(points,p))
                corners++;
            
            if(HasTopRightCorner(points,p))
                corners++;
            
            if(HasDownLeftCorner(points,p))
                corners++;
            
            if(HasDownRightCorner(points,p))
                corners++;
        }

        return corners;
    }

    private static bool HasTopLeftCorner(ISet<Point2> points, Point2 p)
    {
        if (!points.Contains(p.Up.Left) && !points.Contains(p.Up) && !points.Contains(p.Left))
            return true;
        
        if (!points.Contains(p.Up.Left) && points.Contains(p.Up) && points.Contains(p.Left))
            return true;
        
        if (points.Contains(p.Up.Left) && !points.Contains(p.Up) && !points.Contains(p.Left))
            return true;

        return false;
    }
    
    private static bool HasTopRightCorner(ISet<Point2> points, Point2 p)
    {
        if (!points.Contains(p.Up.Right) && !points.Contains(p.Up) && !points.Contains(p.Right))
            return true;
        
        if (!points.Contains(p.Up.Right) && points.Contains(p.Up) && points.Contains(p.Right))
            return true;
        
        if (points.Contains(p.Up.Right) && !points.Contains(p.Up) && !points.Contains(p.Right))
            return true;

        return false;
    }
    
    private static bool HasDownLeftCorner(ISet<Point2> points, Point2 p)
    {
        if (!points.Contains(p.Down.Left) && !points.Contains(p.Down) && !points.Contains(p.Left))
            return true;
        
        if (!points.Contains(p.Down.Left) && points.Contains(p.Down) && points.Contains(p.Left))
            return true;
        
        if (points.Contains(p.Down.Left) && !points.Contains(p.Down) && !points.Contains(p.Left))
            return true;

        return false;
    }
    
    private static bool HasDownRightCorner(ISet<Point2> points, Point2 p)
    {
        if (!points.Contains(p.Down.Right) && !points.Contains(p.Down) && !points.Contains(p.Right))
            return true;
        
        if (!points.Contains(p.Down.Right) && points.Contains(p.Down) && points.Contains(p.Right))
            return true;
        
        if (points.Contains(p.Down.Right) && !points.Contains(p.Down) && !points.Contains(p.Right))
            return true;

        return false;
    }

    private static List<HashSet<Point2>> GetRegions(char[,] grid)
    {
        var visited = new HashSet<Point2>();
        var regions = new List<HashSet<Point2>>();
        foreach (var (p, _) in grid.AsEnumerable())
        {
            if (visited.Contains(p))
                continue;

            var region = grid.Bfs<char, Point2>(GetAdjacent, p)
                .FloodFill()
                .ToHashSet();

            regions.Add(region);
            visited = visited.Concat(region).ToHashSet();
        }

        return regions;
    }

    private static IEnumerable<Point2> GetAdjacent(char[,] grid, Point2 p)
    {
        var bounds = grid.Bounds();
        foreach (var neighbor in p.GetNeighbors())
        {
            if(!bounds.Contains(neighbor))
                continue;
            
            if(grid[neighbor.Y, neighbor.X] != grid[p.Y, p.X])
                continue;

            yield return neighbor;
        }
    }
}
