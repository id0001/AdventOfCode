namespace AdventOfCode.Lib.PathFinding;

public class AStar<T>(Func<T, IEnumerable<T>> adjacent, Func<T, T, int> weight, Func<T, int> heuristic)
    where T : notnull
{
    public AStar(Func<T, IEnumerable<T>> adjacent, Func<T, T, int> weight) : this(adjacent, weight, _ => 0)
    {
    }

    public bool TryPath(
        T start,
        Func<T, bool> isFinished,
        out IEnumerable<T> path,
        out int totalCost
    )
    {
        var queue = new PriorityQueue<T, int>();
        var cameFrom = new Dictionary<T, T>();
        var costSoFar = new Dictionary<T, int> {{start, 0}};

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

            foreach (var nextNode in adjacent(currentNode))
            {
                var newCost = costSoFar[currentNode] + weight(currentNode, nextNode);

                if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                {
                    costSoFar[nextNode] = newCost;
                    queue.Enqueue(nextNode, newCost + heuristic(nextNode));
                    cameFrom[nextNode] = currentNode;
                }
            }
        }

        path = Enumerable.Empty<T>();
        totalCost = 0;
        return false;
    }

    public bool All(
        T start,
        Func<T, bool> shouldContinue
    )
    {
        var queue = new PriorityQueue<T, int>();
        var costSoFar = new Dictionary<T, int> {{start, 0}};

        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (!shouldContinue(currentNode))
                continue;

            foreach (var nextNode in adjacent(currentNode))
            {
                var newCost = costSoFar[currentNode] + weight(currentNode, nextNode);

                if (!costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                {
                    costSoFar[nextNode] = newCost;
                    queue.Enqueue(nextNode, newCost + heuristic(nextNode));
                }
            }
        }

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