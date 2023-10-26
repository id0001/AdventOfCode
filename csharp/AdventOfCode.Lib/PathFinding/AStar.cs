namespace AdventOfCode.Lib.PathFinding;

public class AStar<T> where T : notnull
{
    private readonly Func<T, IEnumerable<T>> _adjacent;
    private readonly Func<T, T, int> _weight;
    private readonly Func<T, int> _heuristic;

    public AStar(Func<T, IEnumerable<T>> adjecent, Func<T, T, int> weight) : this(adjecent, weight, _ => 0)
    {
    }

    public AStar(Func<T, IEnumerable<T>> adjecent, Func<T, T, int> weight, Func<T, int> heuristic)
    {
        _adjacent = adjecent;
        _weight = weight;
        _heuristic = heuristic;
    }

    public bool TryPath(
        T start,
        Func<T, bool> isFinished,
        out IEnumerable<T> path,
        out int totalCost
        )
        => TryPath(start, _adjacent, _weight, _heuristic, isFinished, out path, out totalCost);

    private static bool TryPath(
        T start,
        Func<T, IEnumerable<T>> getAdjacent,
        Func<T, T, int> weight,
        Func<T, int> heuristic,
        Func<T, bool> isFinished,
        out IEnumerable<T> path,
        out int totalCost
        )
    {
        var queue = new PriorityQueue<T, int>();
        var cameFrom = new Dictionary<T, T>();
        var costSoFar = new Dictionary<T, int> { { start, 0 } };

        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                path = GetPath(currentNode, cameFrom);
                totalCost = costSoFar[currentNode];
                return true;
            }

            foreach (var adjacent in getAdjacent(currentNode))
            {
                var newCost = costSoFar[currentNode] + weight(currentNode, adjacent);

                if (!costSoFar.ContainsKey(adjacent) || newCost < costSoFar[adjacent])
                {
                    costSoFar[adjacent] = newCost;
                    queue.Enqueue(adjacent, newCost + heuristic(adjacent));
                    cameFrom[adjacent] = currentNode;
                }
            }
        }

        path = Enumerable.Empty<T>();
        totalCost = 0;
        return false;
    }

    private static IEnumerable<T> GetPath(T end, IDictionary<T, T> previous)
    {
        var stack = new Stack<T>();
        var current = end;
        do
        {
            stack.Push(current);
        } while (previous.TryGetValue(current, out current));

        return stack;
    }
}