namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static int Count<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TNode, bool> isFinished)
            where TNode : notnull
        {
            var queue = new PriorityQueue<TNode, int>();
            var pathsCount = new Dictionary<TNode, int>() { { source.StartNode, 1 } };
            var costSoFar = new Dictionary<TNode, int> { { source.StartNode, 0 } };

            queue.Enqueue(source.StartNode, 0);

            var count = 0;
            while (queue.Count > 0)
            {
                queue.TryDequeue(out var currentNode, out var currentCost);
                if (currentNode is null)
                    continue; // Impossible

                if (isFinished(currentNode))
                {
                    count += pathsCount[currentNode];
                    continue;
                }

                foreach (var nextNode in source.GetAdjacent(currentNode))
                {
                    var newCost = costSoFar[currentNode] + source.GetWeight(currentNode, nextNode);

                    if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                    {
                        costSoFar[nextNode] = newCost;
                        queue.Enqueue(nextNode, newCost + source.GetHeuristic(nextNode));
                        pathsCount[nextNode] = pathsCount[currentNode];
                    }
                    else if (newCost == costSoFar[nextNode])
                        pathsCount[nextNode] += pathsCount[currentNode];
                }
            }

            return count;
        }
    }
}
