﻿using AdventOfCode.Lib.Graphs;

namespace AdventOfCode.Lib;

public static partial class UndirectedGraphExtensions
{
    public static BreadthFirstSearch<UndirectedGraph<TVertex, TEdge>, TVertex> Path<TVertex, TEdge>(
        this UndirectedGraph<TVertex, TEdge> source, TVertex startNode)
        where TVertex : notnull
        where TEdge : notnull
        => new(source, startNode, v => source.AdjacentEdges(v).Keys);
}