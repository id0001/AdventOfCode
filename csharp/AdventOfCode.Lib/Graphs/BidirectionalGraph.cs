namespace AdventOfCode.Lib.Graphs;

public class BidirectionalGraph<TVertex, TEdge>
where TVertex : notnull
where TEdge : notnull
{
    private readonly Dictionary<(TVertex, TVertex), TEdge> _edges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexOutEdges = new();
    private readonly Dictionary<TVertex, List<TVertex>> _vertexInEdges = new();

    public int VertexCount => _vertexOutEdges.Count;

    public int EdgeCount => _edges.Count / 2; // _edges always contains 2 entries for each edge. (a->b, b->a)

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

        foreach (var target in _vertexOutEdges[vertex])
        {
            _edges.Remove((vertex, target));
            _edges.Remove((target, vertex));
            _vertexOutEdges[target].Remove(vertex);
            _vertexInEdges[target].Remove(vertex);
        }
        
        _vertexInEdges.Remove(vertex);
        _vertexOutEdges.Remove(vertex);
        return true;
    }

    public bool AddEdge(TVertex vertex1, TVertex vertex2, TEdge edge)
    {
        if (ContainsEdge(vertex1, vertex2))
            return false;
        
        _edges.Add((vertex1, vertex2), edge);
        _edges.Add((vertex2, vertex1), edge);
        _vertexOutEdges[vertex1].Add(vertex2);
        _vertexOutEdges[vertex2].Add(vertex1);
        _vertexInEdges[vertex1].Add(vertex2);
        _vertexInEdges[vertex2].Add(vertex1);
        return true;
    }

    public bool RemoveEdge(TVertex vertex1, TVertex vertex2)
    {
        if (!ContainsEdge(vertex1, vertex2))
            return false;

        _edges.Remove((vertex1, vertex2));
        _edges.Remove((vertex2, vertex1));
        _vertexOutEdges[vertex1].Remove(vertex2);
        _vertexOutEdges[vertex2].Remove(vertex1);
        _vertexInEdges[vertex1].Remove(vertex2);
        _vertexInEdges[vertex2].Remove(vertex1);
        return true;
    }

    public bool ContainsVertex(TVertex vertex) => _vertexOutEdges.ContainsKey(vertex);

    public bool ContainsEdge(TVertex source, TVertex target) => _edges.ContainsKey((source, target));

    public bool HasEdges(TVertex vertex) => _vertexOutEdges.TryGetValue(vertex, out var edges) && edges.Count > 0;
   
    public IReadOnlyDictionary<TVertex, TEdge> OutEdges(TVertex vertex)
    {
        return !ContainsVertex(vertex) 
            ? new Dictionary<TVertex,TEdge>()
            : _vertexOutEdges[vertex]
                .Select(t => (Target: t, Edge: _edges[(vertex, t)]))
                .ToDictionary(kv => kv.Target, kv => kv.Edge);
    }

    public IReadOnlyDictionary<TVertex, TEdge> InEdges(TVertex vertex)
    {
        return !ContainsVertex(vertex) 
            ? new Dictionary<TVertex,TEdge>()
            : _vertexInEdges[vertex]
                .Select(t => (Target: t, Edge: _edges[(vertex, t)]))
                .ToDictionary(kv => kv.Target, kv => kv.Edge);
    }
}