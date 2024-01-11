using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2023.Challenges;

[Challenge(17)]
public class Challenge17(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = await InputReader.ReadGridAsync<int>(17);
        var start = new Pose2(Point2.Zero, Face.Right);
        var end = new Point2(graph.GetUpperBound(1), graph.GetUpperBound(0));

        var bounds = new Rectangle(0, 0, graph.GetLength(1), graph.GetLength(0));
        var astar = new AStar<Node>(n => GetAdjacent(bounds, n), (_, b) => GetWeight(graph, b));
        var isFinished = new Func<Node, bool>(n => n.Pose.Position == end);

        astar.TryPath(new Node(start, 0), isFinished, out _, out var cost);
        return cost.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var graph = await InputReader.ReadGridAsync<int>(17);
        var start = new Pose2(Point2.Zero, Face.Right);
        var end = new Point2(graph.GetUpperBound(1), graph.GetUpperBound(0));

        var bounds = new Rectangle(0, 0, graph.GetLength(1), graph.GetLength(0));
        var astar = new AStar<Node>(n => GetAdjacent2(bounds, n), (_, b) => GetWeight(graph, b));
        var isFinished = new Func<Node, bool>(n => n.Pose.Position == end && n.Consecutive > 3);

        astar.TryPath(new Node(start, 0), isFinished, out _, out var cost);
        return cost.ToString();
    }

    private static int GetWeight(int[,] graph, Node current) =>
        graph[current.Pose.Position.Y, current.Pose.Position.X];

    private static IEnumerable<Node> GetAdjacent(Rectangle bounds, Node node)
    {
        var ahead = node.Pose.Step();
        var left = node.Pose.TurnLeft().Step();
        var right = node.Pose.TurnRight().Step();

        if (node.Consecutive < 3 && bounds.Contains(ahead.Position))
            yield return new Node(ahead, node.Consecutive + 1);

        if (bounds.Contains(left.Position))
            yield return new Node(left, 1);

        if (bounds.Contains(right.Position))
            yield return new Node(right, 1);
    }

    private static IEnumerable<Node> GetAdjacent2(Rectangle bounds, Node node)
    {
        var ahead = node.Pose.Step();
        var left = node.Pose.TurnLeft().Step();
        var right = node.Pose.TurnRight().Step();

        if (node.Consecutive < 10 && bounds.Contains(ahead.Position))
            yield return new Node(ahead, node.Consecutive + 1);

        if (bounds.Contains(left.Position) && node.Consecutive > 3)
            yield return new Node(left, 1);

        if (bounds.Contains(right.Position) && node.Consecutive > 3)
            yield return new Node(right, 1);
    }

    private record Node(Pose2 Pose, int Consecutive);
}