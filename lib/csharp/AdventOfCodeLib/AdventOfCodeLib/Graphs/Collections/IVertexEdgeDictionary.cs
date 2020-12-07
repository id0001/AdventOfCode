using System.Collections.Generic;

namespace AdventOfCodeLib.Graphs.Collections
{
	public interface IVertexEdgeDictionary<TVertex, TEdge> : IDictionary<TVertex, IEdgeList<TVertex, TEdge>> where TEdge : IEdge<TVertex>
	{
	}
}
