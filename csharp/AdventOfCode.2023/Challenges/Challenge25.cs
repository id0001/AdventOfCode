using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2023.Challenges;

[Challenge(25)]
public class Challenge25
{
    private readonly IInputReader _inputReader;

    public Challenge25(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        // Created graphviz dot file with layout=neato and manually extracted the cutting edges.
        // lxb - vcq
        // rnx - ddj
        // mmr - znk

        var graph = ParseInput(await _inputReader.ReadAllTextAsync(25));
        graph.RemoveEdge("lxb", "vcq");
        graph.RemoveEdge("rnx", "ddj");
        graph.RemoveEdge("mmr", "znk");

        var bfs = new BreadthFirstSearch<string>(n => graph.OutEdges(n).Keys);

        var left = bfs.FloodFill("lxb").Count();
        var right = bfs.FloodFill("rnx").Count();

        return (left * right).ToString();
    }

    private static IEnumerable<string> Visit(BidirectionalGraph<string, int> graph, string start)
    {
        var queue = new Queue<string>();
        var visited = new HashSet<string> { start };

        queue.Enqueue(start);
        
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            foreach (var adjacent in graph.OutEdges(currentNode).Keys)
            {
                if (visited.Contains(adjacent)) 
                    continue;
                
                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }

        return visited;
    }

    private static BidirectionalGraph<string,int> ParseInput(string text)
    {
        var nl = Environment.NewLine;

        var dict = text.SplitBy(nl)
            .Select(line => line.SplitBy(":")
                .Transform(parts => (parts.First(), parts.Second().SplitBy(" "))))
            .ToDictionary(kv => kv.Item1, kv => kv.Item2);

        var graph = new BidirectionalGraph<string,int>();

        foreach (var kv in dict)
        {
            if (!graph.ContainsVertex(kv.Key))
                graph.AddVertex(kv.Key);

            foreach (var to in kv.Value)
            {
                if (!graph.ContainsVertex(to))
                    graph.AddVertex(to);
                
                graph.AddEdge(kv.Key, to, 1);
            }
        }

        return graph;
    }
}
