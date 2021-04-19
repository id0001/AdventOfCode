using System;

namespace AdventOfCode.Lib.Graphs
{
	public interface IMutableGraph<TVertex, TEdge> : IGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		bool AddVertex(TVertex vertex);

		bool RemoveVertex(TVertex vertex);

		bool AddEdge(TEdge edge);

		bool RemoveEdge(TEdge edge);
	}
}
