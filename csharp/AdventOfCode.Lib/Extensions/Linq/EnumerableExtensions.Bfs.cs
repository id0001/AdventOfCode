namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static BreadthFirstSearch<IEnumerable<TNode>, TNode> Bfs<TNode>(this IEnumerable<TNode> source, Func<TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<IEnumerable<TNode>, TNode>(source, startNode, getAdjacent);

        public static BreadthFirstSearch<IEnumerable<TGraph>, TNode> Bfs<TGraph, TNode>(this IEnumerable<TGraph> source, Func<IEnumerable<TGraph>, TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<IEnumerable<TGraph>, TNode>(source, startNode, n => getAdjacent(source, n));
    }
}
