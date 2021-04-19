using System.Collections.Generic;

namespace AdventOfCode.Lib.Graphs.Collections
{
	public interface IEdgeList<TVertex, TEdge> : IList<TEdge> where TEdge : IEdge<TVertex>
	{
	}
}
