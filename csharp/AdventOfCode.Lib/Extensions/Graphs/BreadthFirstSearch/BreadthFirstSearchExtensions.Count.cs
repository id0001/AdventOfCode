namespace AdventOfCode.Lib;

public static partial class BreadthFirstSearchExtensions
{
    public static int Count<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source, Func<TNode, bool> isFinished)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var pathsCount = new Dictionary<TNode, int> {{source.StartNode, 1}};
        queue.Enqueue(source.StartNode);

        var count = 0;
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                count += pathsCount[currentNode];
                continue;
            }

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (pathsCount.TryAdd(adjacent, pathsCount[currentNode]))
                {
                    queue.Enqueue(adjacent);
                    continue;
                }

                pathsCount[adjacent] += pathsCount[currentNode];
            }
        }

        return count;
    }
}