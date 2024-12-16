namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static AStar<TGraph[,], TNode> AStar<TGraph, TNode>(this TGraph[,] source, TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new AStar<TGraph[,], TNode>(source, startNode, getAdjacent, (_, _) => 1, _ => 0);

        public static AStar<TGraph[,], TNode> AStar<TGraph, TNode>(this TGraph[,] source, TNode startNode, Func<TGraph[,], TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new AStar<TGraph[,], TNode>(source, startNode, n => getAdjacent(source, n), (_, _) => 1, _ => 0);
    }
}
