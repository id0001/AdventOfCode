using System;
using System.Collections.Generic;

namespace AdventOfCode.Lib.Graphs
{
	public interface IBidirectionalGraph<TVertex, TEdge> : IGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		public TEdge OutEdge(TVertex vertex, int index);

		public IEnumerable<TEdge> OutEdges(TVertex vertex);

		public TEdge InEdge(TVertex vertex, int index);

		public IEnumerable<TEdge> InEdges(TVertex vertex);
	}
}
