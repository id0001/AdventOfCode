using AdventOfCode.Lib.Graphs;
using System.Collections.Immutable;

namespace AdventOfCode.Lib
{
    public static partial class UndirectedGraphExtensions
    {
        public static List<List<TVertex>> EnumerateCliques<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> source)
            where TVertex : notnull
            where TEdge : notnull
        {
            var r = ImmutableHashSet<TVertex>.Empty;
            var p = source.Vertices.ToImmutableHashSet();
            var x = ImmutableHashSet<TVertex>.Empty;

            var cliques = new List<List<TVertex>>();
            BronKerbosch(source, cliques, r, p, x);
            return cliques;
        }

        private static void BronKerbosch<TVertex, TEdge>(UndirectedGraph<TVertex, TEdge> graph, List<List<TVertex>> cliques, ImmutableHashSet<TVertex> r, ImmutableHashSet<TVertex> p, ImmutableHashSet<TVertex> x)
            where TVertex : notnull
            where TEdge : notnull
        {
            if (p.Count == 0 && x.Count == 0)
            {
                cliques.Add([.. r]);
                return;
            }

            var pivot = p.Union(x).First();
            foreach (var v in p.Except(graph.AdjacentEdges(pivot).Keys))
            {
                BronKerbosch(graph, cliques, r.Add(v), p.Intersect(graph.AdjacentEdges(v).Keys), x.Intersect(graph.AdjacentEdges(v).Keys));
                p = p.Remove(v);
                x = x.Add(v);
            }
        }
    }
}
