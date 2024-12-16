using AdventOfCode.Lib.Graphs;

namespace AdventOfCode.Lib
{
    public static partial class UndirectedGraphExtensions
    {
        public static BreadthFirstSearch<UndirectedGraph<TVertex, TEdge>, TVertex> Bfs<TVertex, TEdge>(this UndirectedGraph<TVertex, TEdge> source, TVertex startNode)
            where TVertex : notnull
            where TEdge : notnull
            => new BreadthFirstSearch<UndirectedGraph<TVertex, TEdge>, TVertex>(source, startNode, v => source.AdjacentEdges(v).Keys);
    }
}
