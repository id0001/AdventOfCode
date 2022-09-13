namespace AdventOfCode.Lib.PathFinding;

public class Dijkstra<T> where T : notnull
{
    private readonly Func<T, IEnumerable<T>> _getAdjacent;
    private readonly Func<T, T, int> _weight;

    public Dijkstra(Func<T, IEnumerable<T>> getAdjacent, Func<T, T, int> weight)
    {
        _getAdjacent = getAdjacent;
        _weight = weight;
    }

    public bool TryPath(T start, Func<T, bool> isFinished, out IEnumerable<T> path,
        out int totalDistance)
        => TryPath(start, _getAdjacent, _weight, isFinished, out path, out totalDistance);

    private static bool TryPath(T start, Func<T, IEnumerable<T>> getAdjacent, Func<T, T, int> weight,
        Func<T, bool> isFinished, out IEnumerable<T> path, out int totalDistance)
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

                distances.AddOrUpdate(adjacent, newDistance);
                previous.AddOrUpdate(adjacent, currentNode);
                queue.Enqueue(adjacent, newDistance);
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
            stack.Push(current!);
        } while (previous.TryGetValue(current, out current));

        return stack;
    }
}