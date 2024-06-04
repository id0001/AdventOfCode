using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode.Lib.Extensions
{
    public static class UndirectedGraphExtensions
    {
        public static IEnumerable<(TVertex Vertex, int Distance)> FloodFill<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> source, TVertex start)
            where TVertex : notnull
            where TEdge : notnull
        {
            var bfs = new BreadthFirstSearch<TVertex>(v => source.AdjacentEdges(v).Keys);
            return bfs.FloodFill(start);
        }

        public static (List<TVertex>[] Partitions, List<(TVertex, TVertex)> CutEdges) MinCut<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> source)
            where TVertex : notnull
            where TEdge : notnull
        {
            var random = new Random();
            var vertices = new TVertex[source.VertexCount];
            var viLookup = new Dictionary<TVertex, int>();
            var subsets = new Subset[source.VertexCount];

            var vi = 0;
            foreach (var vertex in source.Vertices)
            {
                subsets[vi] = new Subset(vi, 0);
                vertices[vi] = vertex;
                viLookup.Add(vertex, vi);
                vi++;
            }


            var edges = source.Edges.Select(e => (viLookup[e.Item1], viLookup[e.Item2])).ToArray();

            var vertexCount = vertices.Length;
            while (vertexCount > 2)
            {
                var i = random.Next(edges.Length);

                var subset1 = FindSubset(subsets, edges[i].Item1);
                var subset2 = FindSubset(subsets, edges[i].Item2);

                if (subset1 == subset2)
                    continue;

                vertexCount--;
                Union(subsets, subset1, subset2);
            }

            var cutEdges = new List<(TVertex, TVertex)>();
            for (var i = 0; i < edges.Length; i++)
            {
                var subset1 = FindSubset(subsets, edges[i].Item1);
                var subset2 = FindSubset(subsets, edges[i].Item2);
                if (subset1 == subset2) continue;

                var v1 = vertices[edges[i].Item1];
                var v2 = vertices[edges[i].Item2];
                cutEdges.Add((v1, v2));
            }

            var partitions = vertices.GroupBy(v => FindSubset(subsets, viLookup[v])).Select(e => e.ToList()).ToArray();

            return (partitions, cutEdges);
        }

        private static int FindSubset(IList<Subset> subset, int v)
        {
            if (subset[v].Parent != v)
                subset[v] = subset[v] with { Parent = FindSubset(subset, subset[v].Parent) };

            return subset[v].Parent;
        }

        private static void Union(IList<Subset> subsets, int x, int y)
        {
            var xRoot = FindSubset(subsets, x);
            var yRoot = FindSubset(subsets, y);

            if (subsets[xRoot].Rank < subsets[yRoot].Rank)
            {
                subsets[xRoot] = subsets[xRoot] with { Parent = yRoot };
            }
            else if (subsets[xRoot].Rank > subsets[yRoot].Rank)
            {
                subsets[yRoot] = subsets[yRoot] with { Parent = xRoot };
            }
            else
            {
                subsets[yRoot] = subsets[yRoot] with { Parent = xRoot };
                subsets[xRoot] = subsets[xRoot] with { Rank = subsets[xRoot].Rank + 1 };
            }
        }

        private record Subset(int Parent, int Rank);
    }
}
