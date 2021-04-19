using AdventOfCode.Lib.Graphs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Graphs
{
	public class BidirectionalGraph<TVertex, TEdge> :
		IGraph<TVertex, TEdge>,
		IMutableGraph<TVertex, TEdge>,
		IBidirectionalGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		private int edgeCount = 0;
		private readonly IVertexEdgeDictionary<TVertex, TEdge> vertexOutEdges;
		private readonly IVertexEdgeDictionary<TVertex, TEdge> vertexInEdges;

		public BidirectionalGraph()
		{
			this.vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>();
			this.vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>();
		}

		public bool IsDirected => true;

		public int VertexCount => vertexOutEdges.Count;

		public int EdgeCount => edgeCount;

		public IEnumerable<TVertex> Vertices => vertexOutEdges.Keys;

		public IEnumerable<TEdge> Edges => vertexOutEdges.Values.SelectMany(e => e);

		public bool AddVertex(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (ContainsVertex(vertex))
				return false;

			vertexOutEdges.Add(vertex, new EdgeList<TVertex, TEdge>());
			vertexInEdges.Add(vertex, new EdgeList<TVertex, TEdge>());
			return true;
		}

		public bool RemoveVertex(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (!ContainsVertex(vertex))
				return false;

			int removed = 0;
			foreach (var outEdge in OutEdges(vertex))
			{
				vertexInEdges[outEdge.Target].Remove(outEdge);
				removed++;
			}

			foreach (var inEdge in InEdges(vertex))
			{
				if (vertexOutEdges[inEdge.Target].Remove(inEdge))
					removed++;
			}

			vertexOutEdges.Remove(vertex);
			vertexInEdges.Remove(vertex);
			edgeCount -= removed;
			return true;
		}

		public bool AddEdge(TEdge edge)
		{
			if (edge == null)
				throw new ArgumentNullException(nameof(edge));

			if (ContainsEdge(edge.Source, edge.Target))
				return false;

			vertexOutEdges[edge.Source].Add(edge);
			vertexInEdges[edge.Target].Add(edge);
			edgeCount++;
			return true;
		}

		public bool RemoveEdge(TEdge edge)
		{
			if (edge == null)
				throw new ArgumentNullException(nameof(edge));

			if (vertexOutEdges[edge.Source].Remove(edge))
			{
				vertexInEdges[edge.Target].Remove(edge);
				edgeCount--;
				return true;
			}

			return false;
		}

		public bool ContainsVertex(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexOutEdges.ContainsKey(vertex);
		}

		public bool ContainsEdge(TVertex source, TVertex target)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (target == null)
				throw new ArgumentNullException(nameof(target));

			if (!TryGetOutEdges(source, out var outEdges))
				return false;

			foreach (var outEdge in outEdges)
			{
				if (outEdge.Target.Equals(target))
					return true;
			}

			return false;
		}

		public bool HasOutEdges(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexOutEdges[vertex].Count > 0;
		}

		public TEdge OutEdge(TVertex vertex, int index)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexOutEdges[vertex][index];
		}

		public IEnumerable<TEdge> OutEdges(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexOutEdges[vertex];
		}

		public bool TryGetOutEdge(TVertex vertex, int index, out TEdge edge)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (vertexOutEdges.TryGetValue(vertex, out var edgeList) && index >= 0 && index < edgeList.Count)
			{
				edge = edgeList[index];
				return true;
			}

			edge = default;
			return false;
		}

		public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (vertexOutEdges.TryGetValue(vertex, out var list))
			{
				edges = list;
				return true;
			}

			edges = default;
			return false;
		}

		public bool HasInEdges(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexInEdges[vertex].Count > 0;
		}

		public TEdge InEdge(TVertex vertex, int index)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexInEdges[vertex][index];
		}

		public IEnumerable<TEdge> InEdges(TVertex vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			return vertexInEdges[vertex];
		}

		public bool TryGetInEdge(TVertex vertex, int index, out TEdge edge)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (vertexInEdges.TryGetValue(vertex, out var edgeList) && index >= 0 && index < edgeList.Count)
			{
				edge = edgeList[index];
				return true;
			}

			edge = default;
			return false;
		}

		public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
		{
			if (vertex == null)
				throw new ArgumentNullException(nameof(vertex));

			if (vertexInEdges.TryGetValue(vertex, out var list))
			{
				edges = list;
				return true;
			}

			edges = default;
			return false;
		}
	}
}
