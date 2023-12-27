namespace AdventOfCode.Lib.Graphs;

public class UndirectedGraph<TVertex, TEdge>
where TVertex : notnull
where TEdge : notnull
{
    private readonly Dictionary<(TVertex, TVertex), TEdge> _edges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexEdges = new();

    public int VertexCount => _vertexEdges.Count;

    public int EdgeCount => _edges.Count / 2; // _edges always contains 2 entries for each edge: (a->b, b->a)

    public IEnumerable<TVertex> Vertices => _vertexEdges.Keys;

    public bool AddVertex(TVertex vertex)
    {
        if (ContainsVertex(vertex))
            return false;

        _vertexEdges.Add(vertex, new List<TVertex>());
        return true;
    }

    public bool RemoveVertex(TVertex vertex)
    {
        if (!ContainsVertex(vertex))
            return false;

        foreach (var target in _vertexEdges[vertex])
        {
            _edges.Remove((vertex, target));
            _edges.Remove((target, vertex));
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
        _edges.Add((vertex2, vertex1), edge);
        _vertexEdges[vertex1].Add(vertex2);
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

    public bool ContainsEdge(TVertex source, TVertex target) => _edges.ContainsKey((source, target));

    public bool HasEdges(TVertex vertex) => _vertexEdges.TryGetValue(vertex, out var edges) && edges.Count > 0;
   
    public IReadOnlyDictionary<TVertex, TEdge> Edges(TVertex vertex)
    {
        return !ContainsVertex(vertex) 
            ? new Dictionary<TVertex,TEdge>()
            : _vertexEdges[vertex]
                .Select(t => (Target: t, Edge: _edges[(vertex, t)]))
                .ToDictionary(kv => kv.Target, kv => kv.Edge);
    }
}