namespace AdventOfCode.Lib;

public static partial class AStarExtensions
{
    public static AStar<TGraph, TNode> WithHeuristic<TGraph, TNode>(this AStar<TGraph, TNode> source,
        Func<TNode, int> getHeuristic)
        where TNode : notnull
        => source with {GetHeuristic = getHeuristic};

    private static IEnumerable<TNode> GetPath<TNode>(TNode end, IDictionary<TNode, TNode> previous)
        where TNode : notnull
    {
        var stack = new Stack<TNode>();
        var current = end;
        do
        {
            stack.Push(current);
        } while (previous.TryGetValue(current, out current));

        return stack;
    }

    private static IList<IList<TNode>> GetPaths<TNode>(TNode start, TNode node,
        IDictionary<TNode, HashSet<TNode>> previous)
        where TNode : notnull
    {
        if (node.Equals(start))
            return [[start]];

        var paths = new List<IList<TNode>>();
        foreach (var previousNode in previous[node])
        foreach (var path in GetPaths(start, previousNode, previous))
            paths.Add([.. path, node]);

        return paths;
    }
}

public record AStar<TGraph, TNode>(
    TGraph Source,
    TNode StartNode,
    Func<TNode, IEnumerable<TNode>> GetAdjacent,
    Func<TNode, TNode, int> GetWeight,
    Func<TNode, int> GetHeuristic)
    where TNode : notnull;

public record AStarResult<TNode>(bool Success, int Cost, IList<TNode> Path);