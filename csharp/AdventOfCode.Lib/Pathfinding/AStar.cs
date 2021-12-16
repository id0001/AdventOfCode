using AdventOfCode.Lib.Collections;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Pathfinding
{
    public static class AStar
    {
        public static Func<T, int> ManhattanDistance<T>(T target) where T : IPoint => new Func<T, int>(a =>
        {
            int value = 0;
            for (int d = 0; d < a.Dimensions; d++)
            {
                value += Math.Abs(a.GetValue(d) - target.GetValue(d));
            }

            return value;
        });
    }

    public class AStar<T>
    {
        public AStar(Func<T, IEnumerable<(T, int)>> getAdjecentFunc)
        {
            GetAdjecentFunc = Requires.NotNull(getAdjecentFunc, nameof(getAdjecentFunc));
        }

        public Func<T, IEnumerable<(T, int)>> GetAdjecentFunc { get; }

        public bool IncludeStartInPath { get; set; }

        public bool TryPath(T start, T end, Func<T, int> heuristicFunc, out AStarResult<T> result) => TryPath(start, n => n.Equals(end), heuristicFunc, out result);

        public bool TryPath(T start, Func<T, bool> goalReachedPredicate, Func<T, int> heuristicFunc, out AStarResult<T> result)
        {
            Requires.NotNull(heuristicFunc, nameof(heuristicFunc));
            Requires.NotNull(goalReachedPredicate, nameof(goalReachedPredicate));

            var queue = new PriorityQueue<Node>(new PriorityComparer());
            var childToParent = new Dictionary<T, T>();
            var itemToCost = new Dictionary<T, int>();

            var root = new Node(start, 0);
            itemToCost.Add(start, 0);
            queue.Enqueue(root);

            int visited = 0;
            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                visited++;

                if (goalReachedPredicate(current.Item))
                {
                    result = GetResult(current.Item, childToParent, itemToCost[current.Item], visited);
                    return true;
                }

                foreach (var (adjecent, cost) in GetAdjecentFunc(current.Item))
                {
                    int newCost = itemToCost[current.Item] + cost;
                    if (!itemToCost.TryGetValue(adjecent, out var lastCost))
                    {
                        lastCost = int.MaxValue;
                    }

                    if (lastCost > newCost)
                    {
                        itemToCost[adjecent] = newCost;
                        childToParent[adjecent] = current.Item;
                        queue.Enqueue(new Node(adjecent, newCost + heuristicFunc(adjecent)));
                    }
                }
            }

            result = new AStarResult<T>(Array.Empty<T>(), 0, 0);
            return false;
        }

        private AStarResult<T> GetResult(T end, IDictionary<T, T> childToParent, int cost, int visited)
        {
            Stack<T> stack = new Stack<T>();
            T current = end;
            do
            {
                stack.Push(current);
            } while (childToParent.TryGetValue(current, out current));

            T[] path = IncludeStartInPath
                ? stack.ToArray()
                : stack.Skip(1).ToArray();

            return new AStarResult<T>(path, cost, visited);
        }

        private record Node(T Item, int Priority);

        private class PriorityComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y) => x.Priority - y.Priority;
        }
    }

    public record AStarResult<T>(T[] Path, int Cost, int Visited);
}
