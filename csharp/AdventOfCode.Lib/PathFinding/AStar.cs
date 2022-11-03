namespace AdventOfCode.Lib.PathFinding;

public class AStar<T> where T : notnull
{
    private readonly Func<T, IEnumerable<T>> _getAdjacent;
    private readonly Func<T, T, int> _weight;

    public AStar(Func<T, IEnumerable<T>> getAdjacent, Func<T, T, int> weight)
    {
        _getAdjacent = getAdjacent;
        _weight = weight;
    }

    public Func<T, int> Heuristic { get; } = _ => 0;

    public bool TryPath(T start, Func<T, bool> isFinished, out IEnumerable<T> path,
        out int totalDistance)
        => TryPath(start, _getAdjacent, _weight, Heuristic, isFinished, out path, out totalDistance);

    private static bool TryPath(T start, Func<T, IEnumerable<T>> getAdjacent, Func<T, T, int> weight,
        Func<T, int> heuristic, Func<T, bool> isFinished, out IEnumerable<T> path, out int totalDistance)
    {
        var queue = new PriorityQueue<T, int>();
        var previous = new Dictionary<T, T>();
        var distances = new Dictionary<T, int> { { start, 0 } };

        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                path = GetPath(currentNode, previous);
                totalDistance = distances[currentNode];
                return true;
            }

            foreach (var adjacent in getAdjacent(currentNode))
            {
                var newDistance = distances[currentNode] + weight(currentNode, adjacent);
                if (!distances.TryGetValue(adjacent, out var oldDistance))
                    oldDistance = int.MaxValue;

                if (newDistance >= oldDistance) continue;

                if (!distances.TryAdd(adjacent, newDistance))
                    distances[adjacent] = newDistance;

                if (!previous.TryAdd(adjacent, currentNode))
                    previous[adjacent] = currentNode;
                
                queue.Enqueue(adjacent, newDistance + heuristic(adjacent));
            }
        }

        path = Enumerable.Empty<T>();
        totalDistance = 0;
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