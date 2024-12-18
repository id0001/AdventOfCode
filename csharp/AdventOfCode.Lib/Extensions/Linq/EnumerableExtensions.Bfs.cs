﻿namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static BreadthFirstSearch<IEnumerable<TNode>, TNode> Bfs<TNode>(this IEnumerable<TNode> source,TNode startNode, Func<TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<IEnumerable<TNode>, TNode>(source, startNode, getAdjacent);

        public static BreadthFirstSearch<IEnumerable<TGraph>, TNode> Bfs<TGraph, TNode>(this IEnumerable<TGraph> source, TNode startNode, Func<IEnumerable<TGraph>, TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<IEnumerable<TGraph>, TNode>(source, startNode, n => getAdjacent(source, n));
        
        public static BreadthFirstSearch<ISet<TGraph>, TNode> Bfs<TGraph, TNode>(this ISet<TGraph> source,TNode startNode, Func<ISet<TGraph>, TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<ISet<TGraph>, TNode>(source, startNode, n => getAdjacent(source, n));
        
        public static BreadthFirstSearch<IList<TGraph>, TNode> Bfs<TGraph, TNode>(this IList<TGraph> source, TNode startNode, Func<IList<TGraph>, TNode, IEnumerable<TNode>> getAdjacent)
            where TNode : notnull
            => new BreadthFirstSearch<IList<TGraph>, TNode>(source, startNode, n => getAdjacent(source, n));
    }
}
