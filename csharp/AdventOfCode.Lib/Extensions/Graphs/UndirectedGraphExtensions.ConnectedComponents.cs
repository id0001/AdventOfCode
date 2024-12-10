using AdventOfCode.Lib.Graphs;

namespace AdventOfCode.Lib.Extensions.Graphs
{
    public static partial class UndirectedGraphExtensions
    {
        public static IEnumerable<IEnumerable<TVertex>> ConnectedComponents<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : notnull
        {
            var candidates = graph.Vertices.ToHashSet();
            while (candidates.Count > 0)
            {
                var cluster = graph.FloodFill(candidates.First());
                foreach (var v in cluster)
                    candidates.Remove(v);

                yield return cluster;
            }
        }
    }
}
