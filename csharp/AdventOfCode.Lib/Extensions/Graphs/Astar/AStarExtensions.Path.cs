namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static AStarResult<TNode> Path<TGraph, TNode>(this AStar<TGraph, TNode> source, Func<TNode, bool> isFinished)
            where TNode : notnull
        {
            var queue = new PriorityQueue<TNode, int>();
            var cameFrom = new Dictionary<TNode, TNode>();
            var costSoFar = new Dictionary<TNode, int> { { source.StartNode, 0 } };

            queue.Enqueue(source.StartNode, 0);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (isFinished(currentNode))
                {
                    var path = GetPath(currentNode, cameFrom);
                    var totalCost = costSoFar[currentNode];
                    return new AStarResult<TNode>(true, totalCost, path.ToList());
                }

                foreach (var nextNode in source.GetAdjacent(currentNode))
                {
                    var newCost = costSoFar[currentNode] + source.GetWeight(currentNode, nextNode);

                    if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                    {
                        costSoFar[nextNode] = newCost;
                        queue.Enqueue(nextNode, newCost + source.GetHeuristic(nextNode));
                        cameFrom[nextNode] = currentNode;
                    }
                }
            }

            return new AStarResult<TNode>(false, 0, new List<TNode>());
        }

        private static IEnumerable<TNode> GetPath<TNode>(TNode end, IDictionary<TNode, TNode> previous)
            where TNode : notnull
        {
            var stack = new Stack<TNode>();
            var current = end;
            do
            {
                stack.Push(current);
            } while (previous.TryGetValue(current, out current));

            return stack;
        }
    }
}
