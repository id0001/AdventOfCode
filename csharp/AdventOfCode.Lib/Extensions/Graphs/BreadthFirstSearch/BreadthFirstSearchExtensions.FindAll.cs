namespace AdventOfCode.Lib;

public static partial class BreadthFirstSearchExtensions
{
    public static IEnumerable<BreadthFirstSearchResult<TNode>> FindAll<TGraph, TNode>(
        this BreadthFirstSearch<TGraph, TNode> source, Func<TNode, bool> isFinished)
        where TNode : notnull
    {
        var cameFrom = new Dictionary<TNode, HashSet<TNode>>();
        var queue = new Queue<TNode>();
        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                foreach (var result in GetPaths(source.StartNode, currentNode, cameFrom)
                             .Select(p => new BreadthFirstSearchResult<TNode>(true, p)))
                    yield return result;

                continue;
            }

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (cameFrom.TryAdd(adjacent, [currentNode]))
                {
                    queue.Enqueue(adjacent);
                    continue;
                }

                cameFrom[adjacent].Add(currentNode);
            }
        }
    }
}