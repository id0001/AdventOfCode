using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = await inputReader.ReadGridAsync<int>(10);

        var trailheads = graph.Where((p, v) => v == 0).ToList();
        return trailheads.Sum(start => graph.Bfs(start, GetAdjacent).Count(n => graph[n.Y, n.X] == 9)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var graph = await inputReader.ReadGridAsync<int>(10);

        var trailheads = graph.Where((p, v) => v == 0).ToList();
        var trailEnds = graph.Where((p, v) => v == 9).ToList();

        return trailheads
            .SelectMany(start => trailEnds, (start, end) => graph
                .Bfs(start, GetAdjacent)
                .IgnoreVisited()
                .Count(n => n == end)
            )
            .Sum()
            .ToString();
    }

    private static IEnumerable<Point2> GetAdjacent(int[,] graph, Point2 current)
    {
        var bounds = graph.Bounds();
        foreach (var neighbor in current.GetNeighbors().Where(n => bounds.Contains(n) && graph[n.Y, n.X] == graph[current.Y, current.X] + 1))
            yield return neighbor;
    }
}
