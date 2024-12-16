namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static IEnumerable<(int Cost, IList<IList<TNode>> Paths)> All<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TNode, bool> isFinished)
            where TNode : notnull
        {
            var queue = new PriorityQueue<TNode, int>();
            var cameFrom = new Dictionary<TNode, HashSet<TNode>>();
            var costSoFar = new Dictionary<TNode, int> { { source.StartNode, 0 } };

            queue.Enqueue(source.StartNode, 0);

            var finishedStates = new List<(int Cost, TNode Node)>();
            while (queue.Count > 0)
            {
                queue.TryDequeue(out var currentNode, out var currentCost);
                if (currentNode is null)
                    continue; // Impossible

                if (isFinished(currentNode))
                {
                    finishedStates.Add((currentCost, currentNode));
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
                        cameFrom[nextNode].Add(currentNode);
                }
            }

            return finishedStates
                .Select(end => (end.Cost, Paths: Backtrack(source.StartNode, end.Node, cameFrom)))
                .ToList();
        }

        private static IList<IList<TNode>> Backtrack<TNode>(TNode start, TNode node, IDictionary<TNode, HashSet<TNode>> previous)
            where TNode : notnull
        {
            if (node.Equals(start))
                return [[start]];

            var paths = new List<IList<TNode>>();
            foreach (var previousNode in previous[node])
                foreach (var path in Backtrack(start, previousNode, previous))
                    paths.Add([.. path, node]);

            return paths;
        }
    }
}
