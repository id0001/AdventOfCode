using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2017.Challenges;

[Challenge(12)]
public class Challenge12(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = ParseInput(await inputReader.ReadAllTextAsync(12));
        return graph.FloodFill(0).Count().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var graph = ParseInput(await inputReader.ReadAllTextAsync(12));
        var remaining = graph.Vertices.ToHashSet();

        var groups = 0;
        while (remaining.Count > 0)
        {
            remaining.ExceptWith(graph.FloodFill(remaining.First()).Select(x => x.Vertex));
            groups++;
        }

        return groups.ToString();
    }

    private static UndirectedGraph<int, int> ParseInput(string text)
    {
        var nl = Environment.NewLine;

        var dict = text.SplitBy(nl)
            .Select(line => line.SplitBy("<->")
                .Into(parts => (parts.First().As<int>(), parts.Second().SplitBy(",").As<int>())))
            .ToDictionary(kv => kv.Item1, kv => kv.Item2);

        var graph = new UndirectedGraph<int, int>();

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