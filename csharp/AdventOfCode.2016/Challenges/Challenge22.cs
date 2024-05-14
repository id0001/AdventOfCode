using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2016.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var nodes = await inputReader.ReadLinesAsync(22).Skip(2).Select(ParseLine).ToListAsync();

        return nodes
            .SelectMany(a => nodes.Select(b => new {A = a, B = b}))
            .Where(pair => pair.A.Used > 0 && pair.A != pair.B && pair.B.Available >= pair.A.Used)
            .Count()
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var nodes = await inputReader.ReadLinesAsync(22).Skip(2).Select(ParseLine).ToDictionaryAsync(kv => kv.Location);

        var empty = nodes.Values.First(n => n.Used == 0);
        var maxX = nodes.Keys.MaxBy(n => n.X).X;
        var end = nodes[new Point2(maxX - 1, 0)];

        var bfs = new BreadthFirstSearch<Node>(n => GetAdjacent(nodes, n));
        bfs.TryPath(empty, n => n == end, out var path);

        // 1. First move to the node on the left of the top right corner using the shortest path.
        // 2. Using a circular movement >V<<^ (5 steps), move the data back to the start. This can be calculated as (5 * X) + 1 -- (+1 for the last move)
        return (path.Count() - 1 + end.Location.X * 5 + 1).ToString();
    }


    private IEnumerable<Node> GetAdjacent(Dictionary<Point2, Node> nodes, Node current)
    {
        foreach (var neighbor in current.Location.GetNeighbors())
            if (nodes.ContainsKey(neighbor) && current.Size >= nodes[neighbor].Used)
                yield return nodes[neighbor];
    }

    private Node ParseLine(string line) =>
        line.Extract<int>(@"/dev/grid/node-x(\d+)-y(\d+) +(\d+)T +(\d+)T +(\d+)T +(\d+)%")
            .Into(matches => new Node(new Point2(matches[0], matches[1]), matches[2], matches[3]));

    private class Node(Point2 location, int size, int used)
    {
        public Point2 Location => location;

        public int Size => size;

        public int Used { get; } = used;

        public int Available => Size - Used;
    }
}