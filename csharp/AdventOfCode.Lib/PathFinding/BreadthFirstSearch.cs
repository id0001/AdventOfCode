﻿namespace AdventOfCode.Lib.PathFinding;

public class BreadthFirstSearch<T> where T : notnull
{
    private readonly Func<T, IEnumerable<T>> _getAdjacent;

    public BreadthFirstSearch(Func<T, IEnumerable<T>> getAdjacent)
    {
        _getAdjacent = getAdjacent;
    }

    public bool TryPath(T start, Func<T, bool> isFinished, out IEnumerable<T> path) =>
        TryPath(start, _getAdjacent, isFinished, out path);

    private static bool TryPath(T start, Func<T, IEnumerable<T>> getAdjacent, Func<T, bool> isFinished, out IEnumerable<T> path)
    {
        var queue = new Queue<T>();
        var previous = new Dictionary<T, T>();
        var visited = new HashSet<T> { start };

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