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

        private static IList<IList<TNode>> GetPaths<TNode>(TNode start, TNode node, IDictionary<TNode, HashSet<TNode>> previous)
            where TNode : notnull
        {
            if (node.Equals(start))
                return [[start]];

            var paths = new List<IList<TNode>>();
            foreach (var previousNode in previous[node])
                foreach (var path in GetPaths(start, previousNode, previous))
                    paths.Add([.. path, node]);

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
