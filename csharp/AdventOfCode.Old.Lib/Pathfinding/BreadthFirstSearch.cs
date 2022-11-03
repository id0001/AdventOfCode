using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public class BreadthFirstSearch<T>
    {
        private readonly Queue<Node> queue = new Queue<Node>();
        private readonly HashSet<T> visited = new HashSet<T>();
        private readonly Func<T, IEnumerable<T>> getAdjacentNodes;

        public BreadthFirstSearch(Func<T, IEnumerable<T>> getAdjacentNodes)
        {
            this.getAdjacentNodes = getAdjacentNodes;
        }

        public bool IncludeStart { get; set; }

        public IReadOnlySet<T> Visited => visited;

        public bool TryPath(T start, T end, out T[] path) => TryPath(start, a => a.Equals(end), out path);

        public bool TryPath(T start, Func<T, bool> goalReached, out T[] path)
        {
            try
            {
                Setup();

                queue.Enqueue(new Node(start, null));
                visited.Add(start);

                while (queue.Count > 0)
                {
                    var currentNode = queue.Dequeue();
                    foreach (var adjacent in getAdjacentNodes(currentNode.Item))
                    {
                        if (!visited.Contains(adjacent))
                        {
                            var node = new Node(adjacent, currentNode);
                            visited.Add(adjacent);

                            if (goalReached(adjacent))
                            {
                                path = GetPath(node);
                                return true;
                            }

                            queue.Enqueue(node);
                        }
                    }
                }

                path = Array.Empty<T>();
                return false;
            }
            finally
            {
                Cleanup();
            }
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

        private void Setup()
        {
            queue.Clear();
            visited.Clear();
        }

        private void Cleanup()
        {
            visited.Clear();
        }

        private class Node
        {
            public Node(T item, Node parent)
            {
                Item = item;
                Parent = parent;
            }

            public Node Parent { get; set; }

            public T Item { get; set; }
        }
    }
}
