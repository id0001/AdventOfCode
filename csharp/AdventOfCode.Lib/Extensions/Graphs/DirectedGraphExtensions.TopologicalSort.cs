using AdventOfCode.Lib.Graphs;
using Microsoft;

namespace AdventOfCode.Lib;

public static partial class DirectedGraphExtensions
{
    public static IEnumerable<TVertex> TopologicalSort<TVertex, TEdge>(this DirectedGraph<TVertex, TEdge> graph)
        where TVertex : notnull
        where TEdge : notnull
    {
        Requires.NotNull(graph, nameof(graph));

        List<TVertex> result = new();
        Queue<TVertex> options = new(graph.Vertices.Where(v => graph.InEdges(v).Count == 0));
        var inDegree = graph.Vertices.ToDictionary(v => v, v => graph.InEdges(v).Keys.ToHashSet());

        while (options.Count > 0)
        {
            var n = options.Dequeue();
            result.Add(n);

            foreach (var m in graph.OutEdges(n).Keys)
            {
                inDegree[m].Remove(n);
                if (inDegree[m].Count == 0)
                    options.Enqueue(m);
            }
        }

        if (inDegree.Any(e => e.Value.Count > 0))
            throw new ArgumentException("Graph cannot have cycles");

        return result;
    }

    public static IEnumerable<TVertex> LexicographicalTopologicalSort<TVertex, TEdge>(
        this DirectedGraph<TVertex, TEdge> graph, IComparer<TVertex>? comparer = null)
        where TVertex : IComparable<TVertex>
        where TEdge : notnull
    {
        Requires.NotNull(graph, nameof(graph));

        List<TVertex> result = new();
        PriorityQueue<TVertex, TVertex> options = new(comparer ?? Comparer<TVertex>.Default);
        options.EnqueueRange(graph.Vertices.Where(v => graph.InEdges(v).Count == 0).Select(v => (v, v)));

        var inDegree = graph.Vertices.ToDictionary(v => v, v => graph.InEdges(v).Keys.ToHashSet());

        while (options.Count > 0)
        {
            var n = options.Dequeue();
            result.Add(n);

            foreach (var m in graph.OutEdges(n).Keys)
            {
                inDegree[m].Remove(n);
                if (inDegree[m].Count == 0)
                    options.Enqueue(m, m);
            }
        }

        if (inDegree.Any(e => e.Value.Count > 0))
            throw new ArgumentException("Graph cannot have cycles");

        return result;
    }
}