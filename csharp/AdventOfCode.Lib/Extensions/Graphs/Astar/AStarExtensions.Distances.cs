namespace AdventOfCode.Lib
{
    public static partial class AStarExtensions
    {
        public static IDictionary<TNode, int> Distances<TGraph, TNode>(this AStar<TGraph, TNode> source)
            where TNode : notnull
        {
            var queue = new PriorityQueue<TNode, int>();
            var costSoFar = new Dictionary<TNode, int> { { source.StartNode, 0 } };

            queue.Enqueue(source.StartNode, 0);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                foreach (var nextNode in source.GetAdjacent(currentNode))
                {
                    var newCost = costSoFar[currentNode] + source.GetWeight(currentNode, nextNode);

                    if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                    {
                        costSoFar[nextNode] = newCost;
                        queue.Enqueue(nextNode, newCost + source.GetHeuristic(nextNode));
                    }
                }
            }

            return costSoFar;
        }
    }
}
