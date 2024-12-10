namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static AStar<TNode> AStar<TNode>(this TNode[,] source, Func<TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new AStar<TNode>(getAdjacent, startNode, (_, _) => 1, _ => 0);

        public static AStar<TNode> AStar<TGraph, TNode>(this TGraph[,] source, Func<TGraph[,], TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new AStar<TNode>(n => getAdjacent(source, n), startNode, (_, _) => 1, _ => 0);
    }
}
