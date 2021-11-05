using AdventOfCode.Lib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public class Dijkstra<T>
    {
        public delegate IEnumerable<(T Item, int Weight)> GetAdjacentNodesFunc(T from);

        private readonly GetAdjacentNodesFunc getAdjacentNodes;

        public Dijkstra(GetAdjacentNodesFunc getAdjacentNodes)
        {
            this.getAdjacentNodes = getAdjacentNodes;
        }

        public bool IncludeStart { get; set; }

        public bool TryPath(T start, T end, out T[] path, out int shortestPathLength) => TryPath(start, n => n.Equals(end), out path, out shortestPathLength);

        public bool TryPath(T start, Func<T, bool> goalReached, out T[] path, out int shortestPathLength)
        {
            var queue = new PriorityQueue<ItemDistancePair>(new ItemWeightPairComparer());
            var distanceMap = new Dictionary<T, int>();
            var parentToChildMap = new Dictionary<T, T>();

            var root = new ItemDistancePair(start, 0);
            distanceMap.Add(start, 0);
            queue.Enqueue(root);

            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                if (goalReached(current.Item))
                {
                    path = GetPath(current);
                    shortestPathLength = current.Distance;
                    return true;
                }

                foreach (var adjacent in getAdjacentNodes(current.Item))
                {
                    if (!distanceMap.TryGetValue(adjacent.Item, out var distance))
                    {
                        distance = int.MaxValue;
                    }

                    if (distance > current.Distance + adjacent.Weight)
                    {
                        distanceMap[adjacent.Item] = current.Distance + adjacent.Weight;
                        parentToChildMap[adjacent.Item] = current.Item;
                        queue.Enqueue(new ItemDistancePair(adjacent.Item, current.Distance + adjacent.Weight));
                    }
                }
            }

            path = Array.Empty<T>();
            shortestPathLength = 0;
            return false;
        }

        private T[] GetPath(ItemDistancePair node)
        {
            List<T> path = new List<T>();
            ItemDistancePair current = node;
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

        private class ItemDistancePair
        {
            public ItemDistancePair(T item, int weight)
            {
                Item = item;
                Distance = weight;
            }

            public T Item { get; }

            public int Distance { get; set; }

            public ItemDistancePair Parent { get; set; }
        }

        private class ItemWeightPairComparer : IComparer<ItemDistancePair>
        {
            public int Compare(ItemDistancePair x, ItemDistancePair y)
            {
                return x.Distance - y.Distance;
            }
        }
    }
}
