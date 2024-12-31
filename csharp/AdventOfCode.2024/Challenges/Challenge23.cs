using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2024.Challenges;

[Challenge(23)]
public class Challenge23(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = new UndirectedGraph<string, int>();
        var edges = await inputReader.ParseLinesAsync(23, ParseLine).ToListAsync();
        foreach (var edge in edges)
        {
            graph.AddVertex(edge[0]);
            graph.AddVertex(edge[1]);
            graph.AddEdge(edge[0], edge[1], 1);
        }

        return EnumerateCyclesOf3(edges, graph)
            .Count(verts => verts.Any(v => v.StartsWith("t")))
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var graph = new UndirectedGraph<string, int>();
        await foreach (var edge in inputReader.ParseLinesAsync(23, ParseLine))
        {
            graph.AddVertex(edge[0]);
            graph.AddVertex(edge[1]);
            graph.AddEdge(edge[0], edge[1], 1);
        }

        return string.Join(",", graph.EnumerateCliques().OrderByDescending(c => c.Count).First().Order());
    }

    private static IEnumerable<string[]> EnumerateCyclesOf3(IEnumerable<string[]> edges,
        UndirectedGraph<string, int> graph)
    {
        var visited = new HashSet<(string, string, string)>();
        foreach (var edge in edges)
        foreach (var c0 in graph.AdjacentEdges(edge[1]).Keys)
        foreach (var c1 in graph.AdjacentEdges(c0).Keys)
            if (c1 == edge[0])
            {
                var v = new[] {edge[0], edge[1], c0}.Order().ToArray();
                var key = (v[0], v[1], v[2]);
                if (visited.Contains(key))
                    continue;

                visited.Add(key);
                yield return v;
            }
    }

    private static string[] ParseLine(string line) => line.SplitBy("-").ToArray();
}