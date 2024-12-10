namespace AdventOfCode.Lib
{
    public static partial class BreadthFirstSearchExtensions
    {
        public static BreadthFirstSearchIgnoreVisited<TNode> IgnoreVisited<TNode>(this BreadthFirstSearch<TNode> source)
            where TNode : notnull
            => new BreadthFirstSearchIgnoreVisited<TNode>(source.GetAdjacent, source.StartNode);
    }

    public record BreadthFirstSearch<TNode>(Func<TNode, IEnumerable<TNode>> GetAdjacent, TNode StartNode)
        where TNode : notnull;

    public record BreadthFirstSearchIgnoreVisited<TNode>(Func<TNode, IEnumerable<TNode>> GetAdjacent, TNode StartNode)
        where TNode : notnull;
}
