namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static AStar<IEnumerable<TGraph>, TNode> AStar<TGraph, TNode>(this IEnumerable<TGraph> source, TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new AStar<IEnumerable<TGraph>, TNode>(source, startNode, getAdjacent, (_, _) => 1, _ => 0);

        public static AStar<IEnumerable<TGraph>, TNode> AStar<TGraph, TNode>(this IEnumerable<TGraph> source, TNode startNode, Func<IEnumerable<TGraph>, TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new AStar<IEnumerable<TGraph>, TNode>(source, startNode, n => getAdjacent(source, n), (_, _) => 1, _ => 0);
    }
}
