
namespace AdventOfCodeLib.Graphs
{
	public class Edge<TVertex> : IEdge<TVertex>
	{
		public Edge(TVertex source, TVertex target)
		{
			Source = source;
			Target = target;
		}

		public TVertex Source { get; }

		public TVertex Target { get; }

		public override string ToString() => $"{Source} -> {Target}";
	}
}
