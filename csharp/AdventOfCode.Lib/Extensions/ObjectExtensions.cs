namespace AdventOfCode.Lib;

public static class ObjectExtensions
{
    public static T As<T>(this IConvertible source)
        where T : IConvertible
    {
        return (T)Convert.ChangeType(source, typeof(T));
    }

    public static TOut Into<TIn, TOut>(this TIn source, Func<TIn, TOut> converter)
        where TIn : notnull
        where TOut : notnull
        => converter(source);

    public static AStar<TGraph, TNode> AStar<TGraph, TNode>(this TGraph source, Func<TGraph, TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new AStar<TGraph, TNode>(source, n => getAdjacent(source, n), startNode, (_, _) => 1, _ => 0);

    public static BreadthFirstSearch<TNode> Bfs<TGraph, TNode>(this TGraph source, Func<TGraph, TNode, IEnumerable<TNode>> getAdjacent, TNode startNode)
            where TNode : notnull
            => new BreadthFirstSearch<TNode>(c => getAdjacent(source, c), startNode);
}