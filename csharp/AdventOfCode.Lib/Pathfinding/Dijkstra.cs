using AdventOfCode.Lib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public class Dijkstra<T> : IPathFinder<T>
    {
        public delegate IEnumerable<(T Item, int Weight)> GetAdjacentNodesFunc(T from);

        private readonly GetAdjacentNodesFunc getAdjacentNodes;

        public Dijkstra(GetAdjacentNodesFunc getAdjacentNodes)
        {
            this.getAdjacentNodes = getAdjacentNodes;
        }

        public bool IncludeStart { get; set; }

        public bool TryPath(T start, T end, out T[] path)
        {
            var nodes = new Dictionary<T, Node>();
            var pq = new PriorityQueue<Node>(new NodeComparer());

            var root = new Node(start, 0);
            nodes.Add(start, root);
            pq.Enqueue(root);

            while (!pq.IsEmpty)
            {
                var currentNode = pq.Dequeue();
                if (currentNode.Item.Equals(end))
                    break;

                foreach (var adjacent in getAdjacentNodes(currentNode.Item))
                {
                    Node childNode;
                    if (!nodes.TryGetValue(adjacent.Item, out childNode))
                    {
                        childNode = new Node(adjacent.Item, int.MaxValue);
                        nodes[adjacent.Item] = childNode;
                    }

                    if (childNode.Weight > currentNode.Weight + adjacent.Weight)
                    {
                        childNode.Weight = currentNode.Weight + adjacent.Weight;
                        childNode.Parent = currentNode;
                        pq.Enqueue(childNode);
                    }
                }
            }

            if (nodes.ContainsKey(end))
            {
                path = GetPath(nodes[end]);
                return true;
            }

            path = Array.Empty<T>();
            return false;
        }

        private T[] GetPath(Node node)
        {
            List<T> path = new List<T>();
            Node current = node;
            while (current != null)
            {
                path.Add(current.Item);
                current = current.Parent;
            }

            path.Reverse();
            return IncludeStart
                ? path.ToArray()
                : path.Skip(1).ToArray();
        }

        private class Node
        {
            public Node(T item, int weight)
            {
                Item = item;
                Weight = weight;
            }

            public T Item { get; }

            public int Weight { get; set; }

            public Node Parent { get; set; }
        }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                return x.Weight - y.Weight;
            }
        }
    }
}
