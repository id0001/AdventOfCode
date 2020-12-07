using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventOfCodeLib.Graphs.Collections
{
	public class VertexEdgeDictionary<TVertex, TEdge> : Dictionary<TVertex, IEdgeList<TVertex, TEdge>>, IVertexEdgeDictionary<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		public VertexEdgeDictionary()
		{
		}

		public VertexEdgeDictionary(int capacity) : base(capacity)
		{
		}
	}
}
