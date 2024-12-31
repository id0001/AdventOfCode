namespace AdventOfCode.Lib;

public static partial class BreadthFirstSearchExtensions
{
    public static IDictionary<TNode, int> Distances<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var visited = new Dictionary<TNode, int> {{source.StartNode, 0}};

        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var distance = visited[currentNode];

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, distance + 1);
                queue.Enqueue(adjacent);
            }
        }

        return visited;
    }
}