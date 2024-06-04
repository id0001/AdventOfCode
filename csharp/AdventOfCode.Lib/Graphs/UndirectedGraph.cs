namespace AdventOfCode.Lib.Graphs;

public class UndirectedGraph<TVertex, TEdge>
    where TVertex : notnull
    where TEdge : notnull
{
    private readonly Dictionary<(TVertex, TVertex), TEdge> _edges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexEdges = new();

    public int VertexCount => _vertexEdges.Count;

    public int EdgeCount => _edges.Count;

    public IEnumerable<TVertex> Vertices => _vertexEdges.Keys;

    public IEnumerable<(TVertex, TVertex)> Edges => _edges.Keys;

    public bool AddVertex(TVertex vertex)
    {
        _vertexEdges.TryAdd(vertex, new List<TVertex>());
        return true;
    }

    public bool RemoveVertex(TVertex vertex)
    {
        if (!ContainsVertex(vertex))
            return false;

        foreach (var target in _vertexEdges[vertex])
        {
            RemoveEdge(vertex, target);
            _vertexEdges[target].Remove(vertex);
        }

        _vertexEdges.Remove(vertex);
        return true;
    }

    public bool AddEdge(TVertex vertex1, TVertex vertex2, TEdge edge)
    {
        if (ContainsEdge(vertex1, vertex2))
            return false;

        _edges.Add((vertex1, vertex2), edge);
        _vertexEdges[vertex1].Add(vertex2);

        // Don't add the edge twice when vertex1 and vertex2 are the same
        if (!EqualityComparer<TVertex>.Default.Equals(vertex1, vertex2))
            _vertexEdges[vertex2].Add(vertex1);

        return true;
    }

    public bool RemoveEdge(TVertex vertex1, TVertex vertex2)
    {
        if (!ContainsEdge(vertex1, vertex2))
            return false;

        _edges.Remove((vertex1, vertex2));
        _edges.Remove((vertex2, vertex1));
        _vertexEdges[vertex1].Remove(vertex2);
        _vertexEdges[vertex2].Remove(vertex1);
        return true;
    }

    public bool ContainsVertex(TVertex vertex) => _vertexEdges.ContainsKey(vertex);

    public bool ContainsEdge(TVertex source, TVertex target) =>
        _edges.ContainsKey((source, target)) || _edges.ContainsKey((target, source));

    public bool HasEdges(TVertex vertex) => _vertexEdges.TryGetValue(vertex, out var edges) && edges.Count > 0;

    public IReadOnlyDictionary<TVertex, TEdge> AdjacentEdges(TVertex vertex)
    {
        return !ContainsVertex(vertex)
            ? []
            : _vertexEdges[vertex]
                .Select(t => (Target: t, Edge: GetEdge(vertex, t)))
                .ToDictionary(kv => kv.Target, kv => kv.Edge);
    }

    private TEdge GetEdge(TVertex a, TVertex b)
    {
        if (_edges.TryGetValue((a, b), out var edge))
            return edge;

        if (_edges.TryGetValue((b, a), out edge))
            return edge;

        throw new ArgumentException("Edge not found");
    }
}