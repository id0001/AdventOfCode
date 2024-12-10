using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static BreadthFirstSearch<TNode> Bfs<TNode>(this IEnumerable<TNode> source, Func<TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<TNode>(getAdjacent, startNode);

        public static BreadthFirstSearch<TNode> Bfs<TGraph, TNode>(this IEnumerable<TGraph> source, Func<IEnumerable<TGraph>, TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<TNode>(n => getAdjacent(source, n), startNode);
    }
}
