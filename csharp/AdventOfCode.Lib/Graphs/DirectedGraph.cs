using Microsoft;

namespace AdventOfCode.Lib.Graphs;

public class DirectedGraph<TVertex, TEdge>
    where TVertex : notnull
    where TEdge : notnull
{
    private readonly Dictionary<(TVertex, TVertex), TEdge> _edges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexOutEdges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexInEdges = new();

    public int VertexCount => _vertexOutEdges.Count;

    public int EdgeCount => _edges.Count;

    public IEnumerable<TVertex> Vertices => _vertexOutEdges.Keys;

    public bool AddVertex(TVertex vertex)
    {
        if (ContainsVertex(vertex))
            return false;

        _vertexInEdges.Add(vertex, new List<TVertex>());
        _vertexOutEdges.Add(vertex, new List<TVertex>());
        return true;
    }

    public bool RemoveVertex(TVertex vertex)
    {
        if (!ContainsVertex(vertex))
            return false;

        var outEdges = OutEdges(vertex);
        foreach (var edge in outEdges)
            RemoveEdge(vertex, edge.Key);

        _vertexInEdges.Remove(vertex);
        _vertexOutEdges.Remove(vertex);
        return true;
    }

    public bool ContainsVertex(TVertex vertex) => _vertexInEdges.ContainsKey(vertex);

    public bool AddEdge(TVertex source, TVertex target, TEdge value)
    {
        Requires.Argument(ContainsVertex(source), "Vertex does not exist", nameof(source));
        Requires.Argument(ContainsVertex(target), "Vertex does not exist", nameof(target));

        if (ContainsEdge(source, target))
            return false;

        _edges.Add((source, target), value);
        _vertexOutEdges[source].Add(target);
        _vertexInEdges[target].Add(source);
        return true;
    }

    public bool RemoveEdge(TVertex source, TVertex target)
    {
        Requires.Argument(ContainsVertex(source), "Vertex does not exist", nameof(source));
        Requires.Argument(ContainsVertex(target), "Vertex does not exist", nameof(target));

        if (!ContainsEdge(source, target))
            return false;

        _edges.Remove((source, target));
        _vertexInEdges[target].Remove(source);
        _vertexOutEdges[source].Remove(target);
        return true;
    }

    public bool ContainsEdge(TVertex source, TVertex target)
    {
        Requires.Argument(ContainsVertex(source), "Vertex does not exist", nameof(source));
        Requires.Argument(ContainsVertex(target), "Vertex does not exist", nameof(target));

        return _edges.ContainsKey((source, target));
    }

    public IDictionary<TVertex, TEdge> OutEdges(TVertex vertex) =>
        _vertexOutEdges[vertex].ToDictionary(target => target, target => _edges[(vertex, target)]);

    public IDictionary<TVertex, TEdge> InEdges(TVertex vertex) =>
        _vertexInEdges[vertex].ToDictionary(source => source, source => _edges[(source, vertex)]);
}