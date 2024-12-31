namespace AdventOfCode.Lib;

public static partial class BreadthFirstSearchExtensions
{
    public static BreadthFirstSearchResult<TNode> FindShortest<TGraph, TNode>(
        this BreadthFirstSearch<TGraph, TNode> source, Func<TNode, bool> isFinished)
        where TNode : notnull
    {
        var queue = new Queue<TNode>();
        var previous = new Dictionary<TNode, TNode>();
        var visited = new HashSet<TNode> {source.StartNode};

        queue.Enqueue(source.StartNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                var path = GetPath(currentNode, previous);
                return new BreadthFirstSearchResult<TNode>(true, path.ToList());
            }

            foreach (var adjacent in source.GetAdjacent(currentNode))
            {
                if (visited.Contains(adjacent))
                    continue;

                previous.Add(adjacent, currentNode);
                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }

        return new BreadthFirstSearchResult<TNode>(false, new List<TNode>());
    }
}