namespace AdventOfCode.Lib.Graphs
{
	public interface IEdge<TVertex>
	{
		TVertex Source { get; }

		TVertex Target { get; }
	}
}
