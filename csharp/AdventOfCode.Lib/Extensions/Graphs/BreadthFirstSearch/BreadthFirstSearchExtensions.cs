namespace AdventOfCode.Lib
{
    public static partial class BreadthFirstSearchExtensions
    {
        public static AStar<TGraph, TNode> WithWeight<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source, Func<TNode, TNode, int> getWeight)
            where TNode : notnull
           => new AStar<TGraph, TNode>(source.Source, source.StartNode, source.GetAdjacent, getWeight, n => 0);

        public static AStar<TGraph, TNode> WithWeight<TGraph, TNode>(this BreadthFirstSearch<TGraph, TNode> source, Func<TGraph, TNode, TNode, int> getWeight)
            where TNode : notnull
           => new AStar<TGraph, TNode>(source.Source, source.StartNode, source.GetAdjacent, (a, b) => getWeight(source.Source, a, b), n => 0);

        private static IEnumerable<TNode> GetPath<TNode>(TNode end, IDictionary<TNode, TNode> previous)
        {
            var stack = new Stack<TNode>();
            var current = end;
            do
            {
                stack.Push(current);
            } while (previous.TryGetValue(current, out current));

            return stack;
        }

        private static List<List<TNode>> GetPaths<TNode>(TNode start, TNode node, IDictionary<TNode, HashSet<TNode>> previous)
            where TNode : notnull
        {
            var paths = new List<List<TNode>>();
            var stack = new Stack<(TNode currentNode, List<TNode> path)>();

            // Initialize the stack with the starting node and an empty path
            stack.Push((node, [node]));

            while (stack.Count > 0)
            {
                var (currentNode, path) = stack.Pop();

                if (currentNode.Equals(start))
                {
                    // Add the complete path to the results
                    paths.Add(new List<TNode>(path));
                    continue;
                }

                if (previous.TryGetValue(currentNode, out var previousNodes))
                {
                    foreach (var prevNode in previousNodes)
                    {
                        // Create a new path including the previous node
                        var newPath = new List<TNode>(path) { prevNode };
                        stack.Push((prevNode, newPath));
                    }
                }
            }

            // Reverse the paths to get the correct order from start to node
            foreach (var path in paths)
                path.Reverse();

            return paths;
        }
    }

    public record BreadthFirstSearch<TGraph, TNode>(TGraph Source, TNode StartNode, Func<TNode, IEnumerable<TNode>> GetAdjacent)
        where TNode : notnull;

    public record BreadthFirstSearchResult<TNode>(bool Success, IList<TNode> Path)
    {
        public int Distance => Path.Count - 1;
    }
}
