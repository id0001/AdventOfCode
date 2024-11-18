using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode.Lib;

public static partial class DirectedGraphExtensions
{
    public static bool TryPath<TVertex>(this DirectedGraph<TVertex, int> graph, TVertex start,
        Func<TVertex, bool> isFinished, out IEnumerable<TVertex> path, out int totalCost)
        where TVertex : notnull
    {
        var astar = new AStar<TVertex>(c => GetAdjacent(graph, c), (c, n) => GetWeight(graph, c, n));
        return astar.TryPath(start, isFinished, out path, out totalCost);
    }

    private static IEnumerable<TVertex> GetAdjacent<TVertex, TEdge>(DirectedGraph<TVertex, TEdge> graph,
        TVertex current)
        where TVertex : notnull
        where TEdge : notnull
    {
        foreach (var edges in graph.OutEdges(current))
            yield return edges.Key;
    }

    private static int GetWeight<TVertex>(DirectedGraph<TVertex, int> graph, TVertex current, TVertex next)
        where TVertex : notnull
        => graph.OutEdges(current)[next];
}