namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static BreadthFirstSearch<TNode> Bfs<TNode>(this TNode[,] source, Func<TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<TNode>(getAdjacent, startNode);

        public static BreadthFirstSearch<TNode> Bfs<TGraph, TNode>(this TGraph[,] source, Func<TGraph[,], TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<TNode>(c => getAdjacent(source, c), startNode);
    }
}
