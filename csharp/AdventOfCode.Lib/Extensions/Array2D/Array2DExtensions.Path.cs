namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static BreadthFirstSearch<TGraph[,], TNode> Path<TGraph, TNode>(this TGraph[,] source, TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<TGraph[,], TNode>(source, startNode, getAdjacent);

        public static BreadthFirstSearch<TGraph[,], TNode> Path<TGraph, TNode>(this TGraph[,] source, TNode startNode, Func<TGraph[,], TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<TGraph[,], TNode>(source, startNode, n => getAdjacent(source, n));
    }
}
