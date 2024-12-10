using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2024.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = await inputReader.ReadGridAsync<int>(10);

        var trailheads = graph.Where((p, v) => v == 0).ToList();

        var bfs = new BreadthFirstSearch<Point2>(p => GetAdjacent(graph, p));
        return trailheads.Sum(p => ScoreTrailHead(bfs, graph, p)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var graph = await inputReader.ReadGridAsync<int>(10);

        var trailheads = graph.Where((p, v) => v == 0).ToList();
        var trailEnds = graph.Where((p, v) => v == 9).ToList();

        var bfs = new BreadthFirstSearch<Point2>(p => GetAdjacent(graph, p));
        return trailheads.SelectMany(s => trailEnds, (s, e) => CountTrails(bfs, s, e)).Sum().ToString();
    }

    private int ScoreTrailHead(BreadthFirstSearch<Point2> bfs, int[,] graph, Point2 start)
    {
        var list = bfs.FloodFill(start).ToList();
        return list.Count(t => graph[t.Value.Y, t.Value.X] == 9);
    }

    private int CountTrails(BreadthFirstSearch<Point2> bfs, Point2 start, Point2 end)
        => bfs.CountPaths(start, p => p == end);

    private static IEnumerable<Point2> GetAdjacent(int[,] graph, Point2 current)
    {
        var bounds = graph.Bounds();
        foreach (var neighbor in current.GetNeighbors().Where(n => bounds.Contains(n) && graph[n.Y, n.X] == graph[current.Y, current.X] + 1))
            yield return neighbor;
    }
}
