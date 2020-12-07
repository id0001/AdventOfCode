using System.Collections.Generic;

namespace AdventOfCodeLib.Graphs.Collections
{
	public class EdgeList<TVertex, TEdge> : List<TEdge>, IEdgeList<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		public EdgeList()
		{
		}

		public EdgeList(IEnumerable<TEdge> collection) : base(collection)
		{
		}

		public EdgeList(int capacity) : base(capacity)
		{
		}
	}
}
