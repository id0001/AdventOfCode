using AdventOfCode.Lib.Graphs;

namespace AdventOfCode.Lib
{
    public static partial class UndirectedGraphExtensions
    {
        public static BreadthFirstSearch<TVertex> Bfs<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> source, TVertex startNode)
            where TVertex : notnull
            where TEdge : notnull
            => new BreadthFirstSearch<TVertex>(v => source.AdjacentEdges(v).Keys, startNode);
    }
}
