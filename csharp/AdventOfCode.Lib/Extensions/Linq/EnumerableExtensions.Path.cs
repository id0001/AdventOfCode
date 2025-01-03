﻿namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static BreadthFirstSearch<IEnumerable<TNode>, TNode> Path<TNode>(this IEnumerable<TNode> source,
        TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent) where TNode : notnull =>
        new(source, startNode, getAdjacent);

    public static BreadthFirstSearch<IEnumerable<TGraph>, TNode> Path<TGraph, TNode>(this IEnumerable<TGraph> source,
        TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent) where TNode : notnull =>
        new(source, startNode, getAdjacent);

    public static BreadthFirstSearch<IEnumerable<TGraph>, TNode> Path<TGraph, TNode>(this IEnumerable<TGraph> source,
        TNode startNode, Func<IEnumerable<TGraph>, TNode, IEnumerable<TNode>> getAdjacent)
        where TNode : notnull
    {
        var list = source.ToList();
        return new BreadthFirstSearch<IEnumerable<TGraph>, TNode>(list, startNode, n => getAdjacent(list, n));
    }

    public static BreadthFirstSearch<ISet<TGraph>, TNode> Path<TGraph, TNode>(this ISet<TGraph> source, TNode startNode,
        Func<ISet<TGraph>, TNode, IEnumerable<TNode>> getAdjacent) where TNode : notnull =>
        new(source, startNode, n => getAdjacent(source, n));

    public static BreadthFirstSearch<IList<TGraph>, TNode> Path<TGraph, TNode>(this IList<TGraph> source,
        TNode startNode, Func<IList<TGraph>, TNode, IEnumerable<TNode>> getAdjacent) where TNode : notnull =>
        new(source, startNode, n => getAdjacent(source, n));
}