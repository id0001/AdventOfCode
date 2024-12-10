namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static AStar<TNode> WithWeight<TNode>(this AStar<TNode> source, Func<TNode, TNode, int> getWeight)
            where TNode : notnull
           => source with { GetWeight = getWeight };

        public static AStar<TNode> WithHeuristic<TNode>(this AStar<TNode> source, Func<TNode, int> getHeuristic)
            where TNode : notnull
           => source with { GetHeuristic = getHeuristic };
    }

    public record AStar<TNode>(Func<TNode, IEnumerable<TNode>> GetAdjacent, TNode StartNode, Func<TNode, TNode, int> GetWeight, Func<TNode, int> GetHeuristic)
        where TNode : notnull;
}
