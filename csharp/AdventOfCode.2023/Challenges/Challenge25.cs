using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2023.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = ParseInput(await InputReader.ReadAllTextAsync(25));

        List<string>[] partitions;
        List<(string, string)> cutEdges;
        do
        {
            (partitions, cutEdges) = graph.MinCut();
        } while (cutEdges.Count != 3);

        return partitions.Select(x => x.Count).Product().ToString();
    }

    private static UndirectedGraph<string, int> ParseInput(string text)
    {
        var nl = Environment.NewLine;

        var dict = text.SplitBy(nl)
            .Select(line => line.SplitBy(":")
                .Into(parts => (parts.First(), parts.Second().SplitBy(" "))))
            .ToDictionary(kv => kv.Item1, kv => kv.Item2);

        var graph = new UndirectedGraph<string, int>();

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