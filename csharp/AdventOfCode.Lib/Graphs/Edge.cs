using Microsoft;

namespace AdventOfCode.Lib.Graphs;

public class Edge<TVertex> where TVertex : notnull
{
    public Edge(TVertex source, TVertex  target)
    {
        Requires.Argument(!EqualityComparer<TVertex>.Default.Equals(source, target), nameof(target), "Vertices cannot be the same");
        Source = source;
        Target = target;
    }

    public TVertex Source { get; }
    
    public TVertex Target { get; }
}