namespace AdventOfCode.Lib.PathFinding;

public class BreadthFirstSearch<T>(Func<T, IEnumerable<T>> getAdjacent)
    where T : notnull
{
    public bool TryPath(T start, Func<T, bool> isFinished, out IEnumerable<T> path) =>
        TryPath(start, getAdjacent, isFinished, out path);

    public IEnumerable<(T Value, int Distance)> FloodFill(T start)
    {
        var queue = new Queue<T>();
        var visited = new Dictionary<T, int> {{start, 0}};

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var distance = visited[currentNode];

            yield return (currentNode, distance);

            foreach (var adjacent in getAdjacent(currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, distance + 1);
                queue.Enqueue(adjacent);
            }
        }
    }

    public IEnumerable<(T Value, int Distance)> FloodFill(T start, int MaxSteps)
    {
        var queue = new Queue<T>();
        var visited = new Dictionary<T, int> { { start, 0 } };

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var distance = visited[currentNode];

            yield return (currentNode, distance);

            if (distance == MaxSteps)
                continue;

            foreach (var adjacent in getAdjacent(currentNode))
            {
                if (visited.ContainsKey(adjacent))
                    continue;

                visited.Add(adjacent, distance + 1);
                queue.Enqueue(adjacent);
            }
        }
    }

    private static bool TryPath(T start, Func<T, IEnumerable<T>> getAdjacent, Func<T, bool> isFinished,
        out IEnumerable<T> path)
    {
        var queue = new Queue<T>();
        var previous = new Dictionary<T, T>();
        var visited = new HashSet<T> {start};

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (isFinished(currentNode))
            {
                path = GetPath(currentNode, previous);
                return true;
            }

            foreach (var adjacent in getAdjacent(currentNode))
            {
                if (visited.Contains(adjacent))
                    continue;

                previous.Add(adjacent, currentNode);
                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }

        path = Enumerable.Empty<T>();
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