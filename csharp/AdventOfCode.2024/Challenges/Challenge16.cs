using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2024.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(16);

        var start = grid.Find((p, c) => c == 'S');
        var end = grid.Find((p, c) => c == 'E');

        grid.AStar(GetAdjacent, new Pose2(start, Face.Right))
            .WithWeight(GetWeight)
            .TryPath(p => p.Position == end, out _, out int cost);

        return cost.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(0);
        var start = grid.Find((p, c) => c == 'S');
        var end = grid.Find((p, c) => c == 'E');

        var queue = new PriorityQueue<Pose2, int>();
        var distances = new Dictionary<Pose2, int>();
        var predecessors = new Dictionary<Pose2, HashSet<Pose2>>();
        
        queue.Enqueue(new (start, Face.Right), 0);
        distances.Add(new (start, Face.Right), 0);
        
        grid.AStar(GetAdjacent, new Pose2(start, Face.Right))
            .WithWeight(GetWeight)
            .TryPath(p => p.Position == end, out _, out int actualCost);

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var currentNode, out var currentDistance);
            
            if(distances.ContainsKey(currentNode) && currentDistance > distances[currentNode])
                continue;

            foreach (var neighbor in GetAdjacent(grid, currentNode))
            {
                var newDistance = currentDistance + GetWeight(grid, currentNode, neighbor);
                if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    queue.Enqueue(neighbor, newDistance);
                        predecessors[neighbor] = [currentNode];
                }
                else if (newDistance == distances[neighbor])
                {
                        predecessors[neighbor].Add(currentNode);
                }
            }
        }
        
        var paths = Backtrack(new Pose2(start, Face.Right), end, new[]{ new Pose2(end, Face.Up), new Pose2(end, Face.Right) }, predecessors);

        var groups = paths
            .GroupBy(p => CalculateCost(p))
            .ToList();
        
        var test = groups
            .OrderBy(g => g.Key)
            .First().SelectMany(p => p.Select(x => x.Position)).Distinct().Count();
        
        return string.Empty;
    }

    private static int CalculateCost(List<Pose2> path)
    {
        int cost = 0;
        foreach (var pair in path.Windowed(2))
        {
            if (pair[0].Position == pair[1].Position)
                cost += 1000;

            cost++;
        }

        return cost;
    }
    
    private static List<List<Pose2>> Backtrack(Pose2 start, Point2 endPos, Pose2[] nodes, IDictionary<Pose2, HashSet<Pose2>> previous)
    {
        var paths = new List<List<Pose2>>();
        var stack = new Stack<(Pose2, List<Pose2>)>();
        foreach(var end in nodes)
            stack.Push((end, [end]));

        while (stack.Count > 0)
        {
            var (currentNode, path) = stack.Pop();
            if (currentNode == start)
            {
                path.Reverse();
                paths.Add(path);
            }
            else
            {
                foreach(var predecessor in previous[currentNode].Where(p => p.Position != endPos))
                    stack.Push((predecessor, [..path, predecessor]));
            }
        }
        
        return paths;
    }

    private static int GetWeight(char[,] grid, Pose2 current, Pose2 next)
    {
        if (current.Position != next.Position)
            return 1;

        return 1000;
    }

    private static IEnumerable<Pose2> GetAdjacent(char[,] grid, Pose2 current)
    {
        if (grid.At(current.Ahead) != '#')
            yield return current.Step();

        yield return current.TurnLeft();
        yield return current.TurnRight();
    } 
    
    private record State(Pose2 Pose, int Cost) : IEquatable<State>
    {
        public virtual bool Equals(State? other) => other is not null && other.Pose == Pose;

        public override int GetHashCode() => Pose.GetHashCode();
    }
}