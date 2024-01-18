using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2020.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader inputReader)
{
    private readonly DirectedGraph<string, int> _bagGraph = new();

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in inputReader.ReadLinesAsync(7))
        {
            var split = line.Split("contain");
            var m = Regex.Match(split[0].Trim(), @"([a-z]+ [a-z]+) bags");

            var parentBag = m.Groups[1].Value;
            _bagGraph.AddVertex(parentBag);

            var contents = split[1].Split(",");
            foreach (var content in contents)
            {
                m = Regex.Match(content, @"(\d+) ([a-z]+ [a-z]+) bags?");
                if (!m.Success) continue;

                var amount = int.Parse(m.Groups[1].Value);
                var childBag = m.Groups[2].Value;

                _bagGraph.AddVertex(childBag);
                _bagGraph.AddEdge(parentBag, childBag, amount);
            }
        }
    }

    [Part1]
    public string Part1()
    {
        var count = 0;
        var closedSet = new HashSet<string>();
        var openSet = new Stack<string>(_bagGraph.InEdges("shiny gold").Keys);

        while (openSet.Count > 0)
        {
            var source = openSet.Pop();

            if (closedSet.Contains(source)) continue;

            closedSet.Add(source);
            count++;

            foreach (var target in _bagGraph.InEdges(source).Keys)
                if (!closedSet.Contains(target))
                    openSet.Push(target);
        }

        return count.ToString();
    }

    [Part2]
    public string Part2()
    {
        var count = CountBags("shiny gold");
        return count.ToString();
    }

    private int CountBags(string vertex)
    {
        var outEdges = _bagGraph.OutEdges(vertex);
        return outEdges.Sum(edge => edge.Value + edge.Value * CountBags(edge.Key));
    }
}