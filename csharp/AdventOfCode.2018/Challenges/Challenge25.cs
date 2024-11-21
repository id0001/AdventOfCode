using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions.Graphs;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2018.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var graph = new UndirectedGraph<Point4, int>();
        await foreach (var point in inputReader.ParseLinesAsync(25, ParseLine))
            graph.AddVertex(point);

        foreach (var combi in graph.Vertices.Combinations(2))
        {
            var distance = Point4.ManhattanDistance(combi[0], combi[1]);
            if (distance <= 3)
                graph.AddEdge(combi[0], combi[1], distance);
        }

        return graph.ConnectedComponents().Count().ToString();
    }

    private static Point4 ParseLine(string line) => line.SplitBy(",").As<int>().Into(coords => new Point4(coords[0], coords[1], coords[2], coords[3]));
}