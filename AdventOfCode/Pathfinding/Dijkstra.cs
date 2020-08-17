using AdventOfCode.DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Pathfinding
{
	internal class Dijkstra
	{
		public Dijkstra()
		{
		}

		/// <summary>
		/// Calculate the shortest path from start to end.
		/// </summary>
		/// <param name="graph">The graph with known nodes</param>
		/// <param name="start">The start node</param>
		/// <param name="end">The target node</param>
		/// <returns>An array containing the path to target. Excludes the start node. If not path was found, and empty array is returned.</returns>
		public static Point[] Path(IDictionary<Point, bool> graph, Point start, Point end)
		{
			if (graph == null)
				throw new ArgumentNullException(nameof(graph));

			if (start == end)
				throw new ArgumentException("Start cannot be the same as finish");

			if (!graph.ContainsKey(start))
				throw new ArgumentException("Start location is not in the graph");

			if (!graph[start])
				throw new ArgumentException("Start location is not traversable");

			if (!graph.ContainsKey(end))
				throw new ArgumentException("End location is not in the graph");

			if (!graph[end])
				throw new ArgumentException("End location is not traversable");

			var distances = new Dictionary<Point, int>();
			var previous = new Dictionary<Point, Point>();
			var openQueue = new PriorityQueue<Point>(new MinDistComparer(distances));
			var visited = new HashSet<Point>();

			distances[start] = 0;
			openQueue.Enqueue(start);

			while (!openQueue.IsEmpty)
			{
				var node = openQueue.Dequeue();

				visited.Add(node);

				if (node == end)
					break;

				var neighbors = GetNeighbours(graph, node);
				foreach (var neighbor in neighbors)
				{
					int dist = distances[node] + 1;
					if (!distances.ContainsKey(neighbor) || dist < distances[neighbor])
					{
						distances[neighbor] = dist;
						previous[neighbor] = node;

						if (!visited.Contains(neighbor))
							openQueue.Enqueue(neighbor);
					}
				}
			}

			if (!visited.Contains(end))
				return new Point[0];

			var path = new List<Point>();
			Point p = end;
			while (p != start)
			{
				path.Insert(0, p);
				p = previous[p];
			}

			return path.ToArray();
		}

		/// <summary>
		/// Calculate the shortest path from start to end.
		/// </summary>
		/// <param name="graph">The graph with known nodes</param>
		/// <param name="start">The start node</param>
		/// <param name="end">The target node</param>
		/// <param name="path">An array containing the path to target. Excludes the start node.</param>
		/// <returns>True if a path was found. False if not</returns>
		public static bool TryPath(IDictionary<Point, bool> graph, Point start, Point end, out Point[] path)
		{
			path = Path(graph, start, end);
			return path.Length != 0;
		}

		/// <summary>
		/// Get the neighbour nodes.
		/// </summary>
		/// <param name="graph">The graph with known  nodes</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>A list of neighbour nodes.</returns>
		private static IEnumerable<Point> GetNeighbours(IDictionary<Point, bool> graph, Point parent)
		{
			List<Point> neighbours = new List<Point>();
			for (int y = parent.Y - 1; y <= parent.Y + 1; y++)
			{
				for (int x = parent.X - 1; x <= parent.X + 1; x++)
				{
					if (x == parent.X ^ y == parent.Y && graph.TryGetValue(parent, out bool v) && v)
					{
						var p = new Point(x, y);
						neighbours.Add(p);
					}
				}
			}

			return neighbours;
		}

		private class MinDistComparer : IComparer<Point>
		{
			private readonly IDictionary<Point, int> distances;

			public MinDistComparer(IDictionary<Point, int> distances)
			{
				this.distances = distances;
			}

			public int Compare([AllowNull] Point x, [AllowNull] Point y)
			{
				if (!distances.ContainsKey(x) || !distances.ContainsKey(y))
					throw new InvalidOperationException("Unable to compare distance of unknown points.");

				return distances[x] - distances[y];
			}
		}
	}
}
