using System.Collections.Generic;

namespace AdventOfCodeLib.Graphs.Collections
{
	public interface IEdgeList<TVertex, TEdge> : IList<TEdge> where TEdge : IEdge<TVertex>
	{
	}
}
