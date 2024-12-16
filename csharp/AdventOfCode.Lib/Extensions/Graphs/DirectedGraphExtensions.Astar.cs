using AdventOfCode.Lib.Graphs;

namespace AdventOfCode.Lib;

public static partial class DirectedGraphExtensions
{
    public static AStar<DirectedGraph<TVertex, int>, TVertex> AStar<TVertex>(this DirectedGraph<TVertex, int> graph, TVertex start)
        where TVertex : notnull
        => new AStar<DirectedGraph<TVertex, int>, TVertex>(graph, start, n => GetAdjacent(graph, n), (c, n) => GetWeight(graph, c, n), _ => 0);

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