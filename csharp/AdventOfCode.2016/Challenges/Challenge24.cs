using System.Collections;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2016.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(24);

        var allPointsOfInterest = grid.Where((_, c) => char.IsNumber(c)).ToArray();

        var weights = GetWeights(grid, allPointsOfInterest);

        var visited = new BitArray(allPointsOfInterest.Length);
        visited.Set(0, true);

        var aStar = new AStar<Node>(c => GetAdjacent(weights, c), (a, b) => Weight(weights, a, b));
        aStar.TryPath(new Node(0, visited), n => n.Visited.HasAllSet(), out _, out var cost);

        return cost.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(24);

        var allPointsOfInterest = grid.Where((_, c) => char.IsNumber(c)).ToArray();

        var weights = GetWeights(grid, allPointsOfInterest);

        var visited = new BitArray(allPointsOfInterest.Length);

        var aStar = new AStar<Node>(c => GetAdjacent(weights, c), (a, b) => Weight(weights, a, b));
        aStar.TryPath(new Node(0, visited), n => n.Visited.HasAllSet(), out _, out var cost);

        return cost.ToString();
    }

    private static Dictionary<(int, int), int> GetWeights(char[,] grid, IEnumerable<Point2> nodes)
    {
        var bfs = new BreadthFirstSearch<Point2>(n => GetAdjacentSimple(grid, n));
        var dict = new Dictionary<(int, int), int>();

        foreach (var pair in nodes.Combinations(2))
        {
            bfs.TryPath(pair[0], n => n == pair[1], out var path);

            var a = (int) char.GetNumericValue(grid[pair[0].Y, pair[0].X]);
            var b = (int) char.GetNumericValue(grid[pair[1].Y, pair[1].X]);

            var pathLength = path.Count() - 1;
            dict.Add((a, b), pathLength);
            dict.Add((b, a), pathLength);
        }

        return dict;
    }

    private static int Weight(Dictionary<(int, int), int> weights, Node current, Node next) =>
        weights[(current.Id, next.Id)];

    private static IEnumerable<Point2> GetAdjacentSimple(char[,] grid, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
            if (grid[neighbor.Y, neighbor.X] != '#')
                yield return neighbor;
    }

    private static IEnumerable<Node> GetAdjacent(Dictionary<(int, int), int> nodes, Node current)
    {
        foreach (var n in nodes.Where(kv => kv.Key.Item1 == current.Id))
            if (!current.Visited[n.Key.Item2])
            {
                var visitedWithZero = new BitArray(current.Visited);
                visitedWithZero.Set(0, true);

                // Can only end at 0 when all other nodes have been visited
                if (n.Key.Item2 == 0 && !visitedWithZero.HasAllSet())
                    continue;

                var visited = new BitArray(current.Visited);
                visited.Set(n.Key.Item2, true);
                yield return new Node(n.Key.Item2, visited);
            }
    }

    private sealed record Node(int Id, BitArray Visited);
}