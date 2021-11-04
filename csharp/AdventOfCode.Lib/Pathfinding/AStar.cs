using AdventOfCode.Lib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public static class AStar
    {
        public static int ManhattanDistance<T>(T node, T target) where T : IPoint
        {
            int value = 0;
            for (int i = 0; i < node.Dimensions; i++)
            {
                value += Math.Abs(node.GetValue(i) - target.GetValue(i));
            }

            return value;
        }
    }

    public class AStar<T> : IPathFinder<T>
    {
        public delegate IEnumerable<(T Item, int Weight)> GetAdjacentNodesFunc(T from);
        public delegate int HeuristicFunc(T node, T target);

        private readonly GetAdjacentNodesFunc getAdjacentNodes;
        private readonly HeuristicFunc heuristic;


        public AStar(GetAdjacentNodesFunc getAdjacentNodes, HeuristicFunc heuristic)
        {
            this.getAdjacentNodes = getAdjacentNodes;
            this.heuristic = heuristic;
        }

        public bool IncludeStart { get; set; }

        public bool TryPath(T start, T end, out T[] path)
        {
            var openQueue = new PriorityQueue<Node>(new NodeComparer(heuristic, end));
            var closedSet = new Dictionary<T, Node>();

            var root = new Node(start, 0);
            openQueue.Enqueue(root);

            while (!openQueue.IsEmpty)
            {
                var current = openQueue.Dequeue();
                if (current.Item.Equals(end))
                {
                    path = GetPath(current);
                    return true;
                }

                closedSet.Add(current.Item, current);

                foreach (var neighbor in getAdjacentNodes(current.Item))
                {
                    int cost = current.Weight + neighbor.Weight;
                    var openChild = openQueue.SingleOrDefault(x => x.Item.Equals(neighbor.Item));
                    if (openChild is object && cost < openChild.Weight)
                    {
                        openQueue.Remove(openChild);
                        openChild = null;
                    }

                    if (closedSet.TryGetValue(neighbor.Item, out Node closedChild) && cost < closedChild.Weight)
                    {
                        closedSet.Remove(neighbor.Item);
                        closedChild = null;
                    }

                    if (openChild is not object && closedChild is not object)
                    {
                        var child = new Node(neighbor.Item, cost);
                        openQueue.Enqueue(child);
                        child.Parent = current;
                    }
                }
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
            private readonly HeuristicFunc heuristic;
            private readonly T target;

            public NodeComparer(HeuristicFunc heuristic, T target)
            {
                this.heuristic = heuristic;
                this.target = target;
            }

            public int Compare(Node x, Node y)
            {
                int f0 = x.Weight + heuristic(x.Item, target);
                int f1 = y.Weight + heuristic(y.Item, target);

                return f0 - f1;
            }
        }
    }
}
