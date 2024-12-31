namespace AdventOfCode.Lib;

public static partial class AStarExtensions
{
    public static IEnumerable<AStarResult<TNode>> FindAll<TGraph, TNode>(this AStar<TGraph, TNode> source,
        Func<TNode, bool> isFinished)
        where TNode : notnull
    {
        var queue = new PriorityQueue<TNode, int>();
        var cameFrom = new Dictionary<TNode, HashSet<TNode>>();
        var costSoFar = new Dictionary<TNode, int> {{source.StartNode, 0}};

        queue.Enqueue(source.StartNode, 0);

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var currentNode, out var currentCost);
            if (currentNode is null)
                continue; // Impossible

            if (isFinished(currentNode))
            {
                foreach (var result in GetPaths(source.StartNode, currentNode, cameFrom)
                             .Select(p => new AStarResult<TNode>(true, currentCost, p)))
                    yield return result;

                continue;
            }

            foreach (var nextNode in source.GetAdjacent(currentNode))
            {
                var newCost = costSoFar[currentNode] + source.GetWeight(currentNode, nextNode);

                if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                {
                    costSoFar[nextNode] = newCost;
                    queue.Enqueue(nextNode, newCost + source.GetHeuristic(nextNode));
                    cameFrom[nextNode] = [currentNode];
                }
                else if (newCost == costSoFar[nextNode])
                {
                    cameFrom[nextNode].Add(currentNode);
                }
            }
        }
    }
}