using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2023.Challenges;

[Challenge(23)]
public class Challenge23
{
    private readonly IInputReader _inputReader;

    public Challenge23(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await _inputReader.ReadGridAsync(23);
        var start = new Point2(1, 0);
        var end = new Point2(grid.GetUpperBound(1) - 1, grid.GetUpperBound(0));

        var graph = CreateGraph(grid, start, end, false);
        return LongestPath(graph, start, end).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await _inputReader.ReadGridAsync(23);
        var start = new Point2(1, 0);
        var end = new Point2(grid.GetUpperBound(1) - 1, grid.GetUpperBound(0));
        
        var graph = CreateGraph(grid, start, end, true);
        return LongestPath(graph, start, end).ToString();
    }

    private static int LongestPath(DirectedGraph<Point2, int> graph, Point2 start, Point2 end)
    {
        var queue = new Queue<(Point2, List<Point2>, int)>();
        queue.Enqueue((start, new List<Point2>(), 0));

        var longestPath = 0;

        while (queue.Count > 0)
        {
            var (current, visited, length) = queue.Dequeue();
            if (current == end)
            {
                longestPath = Math.Max(length, longestPath);
                continue;
            }

            var newVisited = visited.Concat(new[] {current}).ToList();
            foreach(var n in graph.OutEdges(current))
            {
                if(visited.Contains(n.Key))
                    continue;
                
                queue.Enqueue((n.Key, newVisited, length + n.Value));
            }
        }

        return longestPath;
    }

    private static void TraverseEdge(DirectedGraph<Point2, int> graph, char[,] grid, Point2 end, Point2 from, Point2 to,
        bool downhill, bool ignoreSlopes)
    {
        var bounds = grid.Bounds();
        var visited = new HashSet<Point2> {from};
        while (!IsFork(grid, to) && to != end)
        {
            visited.Add(to);
            to = to.GetNeighbors().First(n => bounds.Contains(n) && grid[n.Y, n.X] != '#' && !visited.Contains(n));
        }

        graph.AddVertex(to);
        if (graph.ContainsEdge(from, to) || graph.ContainsEdge(to, from))
            return;

        (from, to) = downhill ? (from, to) : (to, from);
        graph.AddEdge(from, to, visited.Count);

        if (to == end)
            return;
        
        if (ignoreSlopes)
            graph.AddEdge(to, from, visited.Count);

        foreach (var n in to.GetNeighbors())
        {
            if (!bounds.Contains(n))
                continue;

            if (visited.Contains(n))
                continue;

            if (grid[n.Y, n.X] == '#')
                continue;

            downhill = grid[n.Y, n.X] switch
            {
                'v' when n - to == Point2.Down => true,
                '>' when n - to == Point2.Right => true,
                '<' when n - to == Point2.Left => true,
                '^' when n - to == Point2.Up => true,
                _ => false
            };

            TraverseEdge(graph, grid, end, to, n, downhill, ignoreSlopes);
        }
    }

    private static DirectedGraph<Point2, int> CreateGraph(char[,] grid, Point2 start, Point2 end, bool ignoreSlopes)
    {
        var bounds = grid.Bounds();
        var graph = new DirectedGraph<Point2, int>();
        graph.AddVertex(start);

        var next = start + new Point2(0, 1);
        TraverseEdge(graph, grid, end, start, next, true, ignoreSlopes);

        var prune = graph.Vertices.Where(v => v != end && graph.OutEdges(v).Count == 0).ToList();
        foreach (var v in prune)
            graph.RemoveVertex(v);

        return graph;
    }

    private static bool IsFork(char[,] grid, Point2 p) =>
        p.GetNeighbors().Where(grid.Bounds().Contains).Count(n => grid[n.Y, n.X] is not ('#' or '.')) > 1;
}