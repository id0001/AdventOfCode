using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;
using Rectangle = AdventOfCode.Lib.Rectangle;

namespace AdventOfCode2016.Challenges;

[Challenge(17)]
public class Challenge17
{
    private const string Input = "qzthpkfp";

    private static readonly MD5 Hasher = MD5.Create();
    private static readonly char[] PassDirections = ['U', 'D', 'L', 'R'];
    private static readonly Point2[] Directions = [Face.Up, Face.Down, Face.Left, Face.Right];
    private static readonly Rectangle Bounds = new(0, 0, 4, 4);

    [Part1]
    public string Part1()
    {
        var start = new Node(Point2.Zero, Input);

        var bfs = new BreadthFirstSearch<Node>(GetAdjacent);
        bfs.TryPath(start, node => node.Position == new Point2(3, 3), out var path);

        return path.Last().Passcode[Input.Length..];
    }

    [Part2]
    public string Part2()
    {
        var start = new Node(Point2.Zero, Input);

        var queue = new Queue<Node>();

        queue.Enqueue(start);

        var longestPath = 0;
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (currentNode.Position == new Point2(3, 3))
            {
                longestPath = Math.Max(longestPath, currentNode.Passcode[Input.Length..].Length);
                continue;
            }

            foreach (var adjacent in GetAdjacent(currentNode))
                queue.Enqueue(adjacent);
        }

        return longestPath.ToString();
    }

    private static IEnumerable<Node> GetAdjacent(Node current)
    {
        var hash = GetHash(current.Passcode);

        // U D L R
        for (var i = 0; i < 4; i++)
        {
            if (hash[i] == 'a' || char.IsNumber(hash[i]) || !Bounds.Contains(current.Position + Directions[i]))
                continue;

            yield return new Node(current.Position + Directions[i], current.Passcode + PassDirections[i]);
        }
    }

    private static string GetHash(string passcode) =>
        Convert.ToHexString(Hasher.ComputeHash(Encoding.Default.GetBytes(passcode))).ToLowerInvariant();

    private record Node(Point2 Position, string Passcode);
}