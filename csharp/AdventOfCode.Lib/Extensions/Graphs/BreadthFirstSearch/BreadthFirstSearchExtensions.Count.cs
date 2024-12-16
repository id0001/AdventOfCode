namespace AdventOfCode.Lib
{
    public static partial class BreadthFirstSearchExtensions
    {
        public static int Count<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source, Func<TNode, bool> selector)
            where TNode : notnull
        {
            var queue = new Queue<TNode>();
            var visited = new HashSet<TNode> { source.StartNode };

            queue.Enqueue(source.StartNode);

            int count = 0;
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (selector(currentNode))
                {
                    count++;
                    continue;
                }

                foreach (var adjacent in source.GetAdjacent(currentNode))
                {
                    if (visited.Contains(adjacent))
                        continue;

                    visited.Add(adjacent);
                    queue.Enqueue(adjacent);
                }
            }

            return count;
        }

        public static int Count<TGraph, TNode>(this BreadthFirstSearchIgnoreVisited<TGraph, TNode> source, Func<TNode, bool> selector)
            where TNode : notnull
        {
            var queue = new Queue<TNode>();
            queue.Enqueue(source.StartNode);

            int count = 0;
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (selector(currentNode))
                {
                    count++;
                    continue;
                }

                foreach (var adjacent in source.GetAdjacent(currentNode))
                    queue.Enqueue(adjacent);
            }

            return count;
        }
    }
}
