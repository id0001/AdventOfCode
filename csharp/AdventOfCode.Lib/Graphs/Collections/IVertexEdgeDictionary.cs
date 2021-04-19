using System.Collections.Generic;

namespace AdventOfCode.Lib.Graphs.Collections
{
	public interface IVertexEdgeDictionary<TVertex, TEdge> : IDictionary<TVertex, IEdgeList<TVertex, TEdge>> where TEdge : IEdge<TVertex>
	{
	}
}
