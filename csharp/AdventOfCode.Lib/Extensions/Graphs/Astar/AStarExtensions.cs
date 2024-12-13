namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static AStar<TGraph, TNode> WithWeight<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TNode, TNode, int> getWeight)
            where TNode : notnull
           => source with { GetWeight = getWeight };

        public static AStar<TGraph, TNode> WithWeight<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TGraph, TNode, TNode, int> getWeight)
            where TNode : notnull
           => source with { GetWeight = (a, b) => getWeight(source.Source, a, b) };

        public static AStar<TGraph, TNode> WithHeuristic<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TNode, int> getHeuristic)
            where TNode : notnull
           => source with { GetHeuristic = getHeuristic };
    }

    public record AStar<TGraph, TNode>(TGraph Source, Func<TNode, IEnumerable<TNode>> GetAdjacent, TNode StartNode, Func<TNode, TNode, int> GetWeight, Func<TNode, int> GetHeuristic)
        where TNode : notnull;
}
