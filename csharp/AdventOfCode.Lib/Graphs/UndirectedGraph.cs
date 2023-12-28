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

    public bool ContainsEdge(TVertex source, TVertex target) => _edges.ContainsKey((source, target)) || _edges.ContainsKey((target, source));

    public bool HasEdges(TVertex vertex) => _vertexEdges.TryGetValue(vertex, out var edges) && edges.Count > 0;

    public IReadOnlyDictionary<TVertex, TEdge> Edges(TVertex vertex)
    {
        return !ContainsVertex(vertex)
            ? new Dictionary<TVertex, TEdge>()
            : _vertexEdges[vertex]
                .Select(t => (Target: t, Edge: GetEdge(vertex, t)))
                .ToDictionary(kv => kv.Target, kv => kv.Edge);
    }

    public (List<TVertex>[] Partitions, List<(TVertex, TVertex)> CutEdges) MinCut()
    {
        var random = new Random();
        var vertices = new TVertex[_vertexEdges.Count];
        var viLookup = new Dictionary<TVertex, int>();
        var subsets = new Subset[_vertexEdges.Keys.Count];

        var v = 0;
        foreach (var vertex in _vertexEdges.Keys)
        {
            subsets[v] = new Subset(v, 0);
            vertices[v] = vertex;
            viLookup.Add(vertex, v);
            v++;
        }

        var edges = _edges.Keys.Select(e => (viLookup[e.Item1]!, viLookup[e.Item2]!)).ToArray();

        var vcount = vertices.Length;
        while (vcount > 2)
        {
            var i = random.Next(edges.Length);

            var subset1 = FindSubset(subsets, edges[i].Item1);
            var subset2 = FindSubset(subsets, edges[i].Item2);

            if (subset1 == subset2)
                continue;

            vcount--;
            Union(subsets, subset1, subset2);
        }

        var cutEdges = new List<(TVertex, TVertex)>();
        for (var i = 0; i < edges.Length; i++)
        {
            var subset1 = FindSubset(subsets, edges[i].Item1);
            var subset2 = FindSubset(subsets, edges[i].Item2);
            if (subset1 != subset2)
            {
                var v1 = vertices[edges[i].Item1];
                var v2 = vertices[edges[i].Item2];
                cutEdges.Add((v1, v2));
            }
        }

        var partitions = vertices.GroupBy(v => FindSubset(subsets, viLookup[v])).Select(e => e.ToList()).ToArray();

        return (partitions, cutEdges);
    }

    private static int FindSubset(Subset[] subset, int v)
    {
        if (subset[v].Parent != v)
            subset[v] = subset[v] with { Parent = FindSubset(subset, subset[v].Parent) };

        return subset[v].Parent;
    }

    private static void Union(Subset[] subsets, int x, int y)
    {
        var xroot = FindSubset(subsets, x);
        var yroot = FindSubset(subsets, y);

        if (subsets[xroot].Rank < subsets[yroot].Rank)
            subsets[xroot] = subsets[xroot] with { Parent = yroot };
        else if (subsets[xroot].Rank > subsets[yroot].Rank)
            subsets[yroot] = subsets[yroot] with { Parent = xroot };
        else
        {
            subsets[yroot] = subsets[yroot] with { Parent = xroot };
            subsets[xroot] = subsets[xroot] with { Rank = subsets[xroot].Rank + 1 };
        }
    }

    private TEdge GetEdge(TVertex a, TVertex b)
    {
        if (_edges.TryGetValue((a, b), out var edge))
            return edge;

        if (_edges.TryGetValue((b, a), out edge))
            return edge;

        throw new ArgumentException("Edge not found");
    }

    private record Subset(int Parent, int Rank);
}