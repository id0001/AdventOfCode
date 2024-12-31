namespace AdventOfCode.Lib;

public static partial class BreadthFirstSearchExtensions
{
    public static IEnumerable<TNode> FloodFill<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var visited = new HashSet<TNode> {source.StartNode};

        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            yield return currentNode;

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (visited.Contains(adjacent))
                    continue;

                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }
    }

    public static IEnumerable<(TNode Node, int Distance)> FloodFillWithDistance<TGraph, TNode>(
        this BreadthFirstSearch<TGraph, TNode> source)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var visited = new Dictionary<TNode, int> {{source.StartNode, 0}};

        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var distance = visited[currentNode];

            yield return (currentNode, distance);

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, distance + 1);
                queue.Enqueue(adjacent);
            }
        }
    }

    public static IEnumerable<(TNode Node, int Distance)> FloodFillWithDistance<TGraph, TNode>(
        this BreadthFirstSearch<TGraph, TNode> source, int maxDistance)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var visited = new Dictionary<TNode, int> {{source.StartNode, 0}};

        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var distance = visited[currentNode];

            yield return (currentNode, distance);

            if (distance == maxDistance)
                continue;

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, distance + 1);
                queue.Enqueue(adjacent);
            }
        }
    }
}