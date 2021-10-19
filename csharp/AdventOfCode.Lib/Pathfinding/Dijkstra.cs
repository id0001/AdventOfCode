using AdventOfCode.Lib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public class Dijkstra<T>
    {
        private readonly Func<T, IEnumerable<Node>> getNeighbors;

        public Dijkstra(Func<T, IEnumerable<Node>> getNeighbors)
        {
            this.getNeighbors = getNeighbors;
        }

        public bool TryPath(T start, T end, out T[] path)
        {
            var distances = new Dictionary<T, int>();
            var openQueue = new PriorityQueue<Node>(new MinDistanceComparer(distances));
            var visited = new HashSet<Node>();
            var previous = new Dictionary<T, T>();

            openQueue.Enqueue(new Node(start, 0));
            distances[start] = 0;

            while (!openQueue.IsEmpty)
            {
                var node = openQueue.Dequeue();
                visited.Add(node);

                if (node.Item.Equals(end))
                    break;

                var neighbors = getNeighbors(node.Item);
                foreach (var neighbor in neighbors)
                {
                    int distance = distances[node.Item] + neighbor.Weight;
                    if (!distances.ContainsKey(neighbor.Item) || distance < distances[neighbor.Item])
                    {
                        distances[neighbor.Item] = distance;
                        previous[neighbor.Item] = node.Item;

                        if (!visited.Contains(neighbor))
                            openQueue.Enqueue(neighbor);

                    }
                }
            }

            if (!visited.Any(x => x.Item.Equals(end)))
            {
                path = Array.Empty<T>();
                return false;
            }

            path = GetPath(previous, start, end);
            return true;
        }

        private T[] GetPath(Dictionary<T, T> previous, T start, T end)
        {
            var path = new List<T>();
            T p = end;
            while (!p.Equals(start))
            {
                path.Add(p);
                p = previous[p];
            }

            return path.Reverse<T>().ToArray();
        }

        public record Node(T Item, int Weight);

        private class MinDistanceComparer : IComparer<Node>
        {
            private readonly IDictionary<T, int> distances;

            public MinDistanceComparer(IDictionary<T, int> distances)
            {
                this.distances = distances;
            }

            public int Compare(Node x, Node y)
            {
                if (!distances.ContainsKey(x.Item) || !distances.ContainsKey(y.Item))
                {
                    throw new InvalidOperationException("Unable to compare distance of unknown nodes.");
                }

                return distances[x.Item] - distances[y.Item];
            }
        }
    }
}
