using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2018.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var actions = await inputReader.ParseLinesAsync(7, ParseLine).ToListAsync();
        var options = actions.SelectMany(a => new[] { a.First, a.Second }).Distinct().ToHashSet();

        var graph = new DirectedGraph<char, int>();

        foreach (var node in options)
            graph.AddVertex(node);

        foreach (var action in actions)
            graph.AddEdge(action.First, action.Second, 1);

        return graph.LexicographicalTopologicalSort().AsString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var actions = await inputReader.ParseLinesAsync(7, ParseLine).ToListAsync();
        var nodes = actions.SelectMany(a => new[] { a.First, a.Second }).Distinct().ToHashSet();

        var graph = new DirectedGraph<char, int>();

        foreach (var node in nodes)
            graph.AddVertex(node);

        foreach (var action in actions)
            graph.AddEdge(action.First, action.Second, 1);

        var nodesWork = graph.Vertices.ToDictionary(n => n, n => 61 + (n - 'A'));
        var inDegree = graph.Vertices.ToDictionary(v => v, v => graph.InEdges(v).Keys.ToHashSet());

        var time = 0;
        while (nodesWork.Keys.Count > 0)
        {
            var available = nodesWork.Where(n => inDegree[n.Key].Count == 0).OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();

            foreach(var (worker, node) in Enumerable.Range(0, 5).Zip(available))
            {
                nodesWork[node] -= 1;
                if (nodesWork[node] == 0)
                {
                    nodesWork.Remove(node);
                    foreach (var n in inDegree.Keys)
                        inDegree[n].Remove(node);
                }
            }

            time++;
        }

        return time.ToString();
    }

    private static (char First, char Second) ParseLine(string line) => line.Extract<char, char>(@"Step (\w) must be finished before step (\w) can begin\.");
}