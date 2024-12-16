namespace AdventOfCode.Lib
{
    public static partial class BreadthFirstSearchExtensions
    {
        public static BreadthFirstSearchIgnoreVisited<TGraph, TNode> IgnoreVisited<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source)
            where TNode : notnull
            => new BreadthFirstSearchIgnoreVisited<TGraph, TNode>(source.Source, source.StartNode, source.GetAdjacent);
    }

    public record BreadthFirstSearch<TGraph, TNode>(TGraph Source, TNode StartNode, Func<TNode, IEnumerable<TNode>> GetAdjacent)
        where TNode : notnull;

    public record BreadthFirstSearchIgnoreVisited<TGraph, TNode>(TGraph Source, TNode StartNode, Func<TNode, IEnumerable<TNode>> GetAdjacent)
        where TNode : notnull;

    public record BreadthFirstSearchResult<TNode>(bool Success, IList<TNode> Path)
    {
        public int Distance => Path.Count - 1;
    }
}
