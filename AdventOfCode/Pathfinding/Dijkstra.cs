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

		public static Point[] Path(IDictionary<Point, bool> graph, Point start, Point finish)
		{
			if (graph == null)
				throw new ArgumentNullException(nameof(graph));

			if (start == finish)
				throw new ArgumentException("Start cannot be the same as finish");

			if (!graph.ContainsKey(start))
				throw new ArgumentException("Start location is not in the graph");

			if (!graph[start])
				throw new ArgumentException("Start location is not traversable");

			if (!graph.ContainsKey(finish))
				throw new ArgumentException("Finish location is not in the graph");

			if (!graph[finish])
				throw new ArgumentException("Finish location is not traversable");

			List<Point> finalPath = new List<Point>();

			ISet<Point> closedPoints = new HashSet<Point>();
			PriorityQueue<Node> openQueue = new PriorityQueue<Node>(new PriorityComparer());

			var startNode = new Node { Point = start, Score = 0 };
			openQueue.Enqueue(startNode);

			while (!openQueue.IsEmpty)
			{
				Node node = openQueue.Dequeue();

				if (node.Point == finish)
				{
					do
					{
						finalPath.Add(node.Point);
						node = node.Parent;
					}
					while (node != null);
					break;
				}

				closedPoints.Add(node.Point);
				AddAdjacentNodes(graph, closedPoints, openQueue, node);
			}

			return finalPath.Reverse<Point>().ToArray();
		}

		public static bool TryPath(IDictionary<Point, bool> graph, Point start, Point finish, out Point[] path)
		{
			path = Path(graph, start, finish);
			return path.Length != 0;
		}

		private static void AddAdjacentNodes(IDictionary<Point, bool> graph, ISet<Point> closedPoints, PriorityQueue<Node> openNodes, Node parent)
		{
			Point location = parent.Point;
			for (int y = location.Y - 1; y <= location.Y + 1; y++)
			{
				for (int x = location.X - 1; x <= location.X + 1; x++)
				{
					var p = new Point(x, y);

					if (x == location.X ^ y == location.Y && !closedPoints.Contains(p) && graph.TryGetValue(p, out bool v) && v)
					{
						int score = parent.Score + 1;
						openNodes.Enqueue(new Node
						{
							Point = p,
							Score = score,
							Parent = parent,
						});
					}
				}
			}
		}

		private class Node : IEquatable<Node>
		{
			public Point Point { get; set; }

			public int Score { get; set; }

			public Node Parent { get; set; }

			public override bool Equals(object obj) => obj is Node n && Equals(n);

			public override int GetHashCode() => HashCode.Combine(Point, Score);

			public bool Equals([AllowNull] Node other) => GetHashCode() == other.GetHashCode();
		}

		private class PriorityComparer : IComparer<Node>
		{
			public int Compare([AllowNull] Node x, [AllowNull] Node y)
			{
				return x.Score - y.Score;
			}
		}
	}
}
